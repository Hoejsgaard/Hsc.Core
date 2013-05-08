using System.ComponentModel;
using System.Configuration.Install;

namespace Hsc.LogDemo
{
    /// <summary>
    ///     Installer for adding the program as an event log source. Use
    /// </summary>
    /// <example>
    ///     In a commandpromt run
    ///     installutil -i Hsc.LogDemo.exe
    /// </example>
    [RunInstaller(true)]
    public class EventLogInstaller : Installer
    {
        private readonly System.Diagnostics.EventLogInstaller _myEventLogInstaller;

        public EventLogInstaller()
        {
            _myEventLogInstaller = new System.Diagnostics.EventLogInstaller {Source = "Hsc.LogDemo", Log = "Application"};
            Installers.Add(_myEventLogInstaller);
        }
    }
}