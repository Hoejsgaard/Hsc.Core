using System;
using System.Reflection;
using System.Threading;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace Hsc.Foundation.Log.Nlog
{
    /// <summary>
    ///     Mock NLog configuration repository.
    /// </summary>
    public class MockNlogConfigurationRepository : INLogConfigurationRepository
    {
        private static Timer _timer;

        public MockNlogConfigurationRepository()
        {
            // Check every minute. This should probably be done with a more 
            // generic mechanism. e.g. TableUpdates class or the like.
            _timer = new Timer(CheckForUpdates, null, 60000, 60000);
            Application = Assembly.GetEntryAssembly().GetName().Name;
        }

        private string Application { get; set; }
        public event NLogConfigurationChangedHandler OnConfigurationChanged;

        /// <summary>
        ///     Mock implementation of get.
        /// </summary>
        /// <remarks>
        ///     A real implementation should fetch the targets and rules from the
        ///     Hsc database. If the Hsc database is unavailable a
        ///     default fall back file/console/e-mail configuration should be
        ///     returned.
        /// </remarks>
        /// <returns> A configuration with a file and event target and matching rules. </returns>
        public LoggingConfiguration GetConfiguration()
        {
            try
            {
                return GetFromDatabaseMock();
            }
            catch
            {
                return GetFallback();
            }
        }

        private void CheckForUpdates(object stateInfo)
        {
            if (IsDbUpdated())
            {
                NLogConfigurationChangedHandler subscribers = OnConfigurationChanged;
                if (subscribers != null)
                {
                    subscribers.Invoke(this, new EventArgs());
                }
            }
        }

        private bool IsDbUpdated()
        {
            // Mocked - do the DB magic.
            return false;
        }

        /// <summary>
        ///     Gets the mocked configuration from database mock.
        /// </summary>
        private LoggingConfiguration GetFromDatabaseMock()
        {
            var loggingConfiguration = new LoggingConfiguration();

            AddFileTarget(loggingConfiguration);
            AddEventLogTarget(loggingConfiguration);
            AddDatabaseTarget(loggingConfiguration);

            return loggingConfiguration;
        }

        private LoggingConfiguration GetFallback()
        {
            var loggingConfiguration = new LoggingConfiguration();
            AddFileTarget(loggingConfiguration);
            return loggingConfiguration;
        }

        private void AddEventLogTarget(LoggingConfiguration loggingConfiguration)
        {
            EventLogTarget eventLogTarget = GetEventLogTarget();
            loggingConfiguration.AddTarget("event", GetEventLogTarget());
            loggingConfiguration.LoggingRules.Add(new LoggingRule("*", NLog.LogLevel.Trace, eventLogTarget));
        }

        private void AddFileTarget(LoggingConfiguration loggingConfiguration)
        {
            FileTarget fileTarget = GetFileTarget();
            loggingConfiguration.AddTarget("file", fileTarget);
            loggingConfiguration.LoggingRules.Add(new LoggingRule("*", NLog.LogLevel.Trace, fileTarget));
        }

        private void AddDatabaseTarget(LoggingConfiguration loggingConfiguration)
        {
            DatabaseTarget databaseTarget = GetDatabaseTarget();
            loggingConfiguration.AddTarget("database", databaseTarget);
            loggingConfiguration.LoggingRules.Add(new LoggingRule("*", NLog.LogLevel.Trace, databaseTarget));
        }

        /// <summary>
        ///     Gets the event log target.
        /// </summary>
        /// <remarks>
        ///     Note that the executing assembly must be granted write access to the specified log.
        ///     This can be done using an installer and the installutil -i command
        /// </remarks>
        /// <returns> </returns>
        private EventLogTarget GetEventLogTarget()
        {
            return new EventLogTarget
                       {
                           Layout = "${date:format=yyyy-MM-dd HH\\:mm\\:ss.fff} ${callsite} ${level} ${message}",
                           Source = Assembly.GetEntryAssembly().GetName().Name,
                           Log = "Application",
                           EventId = "${event-context:item=eventID}"
                       };
        }

        private DatabaseTarget GetDatabaseTarget()
        {
            var target = new DatabaseTarget
                             {
                                 ConnectionString = "Server=localhost; Database=Hsc_DB; User Id=logDemoUser; Password=Password42",
                                 CommandText = "INSERT INTO [Hsc_DB].[dbo].[LogEntries] " +
                                               "(DateTime, Server, Application, LogLevel, Callsite, Message, EventId, Exception)" +
                                               "VALUES(GETDATE(), @server, '" + Application + "', @logLevel, @callsite, @message, @eventId, @exception);",
                             };
            target.Parameters.Add(new DatabaseParameterInfo("@server", new SimpleLayout("${machinename}")));
            target.Parameters.Add(new DatabaseParameterInfo("@logLevel", new SimpleLayout("${level}")));
            target.Parameters.Add(new DatabaseParameterInfo("@callsite", new SimpleLayout("${callsite}")));
            target.Parameters.Add(new DatabaseParameterInfo("@message", new SimpleLayout("${message}")));
            target.Parameters.Add(new DatabaseParameterInfo("@eventId", new SimpleLayout("${event-context:item=eventID}")));
            target.Parameters.Add(new DatabaseParameterInfo("@exception", new SimpleLayout("${exception}")));

            return target;
        }

        private FileTarget GetFileTarget()
        {
            return new FileTarget
                       {
                           FileName = new SimpleLayout("c:\\temp\\" + Application + ".${shortdate}.log"),
                           ArchiveFileName = new SimpleLayout("c:\\temp\\Archive\\log." + Application + "{##}"),
                           ArchiveNumbering = ArchiveNumberingMode.Rolling,
                           ArchiveEvery = FileArchivePeriod.Day,
                           MaxArchiveFiles = 60,
                           Layout = "${date:format=u} ${callsite} ${level}\r\n ${message}\r\n" +
                                    "----------------------------------------------------------------------------------",
                           KeepFileOpen = true
                       };
        }
    }
}