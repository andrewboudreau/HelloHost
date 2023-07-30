namespace HelloHost.Application
{
    public class AppSettings
    {
        public AppSettings()
        {
            AzureServiceBus = "Endpoint=sb://<your_service_bus>.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=<your_shared_access_key>";
            AzureServiceBusQueue = "host.all";
            AzureStorage = "DefaultEndpointsProtocol=https;AccountName=<your_storage_account>;AccountKey=<your_storage_key>;EndpointSuffix=core.windows.net";
            AzureStorageContainer = "host";
        }

        public string AzureServiceBus { get; set; }
        public string AzureServiceBusQueue { get; set; }
        public string AzureStorage { get; set; }
        public string AzureStorageContainer { get; set; }
    }
}
