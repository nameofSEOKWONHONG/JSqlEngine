using Dapper;
using JSqlConsole;
using JSqlConsole.Results;
using JSqlEngine;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddCommandLine(args)
#if !DEBUG
            .AddJsonFile("appsettings.json")
#else
    .AddJsonFile("appsettings.development.json")
#endif
    .Build();

var host = AppHostBuilder.CreateHostBuilder(args, configuration)
    .Build()
    .UseJSql();

using var scope = host.Services.CreateScope();
var con = scope.ServiceProvider.GetService<SqlConnection>();
var jsql = scope.ServiceProvider.GetService<JSql>();
var getWeatherParam = new
{
    TENANTID = "00000",
    Id = 2 
};
var sql = jsql.Sql("GET_WEATHER", getWeatherParam);
var exist = await con.QueryFirstOrDefaultAsync<WeatherForecast>(sql, getWeatherParam);
if (exist != null)
{
    Console.WriteLine(exist.Id);
}
    
var getWeatherListParam = new
{
    TENANTID = "00000",
    CITY = "Seoul",
};
sql = jsql.Sql("GET_WEATHER_LIST", getWeatherListParam);
var list = await con.QueryAsync<WeatherForecast>(sql, getWeatherListParam);
if (list != null)
{
    foreach (var item in list)
    {
        Console.WriteLine(item.Id);    
    }
}