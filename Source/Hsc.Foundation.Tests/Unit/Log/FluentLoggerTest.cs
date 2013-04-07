using Hsc.Foundation.Log;
using NUnit.Framework;
using Rhino.Mocks;

namespace Hsc.Foundation.Tests.Unit.Log
{
    [TestFixture]
    internal class FluentLoggerTest
    {
        private ILogger _logger;

        [SetUp]
        public void Setup()
        {
            _logger = MockRepository.GenerateStub<ILogger>();
        }

        [TearDown]
        public void Teardown()
        {
            _logger = null;
        }

        [Test]
        public void Create()
        {
            var fluentLogger = new FluentLogger(_logger);

            fluentLogger.Create().Write();

            _logger.AssertWasCalled(logger => logger.Write(Arg<LogEntry>.Is.Anything));
        }

        [Test]
        public void CreateWithMessage()
        {
            var fluentLogger = new FluentLogger(_logger);

            fluentLogger.CreateWithMessage("foo {0} baz", "bar").Write();

            _logger.AssertWasCalled(
                logger => logger.Write(Arg<LogEntry>.Matches(logEntry => logEntry.Message == "foo bar baz")));
        }
    }
}