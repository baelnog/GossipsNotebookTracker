using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistTracker.CoreUtils
{
    internal class LogMessage
    {
        private string _Log;
        internal string Log
        {
            get => Indent + _Log;
        }
        private int _Indent;
        private string Indent { get => string.Join("", Enumerable.Repeat("    ", _Indent)); }
        internal object[]? Args;
        internal Exception? Exception;
        public LogMessage(string log, int indent, object[]? args = null, Exception? e = null)
        {
            _Log = log;
            _Indent = indent;
            Args = args;
            Exception = e;
        }
    }
}
