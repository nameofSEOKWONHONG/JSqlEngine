using System.Diagnostics;
using System.Text.Json;
using JSqlConsole;
using JSqlEngine;
using JSqlEngine.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

var sw = new Stopwatch();
sw.Start();
using var scope = host.Services.CreateScope();
var getWeatherService = scope.ServiceProvider.GetService<GetWeatherService>();
var weather = await getWeatherService.ExecuteAsync();
if (weather == null)
{
    async void insert(int i1) => await scope.ServiceProvider.GetService<SetWeatherInsertService>().ExecuteAsync(i1);
    Enumerable.Range(1, 100).ToList().ForEach(insert);
}

var getWeatherPagingService = scope.ServiceProvider.GetService<GetWeatherPagingService>();
var result = await getWeatherPagingService.ExecuteAsync();

var getWeatherListService = scope.ServiceProvider.GetService<GetWeatherListService>();
var list = await getWeatherListService.ExecuteAsync();
if (list != null)
{
    foreach (var item in list)
    {
        Console.WriteLine(JsonSerializer.Serialize(item, new JsonSerializerOptions()
        {
            WriteIndented = true
        }));
    }
}
sw.Stop();
Console.WriteLine(sw.Elapsed.TotalSeconds);