using System;
using System.Reflection;
using System.Text;
using System.Threading;
using Hsc.Foundation.Log;

namespace Hsc.LogDemo
{
    public class LogDemo : ILogDemo
    {
        private readonly ILogBuilderFactory _logFactory;
        private readonly ILogger _logger;

        public LogDemo(ILogBuilderFactory logFactory, ILogger logger)
        {
            _logFactory = logFactory;
            _logger = logger;
        }

        #region ILogDemo Members

        public void Run()
        {
            // Rune do not don't like the outcommented approach. The overloads are hideous and error prone. 
            // They force you to pass null, which violates "don't pass null" rule ;)
            //
            // _log.Info("messge {0}", "someFormatValue", someException, someId, cause, resolution).
            // _log.Warn("messge {0}", "someFormatValue", someException, someId, cause, resolution).
            // etc.

            Console.WriteLine("Running ILogBuilderFactory demo. My personal preference.");
            RunILogBuilderFactoryDemo();

            Console.WriteLine("Running ILogger demo.");
            RunILoggerDemo();

            _logger.Flush();

            Console.WriteLine();
            Console.WriteLine(@"Log demos has been exectued. Check log file in C:\temp\ and application event log for results.");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        /// <summary>
        ///     Runs the demo of the ILogBuilderFacotry interface.
        /// </summary>
        /// <remarks>
        ///     This is more compact than ILogger. Main drawback is that you have to end the train with .Write().
        /// </remarks>
        private void RunILogBuilderFactoryDemo()
        {
            _logFactory.CreateWithMessage("My message").AsInfo().Write();

            _logFactory.CreateWithMessage("My formatted message, running from: {0}",
                                          Assembly.GetExecutingAssembly().GetName().Name)
                       .AsInfo()
                       .Write();

            _logFactory.CreateWithMessage("An error message").AsError().Write();

            _logFactory.CreateWithMessage("An error message with exception")
                       .AsError()
                       .WithException(new AbandonedMutexException("message"))
                       .WithCause("Demonstrating ILogBuilderFactory")
                       .WithResolution("Stop running the demo.")
                       .WithEventId(42)
                       .Write();

            _logFactory.CreateWithMessage(GetRandomString(28000))
                       .AsError()
                       .WithException(new IndexOutOfRangeException("My funny exception"))
                       .WithEventId(47)
                       .WithCause("Split me, please")
                       .WithResolution("Try to log smaller messges?")
                       .Write();
        }

        /// <summary>
        ///     Runs the demo of the ILogger interface.
        /// </summary>
        /// <remarks>
        ///     This interface is verbose and flexible.
        /// </remarks>
        private void RunILoggerDemo()
        {
            _logger.Write(new LogEntry {Message = "Another way to write a message"});

            _logger.Write(new LogEntry
                              {
                                  Message =
                                      string.Format("Anoter formatted message, running from: {0}",
                                                    Assembly.GetExecutingAssembly().GetName().Name)
                              });

            _logger.Write(new LogEntry
                              {
                                  Message = "Another error message",
                                  Level = LogLevel.Error
                              });

            _logger.Write(new LogEntry
                              {
                                  Message = "Another error message",
                                  Level = LogLevel.Error,
                                  Cause = "Demonstrating ILogger",
                                  Exception = new AbandonedMutexException("foo exception"),
                                  Resolution = "Stop running the demo.",
                                  EventId = 43
                              });
        }

        private string GetRandomString(int size)
        {
            var builder = new StringBuilder();
            var random = new Random(315341354);
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26*random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        #endregion
    }
}