using System;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using MemoryApi.Core.Database.Repositories;
using MemoryApi.Functions.Auth;
using MemoryApi.Functions.Database.Repositories.Impl;
using MemoryServer.Core.Database.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Practices.ServiceLocation;

namespace MemoryApi.Functions
{
    public class ServiceLocator
    {
        public ServiceLocator(Action<ContainerBuilder> customRegistrations)
        {
            Build(customRegistrations);
        }

        public ServiceLocator()
        {
            Build(a => {});
        }

        public IServiceLocator Instance { get; private set; }

        private void Build(Action<ContainerBuilder> customRegistrations)
        {
            var builder = new ContainerBuilder();

            // Register dependencies.
            builder.RegisterType<DocumentDbLessonRepository>().As<ILessonRepository>().InstancePerDependency();

            builder.RegisterType<AzureAdAuthenticationService>().As<IAuthenticationService>().InstancePerDependency();

            customRegistrations(builder);

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