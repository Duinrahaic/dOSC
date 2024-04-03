using Serilog;

namespace dOSCEngine.Utilities;

public static class LoggingProvider
{
    public static ILogger Logger { get; } = new LoggerConfiguration()
        .WriteTo.Async(writeTo => writeTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss}[{Level}] {Message}{NewLine}{Exception}"))
        .WriteTo.Async(writeTo => writeTo.File(
            Path.Combine(dOSCFileSystem.LogFolder, "dOSC"),
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss}[{Level}] {Message}{NewLine}{Exception}",
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 7,
            rollOnFileSizeLimit: true,
            fileSizeLimitBytes: 200000000,
            buffered: true
        ))
        .Enrich.FromLogContext()
        .MinimumLevel.Debug() // Minimum level is Warning
        .CreateLogger();

}