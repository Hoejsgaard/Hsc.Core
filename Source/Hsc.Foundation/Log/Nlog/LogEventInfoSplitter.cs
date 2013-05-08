using System;
using System.Collections.Generic;
using NLog;

namespace Hsc.Foundation.Log.Nlog
{
    public class LogEventInfoSplitter : ILogEventInfoSplitter
    {
        private const string EventId = "eventID";

        public LogEventInfoSplitter()
        {
            ReservedForFormatting = 200;
            MessageSizeLimit = 8000;
        }

        /// <summary>
        ///     Gets or sets the characters that has been reserved for formatting in NLog. Default value is 200.
        /// </summary>
        public int ReservedForFormatting { get; set; }

        /// <summary>
        ///     Gets or sets the message size limit. Default value is 8000.
        /// </summary>
        /// <remarks>
        ///     limit in DB: varchar(MAX) ~ 8000
        ///     For EventLog: 16-32 KB depending on the OS.
        /// </remarks>
        public int MessageSizeLimit { get; set; }

        /// <summary>
        ///     Gets the reserved for split formatting. This is used for header, footer and counter.
        /// </summary>
        private int ReservedForSplitFormatting
        {
            get { return 100; }
        }

        /// <summary>
        ///     Splits the log event info into multiple fragments, if it's message
        ///     is too big for common target. This is done to aviod truncation in various
        ///     log targets (e.g., DataBase and EventLog).
        /// </summary>
        /// <remarks>
        ///     It is assumed that one character is one Byte. If we start logging
        ///     in unicode, the strategy needs to be revised.
        /// </remarks>
        public List<LogEventInfo> GetEventFragments(LogEventInfo logEventInfo)
        {
            int maxMessageLength = MessageSizeLimit - ReservedForFormatting;

            if (logEventInfo.Message.Length <= maxMessageLength)
            {
                return new List<LogEventInfo> {logEventInfo};
            }

            return Fragment(logEventInfo, maxMessageLength);
        }

        private List<LogEventInfo> Fragment(LogEventInfo logEventInfo, int maxSize)
        {
            maxSize -= ReservedForSplitFormatting;

            var logEvents = new List<LogEventInfo>();
            int fragments = CalculateNumberOfFragments(logEventInfo.Message.Length, maxSize);

            for (int i = 0; i < fragments; i++)
            {
                string messageFragment;
                if (i == fragments - 1)
                {
                    messageFragment = logEventInfo.Message.Substring(i*maxSize);
                }
                else
                {
                    messageFragment = logEventInfo.Message.Substring(i*maxSize, maxSize);
                }
                string fragmentMessage = FormatFragment(messageFragment, i + 1, fragments);
                LogEventInfo fragment = CreateFragment(logEventInfo, fragmentMessage);
                logEvents.Add(fragment);
            }

            return logEvents;
        }

        private int CalculateNumberOfFragments(int length, int size)
        {
            return ((length - 1)/size) + 1;
        }

        private LogEventInfo CreateFragment(LogEventInfo logEvent, string formattedFragment)
        {
            var logEventFragment = new LogEventInfo
                                       {
                                           Level = logEvent.Level,
                                           Message = formattedFragment,
                                           Exception = logEvent.Exception,
                                           TimeStamp = DateTime.UtcNow
                                       };
            if (logEvent.Properties.ContainsKey(EventId))
            {
                logEventFragment.Properties[EventId] = logEvent.Properties[EventId];
            }

            return logEventFragment;
        }

        private string FormatFragment(string body, int fragmentIndex, int lastFagment)
        {
            string header = CreateFragmentHeader(fragmentIndex, lastFagment);
            string footer = CreateFragmentFooter(fragmentIndex, lastFagment);
            return string.Format("{0}\r\n\r\n{1}\r\n\r\n{2}", header, body, footer);
        }

        private string CreateFragmentHeader(int index, int max)
        {
            return string.Format("PART {0}/{1} OF SPLIT MESSAGE>", index, max);
        }

        private string CreateFragmentFooter(int index, int max)
        {
            if (index == max)
            {
                return "<END OF SPLIT MESSAGE";
            }
            else
            {
                return "SPLIT MESSAGE CONTINUES IN NEXT MESSAGE>";
            }
        }
    }
}