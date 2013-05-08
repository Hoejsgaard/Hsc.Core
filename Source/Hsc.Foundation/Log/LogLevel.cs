namespace Hsc.Foundation.Log
{
    /// <summary>
    ///     Describe the severity of a log message.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        ///     Trace level. Very fine grained logging. Can be used for line by line logging.
        /// </summary>
        /// <remarks>This is typically disabled in production environments.</remarks>
        Trace,

        /// <summary>
        ///     Debugging level. More coarse than <see cref="Trace" />.
        /// </summary>
        /// <remarks>This is typically disabled in production environments.</remarks>
        Debug,

        /// <summary>
        ///     Information level.
        /// </summary>
        /// <remarks>This is typically the minimum level for production environments.</remarks>
        Info,

        /// <summary>
        ///     Warning level. Typically used for non-critical issues that the program can recover from.
        /// </summary>
        Warn,

        /// <summary>
        ///     Error level. Used for program errors.
        /// </summary>
        Error,

        /// <summary>
        ///     Fatal level. Very serious issues that usually will result in an unrecoverable state.
        /// </summary>
        Fatal
    }
}