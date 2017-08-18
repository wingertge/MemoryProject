using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace CloudStorage.Wrapper
{
    public class GoogleCloudStorageCacheOptions
    {
        public GoogleCredential Credentials { get; set; }
        public string BucketName { get; set; } = "dotnet-bucket";
        public EncryptionKey EncryptionKey { get; set; } = null;
        public string ProjectName { get; set; }
    }
}