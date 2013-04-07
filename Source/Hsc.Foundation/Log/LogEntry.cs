using System;
using System.Text;

namespace Hsc.Foundation.Log
{
    public class LogEntry
    {
        public LogEntry()
        {
            Level = LogLevel.Debug;
        }

        public LogLevel Level { get; set; }

        public string Message { get; set; }

        public string Cause { get; set; }

        public Exception Exception { get; set; }

        public string Resolution { get; set; }

        public int? EventId { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <remarks>
        /// If this needs to be updated, extract it to a logEntryFormatter and wire it appropriately.
        /// </remarks>
        /// <param name="includeException">If set to <c>true</c> the exception will be included in the message.</param>
        public string ToString(bool includeException)
        {
            var stringBuilder = new StringBuilder();
            if (EventId.HasValue)
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("EVENT ID : " + EventId.Value);
            }
            if (!string.IsNullOrEmpty(Message))
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("MESSAGE : " + Message);
            }
            if (!string.IsNullOrEmpty(Cause))
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("CAUSE : " + Cause);
            }
            if (!string.IsNullOrEmpty(Resolution))
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("RESOLUTION : " + Resolution);
            }

            if (includeException && Exception != null)
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("EXCEPTION : " + Exception);
            }

            return stringBuilder.ToString();
        }

        public override string ToString()
        {
            return ToString(true);
        }
    }
}
