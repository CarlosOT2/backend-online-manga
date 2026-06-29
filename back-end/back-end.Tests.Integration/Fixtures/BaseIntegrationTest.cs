using System.Text;
using System.Text.Json;

namespace back_end.Tests.Integration.Fixtures
{
    public abstract class BaseIntegrationTest : IAsyncLifetime
    {
        protected readonly CustomWebAppFactory Factory;
        protected readonly HttpClient Client;

        protected BaseIntegrationTest(CustomWebAppFactory factory)
        {
            Factory = factory;
            Client = factory.Client;
        }

        public async Task InitializeAsync()
        {
            await Factory.ResetDatabaseAsync();
            await SeedAsync();
        }

        protected async Task SeedAsync()
        {
            HttpResponseMessage? responseDelete = await Client.DeleteAsync("/Seeds");
            responseDelete.EnsureSuccessStatusCode();

            HttpResponseMessage? responseStatic = await Client.PostAsync("/Seeds/Static", null);
            responseStatic.EnsureSuccessStatusCode();

            StringContent body = new StringContent(
                JsonSerializer.Serialize(100),
                Encoding.UTF8,
                "application/json"
            );
            HttpResponseMessage? responseSeed = await Client.PostAsync("/Seeds/Seed", body);
            responseSeed.EnsureSuccessStatusCode();
        }

        public Task DisposeAsync() => Task.CompletedTask;
    }
}

