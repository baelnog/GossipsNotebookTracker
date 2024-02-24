using Microsoft.Extensions.Logging;
using NReco.Logging.File;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ChecklistTracker.CoreUtils
{
    public class Logging
    {
        internal static int Indent = 0;

        static Logging()
        {
            ILoggerFactory factory = LoggerFactory.Create(builder =>
            {
                //builder.AddDebug();
                builder.AddFile("app-log.txt", options =>
                {
                    options.Append = false;
                    options.MinLevel = LogLevel.Debug;
                    options.FileSizeLimitBytes = 1024 * 1024;
                    options.UseUtcTimestamp = true;
                    options.MaxRollingFiles = 1;
                    options.FormatLogEntry = message =>
                    {
                        var builder = new StringBuilder();
                        builder.Append(DateTime.Now.ToString("yyyyMMdd-HH:mm:ss:ff"));
                        for (int i = 0; i <= Indent; i++)
                        {
                            builder.Append("  ");
                        }
                        builder.Append(message.Message);

                        if (message.Exception != null)
                        {
                            builder.Append("  ");
                            builder.Append(message.Exception.ToString());
                        }
                        return builder.ToString();
                    };
                });
            });
            logger = factory.CreateLogger("Program");
        }

        private static readonly ILogger logger;
        //private static readonly ThreadedLogger Logger = new ThreadedLogger();

        public static void WriteLine(string line)
        {
            logger.LogInformation(line);
            //Logger.LogMessage(line);
        }

        public static void WriteLine(string line, params object[]? args)
        {
            logger.LogInformation(line, args);
            //Logger.LogMessage(line, args);
        }

        public static void WriteLine(string line, Exception e)
        {
            logger.LogInformation(line, e);
            //Logger.LogMessage(line, e);
        }

        public static IDisposable Indented()
        {
            Indent++;
            return new OnDispose(() => Indent--);
        }

    }
}
