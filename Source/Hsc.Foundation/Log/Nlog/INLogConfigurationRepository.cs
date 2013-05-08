using NLog.Config;

namespace Hsc.Foundation.Log.Nlog
{
    public interface INLogConfigurationRepository
    {
        event NLogConfigurationChangedHandler OnConfigurationChanged;

        /// <summary>
        ///     Gets Hsc NLog configuration.<br />
        ///     <br />
        ///     The configuration defines targets (e.g., event log, file, e-mail etc.) and their
        ///     respecive formatting, and rules (e.g., which logs are logged to which target).
        /// </summary>
        LoggingConfiguration GetConfiguration();
    }
}