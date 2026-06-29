namespace back_end.Tests.Integration.Fixtures
{
    [CollectionDefinition(IntegrationTestCollection.Name)]
    public class IntegrationTestCollection : ICollectionFixture<CustomWebAppFactory>
    {
        public const string Name = "Integration Tests";
    }
}

