using System;
using Hsc.Foundation.Log;
using NUnit.Framework;
using Rhino.Mocks;

namespace Hsc.Foundation.Tests.Unit.Log
{
    [TestFixture]
    internal class LogEntryBuilderTest
    {
        private ILogger _logger;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _logger = MockRepository.GenerateStub<ILogger>();
        }


        private void AsMethod_SetErrorLevel(Func<LogEntryBuilder, LogEntryBuilder> asMethod, LogLevel logLevel)
        {
            var logEntryBuilder = new LogEntryBuilder(_logger);
            asMethod.Invoke(logEntryBuilder).Write();
            _logger.AssertWasCalled(logger => logger.Write(Arg<LogEntry>.Matches(logEntry => logEntry.Level == logLevel)));
        }

        [Test]
        public void AsDebug_SetErrorLevel()
        {
            AsMethod_SetErrorLevel(logEntryBuilder => logEntryBuilder.AsDebug(), LogLevel.Debug);
        }

        [Test]
        public void AsError_SetErrorLevel()
        {
            AsMethod_SetErrorLevel(logEntryBuilder => logEntryBuilder.AsError(), LogLevel.Error);
        }

        [Test]
        public void AsFatal_SetErrorLevel()
        {
            AsMethod_SetErrorLevel(logEntryBuilder => logEntryBuilder.AsFatal(), LogLevel.Fatal);
        }

        [Test]
        public void AsInfo_SetErrorLevel()
        {
            AsMethod_SetErrorLevel(logEntryBuilder => logEntryBuilder.AsInfo(), LogLevel.Info);
        }

        [Test]
        public void AsTrace_SetErrorLevel()
        {
            AsMethod_SetErrorLevel(logEntryBuilder => logEntryBuilder.AsTrace(), LogLevel.Trace);
        }

        [Test]
        public void AsWarning_SetErrorLevel()
        {
            AsMethod_SetErrorLevel(logEntryBuilder => logEntryBuilder.AsWarning(), LogLevel.Warn);
        }

        [Test]
        public void Create()
        {
            var logEntryBuilder = new LogEntryBuilder(_logger);

            LogEntry entry = logEntryBuilder.LogEntry;

            Assert.NotNull(entry);
            Assert.That(entry.Level == LogLevel.Debug);
        }

        [Test]
        public void CreateWithMessage()
        {
            var logEntryBuilder = new LogEntryBuilder(_logger);

            logEntryBuilder.WithMessage("foo {0} baz", "bar");

            Assert.That(logEntryBuilder.LogEntry.Message == "foo bar baz");
        }

        [Test]
        public void WithAllSetters()
        {
            var logEntryBuilder = new LogEntryBuilder(_logger);

            var exception = new DuplicateWaitObjectException("foo");
            logEntryBuilder.WithMessage("foo {0}", "bar")
                           .AsFatal()
                           .WithEventId(42)
                           .WithCause("cause")
                           .WithResolution("resolution")
                           .WithException(exception)
                           .Write();

            _logger.AssertWasCalled(logger => logger.Write(Arg<LogEntry>.Matches(logEntry =>
                                                                                 logEntry.Message == "foo bar" &&
                                                                                 logEntry.Level == LogLevel.Fatal &&
                                                                                 logEntry.EventId == 42 &&
                                                                                 logEntry.Cause == "cause" &&
                                                                                 logEntry.Resolution == "resolution" &&
                                                                                 logEntry.Exception == exception)));
        }

        [Test]
        public void WithCause()
        {
            var logEntryBuilder = new LogEntryBuilder(_logger);

            logEntryBuilder.WithCause("cause").Write();

            _logger.AssertWasCalled(logger => logger.Write(Arg<LogEntry>.Matches(logEntry => logEntry.Cause == "cause")));
        }

        [Test]
        public void WithEventId()
        {
            var logEntryBuilder = new LogEntryBuilder(_logger);

            logEntryBuilder.WithEventId(42).Write();

            _logger.AssertWasCalled(logger => logger.Write(Arg<LogEntry>.Matches(logEntry => logEntry.EventId == 42)));
        }

        [Test]
        public void WithException()
        {
            var logEntryBuilder = new LogEntryBuilder(_logger);

            var exception = new DuplicateWaitObjectException("foo");
            logEntryBuilder.WithException(exception).Write();

            _logger.AssertWasCalled(logger => logger.Write(Arg<LogEntry>.Matches(logEntry => logEntry.Exception == exception)));
        }

        [Test]
        public void WithMessageFormat()
        {
            var logEntryBuilder = new LogEntryBuilder(_logger);

            logEntryBuilder.WithMessage("{0} message", "composite").Write();

            _logger.AssertWasCalled(logger => logger.Write(Arg<LogEntry>.Matches(logEntry => logEntry.Message == "composite message")));
        }

        [Test]
        public void WithMessageNoFormat()
        {
            var logEntryBuilder = new LogEntryBuilder(_logger);

            logEntryBuilder.WithMessage("message").Write();

            _logger.AssertWasCalled(logger => logger.Write(Arg<LogEntry>.Matches(logEntry => logEntry.Message == "message")));
        }

        [Test]
        public void WithResolution()
        {
            var logEntryBuilder = new LogEntryBuilder(_logger);

            logEntryBuilder.WithResolution("resolution").Write();

            _logger.AssertWasCalled(logger => logger.Write(Arg<LogEntry>.Matches(logEntry => logEntry.Resolution == "resolution")));
        }

        [Test]
        public void Write_CallsLogger()
        {
            var logEntryBuilder = new LogEntryBuilder(_logger);

            logEntryBuilder.Write();

            _logger.AssertWasCalled(logger => logger.Write(Arg<LogEntry>.Is.Anything));
        }
    }
}