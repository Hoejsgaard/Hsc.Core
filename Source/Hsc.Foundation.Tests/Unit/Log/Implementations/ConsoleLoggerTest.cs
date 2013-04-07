using Exformatics.Foundation.Log;
using Exformatics.Foundation.Log.Implementations;
using NUnit.Framework;

namespace Exformatics.Foundation.Tests.Unit.Log.Implementations
{
    [TestFixture]
    class ConsoleLoggerTest
    {
        [Test]
        public void Write()
        {
            var consoleLogger = new ConsoleLogger();

            consoleLogger.Write(new LogEntry());
        }

        [Test]
        public void Flush()
        {
            var consoleLogger = new ConsoleLogger();

            consoleLogger.Flush();
        }
    }
}
