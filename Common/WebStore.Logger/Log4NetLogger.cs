﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;
using log4net;
using Microsoft.Extensions.Logging;

namespace WebStore.Logger
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _Log;

        public Log4NetLogger(string Name, XmlElement xml)
        {
            var logger_repository = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(), 
                typeof(log4net.Repository.Hierarchy.Hierarchy));
            _Log = LogManager.GetLogger(logger_repository.Name, Name);
            log4net.Config.XmlConfigurator.Configure(logger_repository, xml);
        }

        public bool IsEnabled(LogLevel Level)
        {
            switch (Level)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return _Log.IsDebugEnabled;
                case LogLevel.Information:
                    return _Log.IsInfoEnabled;
                case LogLevel.Warning:
                    return _Log.IsWarnEnabled;
                case LogLevel.Error:
                    return _Log.IsErrorEnabled;
                case LogLevel.Critical:
                    return _Log.IsFatalEnabled;
                case LogLevel.None:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Level), Level, null);
            }
        }

        public void Log<TState>(LogLevel Level, EventId id, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
           if(!IsEnabled(Level)) return;
           if(formatter is null) throw new ArgumentNullException(nameof(formatter));

           var msg = formatter(state, exception);
           if (string.IsNullOrEmpty(msg) && exception is null) return;

           switch (Level)
           {
               case LogLevel.Trace:
               case LogLevel.Debug:
                   _Log.Debug(msg);
                    break;
               case LogLevel.Information:
                   _Log.Info(msg);
                    break;
               case LogLevel.Warning:
                   _Log.Warn(msg);
                   break;
               case LogLevel.Error:
                   _Log.Error(msg);
                   break;
               case LogLevel.Critical:
                   _Log.Fatal(msg);
                   break;
               case LogLevel.None:
                   break;
                default:
                   throw new ArgumentOutOfRangeException(nameof(Level), Level, null);
           }
        }

        public IDisposable BeginScope<TState>(TState state) => null;

    }
}
