namespace Hsc.Foundation.Log
{
    /// <summary>
    ///     Null logger for use in unit tests.
    /// </summary>
    public class NullLogger : ILogger
    {
        public void Write(LogEntry entry)
        {
            return;
        }

        public void Flush()
        {
            return;
        }
    }
}