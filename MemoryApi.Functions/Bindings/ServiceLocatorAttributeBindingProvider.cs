using System.Reflection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Practices.ServiceLocation;

namespace MemoryApi.Functions.Bindings
{
    public class ServiceLocatorAttributeBindingProvider : IArgumentBindingProvider<IServiceLocator>
    {
        public IServiceLocator TryCreate(ParameterInfo parameter)
        {
            return new ServiceLocator().Instance;
        }
    }
}