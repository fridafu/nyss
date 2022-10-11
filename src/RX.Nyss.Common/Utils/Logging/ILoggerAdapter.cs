using System;

namespace RX.Nyss.Common.Utils.Logging;

public interface ILoggerAdapter
{
    void Debug(object obj);

    void Info(object obj);

    void Warn(object obj);

    void WarnFormat(string format, params object[] args);

    void Error(object obj);

    void Error(Exception exception, string message);

    void ErrorFormat(string format, params object[] args);
}