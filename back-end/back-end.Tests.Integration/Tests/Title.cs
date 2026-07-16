using System.Net;
using System.Text.Json;
using back_end.Tests.Integration.Fixtures;
namespace back_end.Tests.Integration.Tests
{
    [Collection(IntegrationTestCollection.Name)]
    public class Title : BaseIntegrationTest
    {
        public Title(CustomWebAppFactory factory) : base(factory) { }
        [Fact]
        public async Task GetDefault()
        {
            // With id
            HttpResponseMessage responseId = await Client.GetAsync("/Title?id=1");
            Assert.Equal(HttpStatusCode.OK, responseId.StatusCode);
            // Without parameters → 400
            HttpResponseMessage responseNoParams = await Client.GetAsync("/Title");
            Assert.Equal(HttpStatusCode.BadRequest, responseNoParams.StatusCode);
            // With nonexistent id → 404
            HttpResponseMessage responseNotFound = await Client.GetAsync("/Title?id=999999");
            Assert.Equal(HttpStatusCode.NotFound, responseNotFound.StatusCode);
            // Verify response body with id
            string bodyId = await responseId.Content.ReadAsStringAsync();
            List<DTOs.Title>? titlesById = JsonSerializer.Deserialize<List<DTOs.Title>>(bodyId, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.NotNull(titlesById);
        }

        [Fact]
        public async Task GetLatestUpdates()
        {
            // With limit
            HttpResponseMessage responseLimit = await Client.GetAsync("/Title/latestupdates?limit=10");
            Assert.Equal(HttpStatusCode.OK, responseLimit.StatusCode);
            // Without limit → 400
            HttpResponseMessage responseNoParams = await Client.GetAsync("/Title/latestupdates");
            Assert.Equal(HttpStatusCode.BadRequest, responseNoParams.StatusCode);
            // Exceeding max limit → 500
            HttpResponseMessage responseExceedLimit = await Client.GetAsync("/Title/latestupdates?limit=101");
            Assert.Equal(HttpStatusCode.InternalServerError, responseExceedLimit.StatusCode);
            // With compact=false
            HttpResponseMessage responseNotCompact = await Client.GetAsync("/Title/latestupdates?limit=10&compact=false");
            Assert.Equal(HttpStatusCode.OK, responseNotCompact.StatusCode);
            // Verify response body
            string body = await responseLimit.Content.ReadAsStringAsync();
            List<DTOs.Title>? titles = JsonSerializer.Deserialize<List<DTOs.Title>>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.NotNull(titles);
        }

        [Fact]
        public async Task GetRecentlyAdded()
        {
            // With limit
            HttpResponseMessage responseLimit = await Client.GetAsync("/Title/recentlyadded?limit=10");
            Assert.Equal(HttpStatusCode.OK, responseLimit.StatusCode);
            // Without limit → 400
            HttpResponseMessage responseNoParams = await Client.GetAsync("/Title/recentlyadded");
            Assert.Equal(HttpStatusCode.BadRequest, responseNoParams.StatusCode);
            // Exceeding max limit → 500
            HttpResponseMessage responseExceedLimit = await Client.GetAsync("/Title/recentlyadded?limit=101");
            Assert.Equal(HttpStatusCode.InternalServerError, responseExceedLimit.StatusCode);
            // With compact=false
            HttpResponseMessage responseNotCompact = await Client.GetAsync("/Title/recentlyadded?limit=10&compact=false");
            Assert.Equal(HttpStatusCode.OK, responseNotCompact.StatusCode);
            // Verify response body
            string body = await responseLimit.Content.ReadAsStringAsync();
            List<DTOs.Title>? titles = JsonSerializer.Deserialize<List<DTOs.Title>>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.NotNull(titles);
        }

        [Fact]
        public async Task GetFeatured()
        {
            // With limit
            HttpResponseMessage responseLimit = await Client.GetAsync("/Title/featured?limit=10");
            Assert.Equal(HttpStatusCode.OK, responseLimit.StatusCode);
            // Without limit → 400
            HttpResponseMessage responseNoParams = await Client.GetAsync("/Title/featured");
            Assert.Equal(HttpStatusCode.BadRequest, responseNoParams.StatusCode);
            // Exceeding max limit → 500
            HttpResponseMessage responseExceedLimit = await Client.GetAsync("/Title/featured?limit=101");
            Assert.Equal(HttpStatusCode.InternalServerError, responseExceedLimit.StatusCode);
            // Verify response body
            string body = await responseLimit.Content.ReadAsStringAsync();
            List<DTOs.Title>? titles = JsonSerializer.Deserialize<List<DTOs.Title>>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.NotNull(titles);
        }

        [Fact]
        public async Task GetSearch()
        {
            // No filters
            HttpResponseMessage noFilters = await Client.GetAsync("/Title/search");
            Assert.Equal(HttpStatusCode.OK, noFilters.StatusCode);
            // With name
            HttpResponseMessage withName = await Client.GetAsync("/Title/search?name=naruto");
            Assert.Equal(HttpStatusCode.OK, withName.StatusCode);
            // With genresIds
            HttpResponseMessage withGenres = await Client.GetAsync("/Title/search?genresIds=1&genresIds=2");
            Assert.Equal(HttpStatusCode.OK, withGenres.StatusCode);
            // With excludeGenresIds
            HttpResponseMessage withExcludeGenres = await Client.GetAsync("/Title/search?excludeGenresIds=1&excludeGenresIds=2");
            Assert.Equal(HttpStatusCode.OK, withExcludeGenres.StatusCode);
            // Conflict genre include + exclude same id → 500
            HttpResponseMessage genreConflict = await Client.GetAsync("/Title/search?genresIds=1&excludeGenresIds=1");
            Assert.Equal(HttpStatusCode.InternalServerError, genreConflict.StatusCode);
            // Conflict theme include + exclude same id → 500
            HttpResponseMessage themeConflict = await Client.GetAsync("/Title/search?themesIds=2&excludeThemesIds=2");
            Assert.Equal(HttpStatusCode.InternalServerError, themeConflict.StatusCode);
            // Multiple filters combined
            HttpResponseMessage multipleFilters = await Client.GetAsync("/Title/search?name=one&genresIds=1&statusIds=2&publicationYear=2000");
            Assert.Equal(HttpStatusCode.OK, multipleFilters.StatusCode);
            // Verify response body
            string body = await withName.Content.ReadAsStringAsync();
            List<DTOs.Title>? titles = JsonSerializer.Deserialize<List<DTOs.Title>>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.NotNull(titles);
        }
    }
}