namespace Hsc.Foundation.Log
{
    public interface ILogger
    {
        void Write(LogEntry entry);

        void Flush();
    }
}