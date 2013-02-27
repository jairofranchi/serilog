﻿using System;
using Serilog.Core;

namespace Serilog.Tests.Support
{
    class DelegatingSink : ILogEventSink
    {
        readonly Action<LogEvent> _write;

        public DelegatingSink(Action<LogEvent> write)
        {
            if (write == null) throw new ArgumentNullException("write");
            _write = write;
        }

        public void Write(LogEvent logEvent)
        {
            _write(logEvent);
        }
    }
}
