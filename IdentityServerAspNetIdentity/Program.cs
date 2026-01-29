using IdentityServerAspNetIdentity;
using IdentityServerAspNetIdentity.Data;
using Microsoft.AspNetCore.HttpOverrides;
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

    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();
    });


    var app = builder
        .ConfigureServices();


    // await SeedData.EnsureSeedData(app);

    app.UseForwardedHeaders();

    app.Use((context, next) =>
    {
        context.Request.Scheme = "https";
        return next();
    });

    app.ConfigurePipeline();
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


