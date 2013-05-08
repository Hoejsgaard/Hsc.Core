using System;
using System.Collections.Generic;
using Hsc.Foundation.Log;
using Hsc.Foundation.Log.Nlog;
using NLog;
using NUnit.Framework;
using Rhino.Mocks;
using LogLevel = NLog.LogLevel;

namespace Hsc.Foundation.Tests.Unit.Log.Nlog
{
    [TestFixture]
    internal class NlogLoggerTest
    {
        [SetUp]
        public void Setup()
        {
            _nlogConfigurationRepository = MockRepository.GenerateStub<INLogConfigurationRepository>();

            _logEventInfoSplitter = MockRepository.GenerateStub<ILogEventInfoSplitter>();
            var logEventInfos = new List<LogEventInfo> {new LogEventInfo(LogLevel.Debug, "foo", "bar")};
            _logEventInfoSplitter.Stub(x => x.GetEventFragments(null)).IgnoreArguments().Return(logEventInfos);

            _logger = MockRepository.GenerateMock<ILogger>();
            _builder = new LogEntryBuilder(_logger);
            _nlogLogger = new NLogLogger(_nlogConfigurationRepository, _logEventInfoSplitter);
        }

        [TearDown]
        public void Teardown()
        {
            _nlogConfigurationRepository = null;
            _logEventInfoSplitter = null;
            _logger = null;
            _builder = null;
            _nlogLogger = null;
        }

        private NLogLogger _nlogLogger;

        private INLogConfigurationRepository _nlogConfigurationRepository;
        private ILogEventInfoSplitter _logEventInfoSplitter;
        private ILogger _logger;
        private LogEntryBuilder _builder;

        [Test]
        public void Flush()
        {
            _nlogLogger.Flush();
        }

        [Test]
        public void Write_Debug()
        {
            _nlogLogger.Write(_builder.AsDebug().LogEntry);
        }

        [Test]
        public void Write_Error()
        {
            _nlogLogger.Write(_builder.AsError().LogEntry);
        }

        [Test]
        public void Write_Fatal()
        {
            _nlogLogger.Write(_builder.AsFatal().LogEntry);
        }

        [Test]
        public void Write_Info()
        {
            _nlogLogger.Write(_builder.AsInfo().LogEntry);
        }

        [Test]
        public void Write_OnConfigurationRepositoryChanged_TriggersGet()
        {
            _nlogConfigurationRepository.Raise(n => n.OnConfigurationChanged += null, _nlogConfigurationRepository, EventArgs.Empty);

            //Assert that GetConfiguration was called (triggered by _nlogLogger).
            _nlogConfigurationRepository.AssertWasCalled(ncr => ncr.GetConfiguration());
        }

        [Test]
        public void Write_Trace()
        {
            LogEntry entry = _builder.AsTrace().LogEntry;
            Assert.That(entry, Is.Not.Null);
            _nlogLogger.Write(entry);
        }

        [Test]
        public void Write_Warn()
        {
            _nlogLogger.Write(_builder.AsWarning().LogEntry);
        }

        [Test]
        public void Write_WithBigMessage()
        {
            _nlogLogger.Write(_builder.WithMessage(StringGenerator.BuildRandomString(70000)).LogEntry);
        }

        [Test]
        public void Write_WithEventId()
        {
            _nlogLogger.Write(_builder.WithEventId(42).LogEntry);
        }

        [Test]
        public void Write_WithException()
        {
            _nlogLogger.Write(_builder.WithException(new DuplicateWaitObjectException()).LogEntry);
        }

        [Test]
        public void Write_WithNull()
        {
            _nlogLogger.Write(null);
        }
    }
}