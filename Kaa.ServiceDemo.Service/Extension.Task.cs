using System;
using System.Collections.Generic;
using System.Text;

namespace Kaa.ServiceDemo.Service
{
    public static partial class Extension
    {
        public static Task<T> TaskEx<T>(this Task<T> task, ILogger logger, string logMsg)
        {
            return task.ContinueWith(c =>
            {
                if (!c.IsFaulted)
                    return c.Result;

                if (isLog)
                {
                    c.Exception.InnerExceptions.ToList().ForEach(ex =>
                    {
                        logger.LogError(logMsg ?? string.Empty, ex);
                    });
                }
                return default(T);
            });
        }
    }
}
