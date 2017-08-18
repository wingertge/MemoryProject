using System;
using System.IO;
using System.Threading.Tasks;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace CloudStorage.Wrapper
{
    public class GoogleCloudStorageCache : IDistributedCache
    {
        private readonly StorageClient _storageClient;
        private readonly GoogleCloudStorageCacheOptions _options;
        private readonly Bucket _bucket;

        public GoogleCloudStorageCache(IOptions<GoogleCloudStorageCacheOptions> options)
        {
            _options = options.Value;
            _storageClient = options.Value.EncryptionKey != null ? StorageClient.Create(_options.Credentials, options.Value.EncryptionKey) : StorageClient.Create(_options.Credentials);

            try
            {
                _storageClient.CreateBucket(_options.ProjectName, _options.BucketName);
            }
            catch (Google.GoogleApiException e)
            when(e.Error.Code == 409)
            {
                //Bucket exists, no error
                Console.WriteLine(e.Error.Message);
            }
        }

        public byte[] Get(string key)
        {
            var stream = new MemoryStream();
            _storageClient.DownloadObject(_options.BucketName, key, stream);
            return stream.ToArray();
        }

        public async Task<byte[]> GetAsync(string key)
        {
            var stream = new MemoryStream();
            await _storageClient.DownloadObjectAsync(_options.BucketName, key, stream);
            return stream.ToArray();
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            var stream = new MemoryStream(value);
            _storageClient.UploadObject(_options.BucketName, key, "application/octet-stream", stream);
        }

        public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            var stream = new MemoryStream(value);
            await _storageClient.UploadObjectAsync(_options.BucketName, key, "application/octet-stream", stream);
        }

        //meaningless in this context
        public void Refresh(string key)
        {
        }

        //meaningless in this context
        public Task RefreshAsync(string key)
        {
            return Task.CompletedTask;
        }

        public void Remove(string key)
        {
            _storageClient.DeleteObject(_options.BucketName, key);
        }

        public Task RemoveAsync(string key)
        {
            return _storageClient.DeleteObjectAsync(_options.BucketName, key);
        }
    }
}
