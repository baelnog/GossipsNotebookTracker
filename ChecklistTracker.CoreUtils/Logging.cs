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

        private static readonly ThreadedLogger Logger = new ThreadedLogger();

        public static void WriteLine(string line)
        {
            Logger.LogMessage(line);
        }

        public static void WriteLine(string line, params object[]? args)
        {
            Logger.LogMessage(line, args);
        }

        public static void WriteLine(string line, Exception e)
        {
            Logger.LogMessage(line, e);
        }

        public static IDisposable Indented()
        {
            Logger.Indent++;
            return new OnDispose(() => Logger.Indent--);
        }

    }
}
