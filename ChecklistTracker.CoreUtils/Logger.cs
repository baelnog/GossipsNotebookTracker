using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace ChecklistTracker.CoreUtils
{
    /// <summary>
    /// https://stackoverflow.com/questions/1181561/how-to-effectively-log-asynchronously
    /// </summary>
    public class ThreadedLogger
    {

        ConcurrentQueue<LogMessage> queue = new ConcurrentQueue<LogMessage>();
        AutoResetEvent hasNewItems = new AutoResetEvent(false);

        public ThreadedLogger() : base()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (DequeueBatch(100, out var outQueue))
                    {
                        foreach (var log in outQueue)
                        {
                            if (log.Args != null)
                            {
                                //Debug.WriteLine(log.log, log.e);
                                Debug.WriteLine(log.Log, log.Args);
                            }
                            else if (log.Exception != null)
                            {
                                Debug.WriteLine(log.Log, log.Exception);
                            }
                            else
                            {
                                Debug.WriteLine(log.Log);
                            }
                        }
                        Thread.Sleep(50);
                    }
                    else
                    {
                        hasNewItems.WaitOne();
                    }
                }
            });
        }

        private bool DequeueBatch(int takeN, out Queue<LogMessage> outQueue)
        {
            outQueue = new Queue<LogMessage>();

            var any = false;
            for (int i = 0; i < takeN && queue.TryDequeue(out var result); i++)
            {
                any = true;
                outQueue.Enqueue(result);
            }
            return any;
        }

        [Conditional("DEBUG")]
        public void LogMessage(string log)
        {
            queue.Enqueue(new LogMessage(log, Logging.Indent));
            hasNewItems.Set();
        }

        [Conditional("DEBUG")]
        public void LogMessage(string log, Exception e)
        {
            queue.Enqueue(new LogMessage(log, Logging.Indent, e: e));
            hasNewItems.Set();
        }

        [Conditional("DEBUG")]
        public void LogMessage([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string log, params object[]? args)
        {
            queue.Enqueue(new LogMessage(log, Logging.Indent, args: args));
            hasNewItems.Set();
        }
    }
}
