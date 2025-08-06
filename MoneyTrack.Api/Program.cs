using MoneyTrack.Api;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

Log.Information("Moneytrack API starting");

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(
    (context, services, configuration) =>
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console());

var app = builder.ConfigurationService().ConfigurationPipeline();
app.UseSerilogRequestLogging();
await app.ResetDatabaseAsync();

app.Run();