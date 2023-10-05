using System.Data;
using JSqlEngine;
using JSqlEngine.Core;
using JSqlEngine.Data;
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
                services.AddScoped<GetWeatherService>();
                services.AddScoped<GetWeatherListService>();
                services.AddScoped<GetWeatherPagingService>();
                services.AddScoped<SetWeatherInsertService>();
                
                services.AddTransient<SqlConnection>(sp =>
                {
                    var con = sp.GetService<IConfiguration>();
                    var c = con.GetConnectionString("MSSQL");
                    var sqlConnection = new SqlConnection(c);
                    if(sqlConnection.State != ConnectionState.Open) sqlConnection.Open();
                    return sqlConnection;
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