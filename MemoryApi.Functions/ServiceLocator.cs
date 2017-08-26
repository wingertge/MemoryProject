using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Practices.ServiceLocation;

namespace MemoryApi.Functions
{
    public class ServiceLocator
    {
        public ServiceLocator()
        {
            this.Build();
        }

        public IServiceLocator Instance { get; private set; }

        private void Build()
        {
            var builder = new ContainerBuilder();

            // Register dependencies.
            builder.RegisterType<AzureAdAuthenticationService>().As<IAuthenticationService>().InstancePerDependency();

            var container = builder.Build();

            // Create service locator.
            var csl = new AutofacServiceLocator(container);

            // Set the service locator created.
            Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(() => csl);

            // Use the service locator.
            this.Instance = Microsoft.Practices.ServiceLocation.ServiceLocator.Current;
        }
    }
}