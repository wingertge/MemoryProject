using JetBrains.Annotations;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Config;

namespace MemoryApi.Functions.Bindings
{
    public static class JobHostConfigurationExtensions
    {
        public static void UseMemoryExtensions(this JobHostConfiguration config)
        {
            config.RegisterExtensionConfigProvider(new MemoryApiExtensionConfig());
        }

        private class MemoryApiExtensionConfig : IExtensionConfigProvider
        {
            public void Initialize([NotNull] ExtensionConfigContext context)
            {
                context.Config.RegisterBindingExtensions(new ServiceLocatorAttributeBindingProvider());
            }
        }
    }
}