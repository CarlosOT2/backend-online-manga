using System.Net;
using System.Text.Json;
using back_end.Tests.Integration.Fixtures;

namespace back_end.Tests.Integration.Tests
{
    [Collection(IntegrationTestCollection.Name)]
    public class Static : BaseIntegrationTest
    {
        public Static(CustomWebAppFactory factory) : base(factory) { }

        [Fact]
        public async Task GetDefault()
        {
            HttpResponseMessage response = await Client.GetAsync("/Static");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            string body = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrWhiteSpace(body));

            JsonElement json = JsonSerializer.Deserialize<JsonElement>(body);
            Assert.True(json.EnumerateObject().Any());
        }
    }
}
