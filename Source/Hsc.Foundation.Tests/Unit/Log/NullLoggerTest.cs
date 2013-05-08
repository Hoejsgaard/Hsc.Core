using Hsc.Foundation.Log;
using NUnit.Framework;

namespace Hsc.Foundation.Tests.Unit.Log
{
    [TestFixture]
    internal class NullLoggerTest
    {
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

        private NullLogger _nullLogger;
        private LogEntryBuilder _logEntryBuilder;

        [Test]
        public void Flush()
        {
            _nullLogger.Flush();
        }

        [Test]
        public void Write()
        {
            LogEntry etntry = _logEntryBuilder.WithMessage("foo").LogEntry;
            _nullLogger.Write(etntry);
        }
    }
}