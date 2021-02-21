using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Loki;
using SerilogWithLokiIntegrator.Options;

namespace SerilogWithLokiIntegrator
{
    public static class Extensions
    {
        public static IWebHostBuilder UseSerilogLoki(this IWebHostBuilder webHostBuilder)
        {
            webHostBuilder.UseSerilog((ctx, cfg) =>
            {
                SerilogWithLokiLoggingOptions options = ctx.Configuration.GetSection("SerilogLoki").Get<SerilogWithLokiLoggingOptions>();
                
                BasicAuthCredentials credentials = new BasicAuthCredentials(options.Address,options.User,options.Password);

                cfg.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Application", ctx.HostingEnvironment.ApplicationName)
                    .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName)
                    .WriteTo.LokiHttp(credentials);

                if(ctx.HostingEnvironment.IsDevelopment())
                    cfg.WriteTo.Console(new RenderedCompactJsonFormatter());
            });
            
            return webHostBuilder;
        }
    }
}