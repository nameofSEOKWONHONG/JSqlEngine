using JSqlEngine;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JSqlConsole;

public class AppHostBuilder
{
    public static IHostBuilder CreateHostBuilder(string[] args, IConfigurationRoot configuration) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(builder =>
            {
                builder.Sources.Clear();
                builder.AddConfiguration(configuration);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddTransient<SqlConnection>(sp =>
                {
                    var con = sp.GetService<IConfiguration>();
                    var c = con.GetConnectionString("MSSQL");
                    return new SqlConnection(c);
                });
            
                services.Configure<JSqlOption>(hostContext.Configuration.GetSection("JSqlOption"));
                services.AddJSql();
            })
            .ConfigureLogging(logging =>
            {
                // 로깅 설정
                logging.ClearProviders();
                logging.AddConsole();
            })
            .UseConsoleLifetime(); // 콘솔 라이프타임 사용
}