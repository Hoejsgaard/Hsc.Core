using System;
using System.Collections.Generic;
using Hsc.Foundation.Log.Nlog;
using NLog;
using NUnit.Framework;

namespace Hsc.Foundation.Tests.Unit.Log.Nlog
{
    [TestFixture]
    public class LogEventInfoSplitterTest
    {
        [SetUp]
        public void Setup()
        {
            _splitter = new LogEventInfoSplitter();
            _logEventInfo = new LogEventInfo();
        }

        [TearDown]
        public void Teardown()
        {
            _logEventInfo = null;
            _splitter = null;
        }

        private LogEventInfoSplitter _splitter;
        private LogEventInfo _logEventInfo;


        private int MaxSize
        {
            get { return _splitter.MessageSizeLimit - _splitter.ReservedForFormatting; }
        }

        [Test]
        public void Constructor_SetsDefaultValues()
        {
            Assert.That(_splitter.MessageSizeLimit, Is.EqualTo(8000));
            Assert.That(_splitter.ReservedForFormatting, Is.EqualTo(200));
        }

        [Test]
        public void GetEventFragments_GeneratesCorrectFooter_WhenSplitting()
        {
            _logEventInfo.Message = StringGenerator.BuildRandomString(MaxSize + 1);

            List<LogEventInfo> result = _splitter.GetEventFragments(_logEventInfo);

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Message, Is.StringContaining("SPLIT MESSAGE CONTINUES IN NEXT MESSAGE>"));
            Assert.That(result[1].Message, Is.StringContaining("<END OF SPLIT MESSAGE"));
        }

        [Test]
        public void GetEventFragments_GeneratesCorrectHeader_WhenSplitting()
        {
            _logEventInfo.Message = StringGenerator.BuildRandomString(MaxSize + 1);

            List<LogEventInfo> result = _splitter.GetEventFragments(_logEventInfo);

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Message, Is.StringContaining("PART 1/2 OF SPLIT MESSAGE>"));
            Assert.That(result[1].Message, Is.StringContaining("PART 2/2 OF SPLIT MESSAGE>"));
        }

        [Test]
        public void GetEventFragments_PreservesEventId_WhenSplitting()
        {
            _logEventInfo.Message = StringGenerator.BuildRandomString(MaxSize + 1);
            _logEventInfo.Properties["eventID"] = 42;

            List<LogEventInfo> result = _splitter.GetEventFragments(_logEventInfo);

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Properties["eventID"], Is.EqualTo(42));
            Assert.That(result[1].Properties["eventID"], Is.EqualTo(42));
        }

        [Test]
        public void GetEventFragments_PreservesException_WhenSplitting()
        {
            _logEventInfo.Message = StringGenerator.BuildRandomString(MaxSize + 1);
            var exception = new IndexOutOfRangeException("My exception");
            _logEventInfo.Exception = exception;

            List<LogEventInfo> result = _splitter.GetEventFragments(_logEventInfo);

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Exception, Is.EqualTo(exception));
            Assert.That(result[1].Exception, Is.EqualTo(exception));
        }

        [Test]
        public void GetEventFragments_PreservesLevel_WhenSplitting()
        {
            _logEventInfo.Message = StringGenerator.BuildRandomString(MaxSize + 1);
            _logEventInfo.Level = LogLevel.Fatal;

            List<LogEventInfo> result = _splitter.GetEventFragments(_logEventInfo);

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Level, Is.EqualTo(LogLevel.Fatal));
            Assert.That(result[1].Level, Is.EqualTo(LogLevel.Fatal));
        }

        [Test]
        public void GetEventFragments_PreservesOrIncrementsTimeStapm_WhenSplitting()
        {
            _logEventInfo.Message = StringGenerator.BuildRandomString(MaxSize + 1);
            var exception = new IndexOutOfRangeException("My exception");
            _logEventInfo.TimeStamp = DateTime.UtcNow;

            List<LogEventInfo> result = _splitter.GetEventFragments(_logEventInfo);

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].TimeStamp, Is.GreaterThanOrEqualTo(_logEventInfo.TimeStamp));
            Assert.That(result[1].TimeStamp, Is.GreaterThanOrEqualTo(result[0].TimeStamp));
            Assert.That(result[1].TimeStamp, Is.GreaterThanOrEqualTo(_logEventInfo.TimeStamp));
        }

        [Test]
        public void GetEventFragments_SplitsEvent_WhenItsMessageIsOverMaxSize()
        {
            _logEventInfo.Message = StringGenerator.BuildRandomString(MaxSize + 1);

            List<LogEventInfo> result = _splitter.GetEventFragments(_logEventInfo);

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetEventFragments_WrapsEvent_WhenItsMessageIsMaxSize()
        {
            _logEventInfo.Message = StringGenerator.BuildRandomString(MaxSize);

            List<LogEventInfo> result = _splitter.GetEventFragments(_logEventInfo);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(_logEventInfo));
        }

        [Test]
        public void GetEventFragments_WrapsEvent_WhenItsMessageIsUnderMaxSize()
        {
            _logEventInfo.Message = StringGenerator.BuildRandomString(MaxSize - 1);

            List<LogEventInfo> result = _splitter.GetEventFragments(_logEventInfo);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(_logEventInfo));
        }

        [Test]
        public void MessageSizeLimit_SetsValue_WhenAssigned()
        {
            _splitter.MessageSizeLimit = 42;
            Assert.That(_splitter.MessageSizeLimit, Is.EqualTo(42));
        }

        [Test]
        public void ReservedForFormatting_SetsValue_WhenAssigned()
        {
            _splitter.ReservedForFormatting = 42;
            Assert.That(_splitter.ReservedForFormatting, Is.EqualTo(42));
        }
    }
}