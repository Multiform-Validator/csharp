using Microsoft.Extensions.Logging;

namespace MultiformValidator;

public static class LoggerSetup
{
    public static ILogger? Logger { get; private set; }
    public static void ConfigureLogger(ILogger logger)
    {
        Logger = logger;
    }
}