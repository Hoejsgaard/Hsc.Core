using Hsc.Foundation.Log;
using Hsc.Foundation.Log.Nlog;
using Microsoft.Practices.Unity;

namespace Hsc.LogDemo
{
    public class ContainerConfiguration
    {
        public static IUnityContainer GetUnityContainer()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<INLogConfigurationRepository, MockNlogConfigurationRepository>(
                new ContainerControlledLifetimeManager());
            container.RegisterType<ILogEventInfoSplitter, LogEventInfoSplitter>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILogger, NLogLogger>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILogBuilderFactory, FluentLogger>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILogDemo, LogDemo>(new ContainerControlledLifetimeManager());

            return container;
        }
    }
}