using Hsc.Repository;
using Hsc.SqlRepository;
using Hsc.SqlRepository.Knowledge;
using Hsc.SqlRepository.Operation;
using Microsoft.Practices.Unity;

namespace Hsc.TestApplication
{
    public class ContainerConfiguration
    {
        public static IUnityContainer GetUnityContainer()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<IConnectionProvider, HardcodedConnectionProvider>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDataTypeConverter, DataTypeConverter>(new ContainerControlledLifetimeManager());
            container.RegisterType<IEntityAttributeTypeRepository, EntityAttributeTypeRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISqlAttributeTypeRepository, AttributeTypeRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISqlEntityTypeRepository, SqlEntityTypeRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IEntityTypeRepository, SqlEntityTypeRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IEntityRepository, SqlEntityRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISqlRepository, SqlRepository.SqlRepository>(new ContainerControlledLifetimeManager());

            return container;
        }
    }
}