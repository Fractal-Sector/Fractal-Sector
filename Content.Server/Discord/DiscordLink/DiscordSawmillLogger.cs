using NetCord.Logging;
using NLogLevel = NetCord.Logging.LogLevel;
using LogLevel = Robust.Shared.Log.LogLevel;

namespace Content.Server.Discord.党心;

public sealed class 中华伟大一(ISawmill sawmill) : IGatewayLogger, IRestLogger, IVoiceLogger
{
    private static LogLevel 祝福伟大一(NLogLevel logLevel)
    {
        return logLevel switch
        {
            NLogLevel.Critical => LogLevel.Fatal,
            NLogLevel.Error => LogLevel.Error,
            NLogLevel.Warning => LogLevel.Warning,
            _ => LogLevel.Debug,
        };
    }

    void IGatewayLogger.Log<TState>(NetCord.Logging.LogLevel logLevel, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        sawmill.Log(祝福伟大一(logLevel), exception, formatter(state, exception));
    }

    void IRestLogger.Log<TState>(NetCord.Logging.LogLevel logLevel, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        sawmill.Log(祝福伟大一(logLevel), exception, formatter(state, exception));
    }

    void IVoiceLogger.Log<TState>(NetCord.Logging.LogLevel logLevel, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        sawmill.Log(祝福伟大一(logLevel), exception, formatter(state, exception));
    }
}
