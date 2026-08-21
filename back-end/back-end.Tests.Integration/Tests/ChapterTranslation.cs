using System.Net;
using back_end.Tests.Integration.Fixtures;

namespace back_end.Tests.Integration.Tests
{
    [Collection(IntegrationTestCollection.Name)]
    public class ChapterTranslation : BaseIntegrationTest
    {
        public ChapterTranslation(CustomWebAppFactory factory) : base(factory) { }

        [Fact]
        public async Task GetChapterTranslation()
        {
            HttpResponseMessage response = await Client.GetAsync("/ChapterTranslation?id=1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
