namespace Hsc.Foundation.Log
{
    public interface ILogBuilderFactory
    {
        /// <summary>
        /// Creates the specified formatted message. Rember to end your train with a .Write().
        /// </summary>
        /// <param name="formattedMessage">The formatted message.</param>
        /// <param name="formatArgs">The format args.</param>
        LogEntryBuilder CreateWithMessage(string formattedMessage, params object[] formatArgs);
    }
}