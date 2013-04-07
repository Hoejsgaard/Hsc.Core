using Microsoft.Practices.Unity;

namespace Hsc.LogDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            IUnityContainer conatiner = ContainerConfiguration.GetUnityContainer();

            var logDemo = conatiner.Resolve<ILogDemo>();

            logDemo.Run();
        }
    }
}
