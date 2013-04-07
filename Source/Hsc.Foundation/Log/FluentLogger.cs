namespace Hsc.Foundation.Log
{
    public class FluentLogger : ILogBuilderFactory
    {
        private readonly ILogger _logger;

        public FluentLogger(ILogger logger)
        {
            _logger = logger;
        }

        public LogEntryBuilder CreateWithMessage(string formattedMessage, params object[] formatArgs)
        {
            return Create().WithMessage(formattedMessage, formatArgs);
        }

        public LogEntryBuilder Create()
        {
            return new LogEntryBuilder(_logger);
        }
    }
}