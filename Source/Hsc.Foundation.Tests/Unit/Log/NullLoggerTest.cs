using Hsc.Foundation.Log;
using NUnit.Framework;

namespace Hsc.Foundation.Tests.Unit.Log
{
    [TestFixture]
    class NullLoggerTest
    {
        NullLogger _nullLogger;
        LogEntryBuilder _logEntryBuilder;

        [SetUp]
        public void Setup()
        {
            _nullLogger = new NullLogger();
            _logEntryBuilder = new LogEntryBuilder(_nullLogger);
        }

        [TearDown]
        public void Teardown()
        {
            _nullLogger = null;
        }

        [Test]
        public void Write()
        {
            var etntry = _logEntryBuilder.WithMessage("foo").LogEntry;
            _nullLogger.Write(etntry);
        }

        [Test]
        public void Flush()
        {
            _nullLogger.Flush();
        }
    }
}
