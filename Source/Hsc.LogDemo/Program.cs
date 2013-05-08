using Microsoft.Practices.Unity;

namespace Hsc.LogDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IUnityContainer conatiner = ContainerConfiguration.GetUnityContainer();

            var logDemo = conatiner.Resolve<ILogDemo>();

            logDemo.Run();
        }
    }
}