using System;

namespace modules
{
    public class Configuration
    {
        private AuthApiClientConfig authApiClientConfig = new AuthApiClientConfig();
        private PricingApiClientConfig pricingApiClientConfig = new PricingApiClientConfig();
        private BillingApiClientConfig billingApiClientConfig = new BillingApiClientConfig();
        private ApiLogApiClientConfig apiLogApiClientConfig = new ApiLogApiClientConfig();
        private CommunicationApiClientConfig communicationApiClientConfig = new CommunicationApiClientConfig();

        private AwsMarketplaceApiClientConfig awsMarketplaceApiClientConfig = new AwsMarketplaceApiClientConfig();
        private IntegrationApiClientConfig integrationApiClientConfig = new IntegrationApiClientConfig();
        public Configuration()
        {
            string urlBase = Environment.GetEnvironmentVariable("SAASUS_API_URL_BASE");
            if (urlBase != null)
            {
                authApiClientConfig.BasePath = $"{urlBase}/v1/auth";
                pricingApiClientConfig.BasePath = $"{urlBase}/v1/pricing";
                billingApiClientConfig.BasePath = $"{urlBase}/v1/billing";
                apiLogApiClientConfig.BasePath = $"{urlBase}/v1/apilog";
                communicationApiClientConfig.BasePath = $"{urlBase}/v1/communication";
                awsMarketplaceApiClientConfig.BasePath = $"{urlBase}/v1/awsmarketplace";
                integrationApiClientConfig.BasePath = $"{urlBase}/v1/integration";
            }
        }

        public AuthApiClientConfig GetAuthApiClientConfig()
        {
            return authApiClientConfig;
        }

        public PricingApiClientConfig GetPricingApiClientConfig()
        {
            return pricingApiClientConfig;
        }

        public BillingApiClientConfig GetBillingApiClientConfig()
        {
            return billingApiClientConfig;
        }

        public ApiLogApiClientConfig GetApiLogApiClientConfig()
        {
            return apiLogApiClientConfig;
        }

        public CommunicationApiClientConfig GetCommunicationApiClientConfig()
        {
            return communicationApiClientConfig;
        }

        public AwsMarketplaceApiClientConfig GetAwsMarketplaceApiClientConfig()
        {
            return awsMarketplaceApiClientConfig;
        }

        public IntegrationApiClientConfig GetIntegrationApiClientConfig()
        {
            return integrationApiClientConfig;
        }
    }
}
