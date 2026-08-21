using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using StackExchange.Redis;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;
using back_end.Data;
using Microsoft.AspNetCore.TestHost;
using System.Text;
using System.Text.Json;

namespace back_end.Tests.Integration.Fixtures
{
    public class CustomWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder("postgres:16-alpine")
            .WithDatabase("onlinemanga")
            .WithUsername("testuser")
            .WithPassword("testpassword")
            .Build();

        private readonly RedisContainer _redisContainer = new RedisBuilder("redis:7-alpine")
            .Build();

        private Respawner _respawner = default!;
        private NpgsqlConnection _dbConnection = default!;

        public HttpClient Client { get; private set; } = default!;
        public string PostgresConnectionString => _postgresContainer.GetConnectionString();
        public string RedisConnectionString => _redisContainer.GetConnectionString();

        // Config
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:Default"] = _postgresContainer.GetConnectionString(),
                    ["ConnectionStrings:Redis"] = _redisContainer.GetConnectionString()
                });
            });

            builder.ConfigureTestServices(services =>
            {
                ServiceDescriptor? dbDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (dbDescriptor is not null)
                    services.Remove(dbDescriptor);
                services.AddDbContext<AppDbContext>(options => options.UseNpgsql(_postgresContainer.GetConnectionString()));

                ServiceDescriptor? redisDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IConnectionMultiplexer));
                if (redisDescriptor is not null)
                    services.Remove(redisDescriptor);
                services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(_redisContainer.GetConnectionString()));
            });
        }

        // Initialize / Dispose
        public async Task InitializeAsync()
        {
            await Task.WhenAll(
                _postgresContainer.StartAsync(),
                _redisContainer.StartAsync()
            );

            await ApplyMigrationsAsync();

            _dbConnection = new NpgsqlConnection(_postgresContainer.GetConnectionString());
            await _dbConnection.OpenAsync();

            _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = ["public"],
                TablesToIgnore = ["__EFMigrationsHistory"]
            });

            Client = CreateClient();
            Client.Timeout = TimeSpan.FromSeconds(200);
            await SeedAsync();
        }

        Task IAsyncLifetime.DisposeAsync()
        {
            return DisposeAsync().AsTask();
        }
        public override async ValueTask DisposeAsync()
        {
            if (_dbConnection is not null)
                await _dbConnection.DisposeAsync();

            await _postgresContainer.DisposeAsync();
            await _redisContainer.DisposeAsync();

            await base.DisposeAsync();
        }

        public async Task ResetDatabaseAsync()
        {
            await _respawner.ResetAsync(_dbConnection);
        }

        // Database
        private async Task ApplyMigrationsAsync()
        {
            using IServiceScope scope = Services.CreateScope();
            AppDbContext? db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.Database.MigrateAsync();
        }
        protected async Task SeedAsync()
        {
            HttpResponseMessage? responseStatic = await Client.PostAsync("/Seeds/Static", null);
            responseStatic.EnsureSuccessStatusCode();

            StringContent body = new StringContent(JsonSerializer.Serialize(50), Encoding.UTF8, "application/json");
            HttpResponseMessage? responseSeed = await Client.PostAsync("/Seeds/Seed", body);
            responseSeed.EnsureSuccessStatusCode();
        }
    }
}