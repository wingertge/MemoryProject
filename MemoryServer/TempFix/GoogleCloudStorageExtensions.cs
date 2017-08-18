using System;
using CloudStorage.Wrapper;
using Microsoft.Extensions.Caching.Distributed;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class GoogleCloudStorageExtensions
    {
        public static void AddDistributedCloudStorageCache(this IServiceCollection services, Action<GoogleCloudStorageCacheOptions> optionAction)
        {
            services.Configure(optionAction);
            services.AddScoped<IDistributedCache, GoogleCloudStorageCache>();
        }
    }
}