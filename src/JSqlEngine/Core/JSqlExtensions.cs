using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JSqlEngine;

public static class JSqlExtensions
{
    public static IServiceCollection AddJSql(this IServiceCollection services)
    {
        services.AddSingleton<JSqlReader>()
            .AddSingleton<JSqlTimer>()
            .AddScoped<JSql>();
        
        return services;
    }

    public static IHost UseJSql(this IHost host)
    {
        var engine = host.Services.GetService<JSqlReader>();
        engine.Initialize();
        
        var timer = host.Services.GetService<JSqlTimer>();
        timer.Initialize();
        return host;
    }
}