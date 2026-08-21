namespace back_end.Tests.Integration.Fixtures
{
    public abstract class BaseIntegrationTest 
    {
        protected readonly CustomWebAppFactory Factory;
        protected readonly HttpClient Client;

        protected BaseIntegrationTest(CustomWebAppFactory factory)
        {
            Factory = factory;
            Client = factory.Client;
        }
    }
}

