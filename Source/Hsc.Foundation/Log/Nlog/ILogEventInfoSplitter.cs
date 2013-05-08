using System.Collections.Generic;
using NLog;

namespace Hsc.Foundation.Log.Nlog
{
    public interface ILogEventInfoSplitter
    {
        List<LogEventInfo> GetEventFragments(LogEventInfo logEventInfo);
    }
}