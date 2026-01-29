using IdentityServerAspNetIdentity;
using IdentityServerAspNetIdentity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(ctx.Configuration));
    builder.Services.AddTransient<IEmailSender, NoOpEmailSender>();


    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();

    await SeedData.EnsureSeedData(app);

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
 
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}


