using System.IO;
using Serilog;
using Serilog.Filters;
using ILogger = Serilog.ILogger;

namespace dOSC.Utilities;

public static class LoggerProvider
{
    public static ILogger Logger => Configuration.CreateLogger();

    public static LoggerConfiguration Configuration => new LoggerConfiguration()
        .WriteTo.Async(writeTo => writeTo.File(
            Path.Combine(AppFileSystem.LogFolder, "Wiresheet"),
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss}[{Level}][Origin] {Message}{NewLine}{Exception}",
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 7,
            rollOnFileSizeLimit: true,
            fileSizeLimitBytes: 200000000,
            buffered: true
        ))
        // .Filter.ByExcluding(Matching.FromSource("Microsoft"))
        // .Filter.ByExcluding(Matching.FromSource("System"))
        .Enrich.FromLogContext()
        .MinimumLevel.Debug(); // Minimum level is Warning

    public static ILogger SetupLogger(string origin, LogSink sink)
    {
        return Configuration
            .Enrich.WithProperty("Origin", origin)
            .WriteTo.Async(writeTo => writeTo.Sink(sink))
            .CreateLogger();
    }
}