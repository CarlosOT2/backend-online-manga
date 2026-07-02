
using back_end.Conventions;
using back_end.Data;
using back_end.Shared.Cache;
using back_end.Shared.Settings;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using back_end.Database.Seeds;

namespace back_end
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //# Services
            builder.Services.AddControllers(options =>
            {
                if (builder.Environment.IsProduction())
                    options.Conventions.Add(new RemoveControllerConvention("Seeds"));
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //? DB
            builder.Services.AddDbContext<AppDbContext>(options =>
               options.UseNpgsql(
                   builder.Configuration.GetConnectionString("Default"),
                   o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
               )
            );

            //? Dependency Injection
            builder.Services.AddScoped<Database.DbAccess.Interfaces.ITitle, Database.DbAccess.Title>();
            builder.Services.AddScoped<Database.DbAccess.Interfaces.IStatic, Database.DbAccess.Static>();

            builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));
            builder.Services.Configure<ValidationSettings>(builder.Configuration.GetSection("Database:Tables:Validation"));

            builder.Services.AddSingleton<CacheHandler>();
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                string? connectionstring = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connectionstring!);
            });

            //? Cors
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("front-end",
                    builder => builder
                        .WithOrigins(
                            "http://localhost:5173",
                            "https://frontend-online-manga.vercel.app"
                            )
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            //# App
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            if (app.Environment.IsProduction())
            {
                using (IServiceScope scope = app.Services.CreateScope())
                {
                    AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    int retries = 10;
                    while (retries > 0)
                    {
                        try
                        {
                            db.Database.Migrate();
                            break;
                        }
                        catch (Exception ex)
                        {
                            retries--;
                            Console.WriteLine($"Error applying migration, retrying... ({retries} attempts left): {ex.Message}");
                            if (retries == 0) throw;
                            await Task.Delay(3000);
                        }
                    }

                    Seeder seeder = new Seeder(db);
                    await seeder.Run(500);
                }
            }

            app.UseCors("front-end");
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
