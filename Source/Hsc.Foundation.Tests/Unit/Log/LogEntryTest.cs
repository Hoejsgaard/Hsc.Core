using System;
using Hsc.Foundation.Log;
using NUnit.Framework;

namespace Hsc.Foundation.Tests.Unit.Log
{
    [TestFixture]
    class LogEntryTest
    {
        private LogEntry GetPopulatedLogEntry()
        {
            return new LogEntry
            {
                Message = "Testmessage",
                Cause = "Testcause",
                Resolution = "Testresolution",
                EventId = 42,
                Exception = new FormatException("Exceptionmessage")
            };
        }

        [Test]
        public void ToString_IncludeMessage()
        {
            LogEntry logEntry = GetPopulatedLogEntry();

            string stringRepresentation = logEntry.ToString();

            Assert.That(stringRepresentation.Contains("Testmessage"));
        }

        [Test]
        public void ToString_IncludeCause()
        {
            LogEntry logEntry = GetPopulatedLogEntry();

            string stringRepresentation = logEntry.ToString();

            Assert.That(stringRepresentation.Contains("Testcause"));
        }

        [Test]
        public void ToString_IncludeResolution()
        {
            LogEntry logEntry = GetPopulatedLogEntry();

            string stringRepresentation = logEntry.ToString();

            Assert.That(stringRepresentation.Contains("Testresolution"));
        }

        [Test]
        public void ToString_IncludeEventId()
        {
            LogEntry logEntry = GetPopulatedLogEntry();

            string stringRepresentation = logEntry.ToString();

            Assert.That(stringRepresentation.Contains("42"));
        }

        [Test]
        public void ToString_ExcludeException_WhenPassingFalse()
        {
            LogEntry logEntry = GetPopulatedLogEntry();

            string stringRepresentation = logEntry.ToString(false);

            Assert.That(!stringRepresentation.Contains("Exceptionmessage"));
        }

        [Test]
        public void ToString_IncludeException_WhenPassingTrue()
        {
            LogEntry logEntry = GetPopulatedLogEntry();

            string stringRepresentation = logEntry.ToString(true);

            Assert.That(stringRepresentation.Contains("Exceptionmessage"));
        }

        [Test]
        public void ToString_IncludeException()
        {
            LogEntry logEntry = GetPopulatedLogEntry();

            string stringRepresentation = logEntry.ToString();

            Assert.That(stringRepresentation.Contains("Exceptionmessage"));
        }

        [Test]
        public void ToString_ReturnsEmptyString_WhenLogEntryIsEmpty()
        {
            LogEntry logEntry = new LogEntry();

            string stringRepresentation = logEntry.ToString();

            Assert.IsEmpty(stringRepresentation);
        }

        [Test]
        public void ToString_RetunsCorrectFormat()
        {
            LogEntry logEntry = GetPopulatedLogEntry();

            
            string stringRepresentation = logEntry.ToString();
            string expectedSerializationFormat = "\r\n" +
                                                 "EVENT ID : 42\r\n\r\n" +
                                                 "MESSAGE : Testmessage\r\n\r\n" +
                                                 "CAUSE : Testcause\r\n\r\n" +
                                                 "RESOLUTION : Testresolution\r\n\r\n" +
                                                 "EXCEPTION : System.FormatException: Exceptionmessage\r\n";
            
            Assert.AreEqual(expectedSerializationFormat, stringRepresentation);
        }
    }
}
