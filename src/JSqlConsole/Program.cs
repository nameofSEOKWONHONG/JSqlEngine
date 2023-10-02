using JSqlEngine;
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

var host = CreateHostBuilder(args, configuration)
    .Build()
    .UseJSql();


using (var scope = host.Services.CreateScope())
{
    var jsql = scope.ServiceProvider.GetService<JSql>();
    var obj = new
    {
        NAME = "TEST",
        AGE = 18 
    };
    var sql = jsql.Execute("demo1.jsql", obj);
    
    if (!string.IsNullOrWhiteSpace(sql))
    {
        Console.WriteLine(sql);
    }
    
    Thread.Sleep(1000 * 10);
    
    sql = jsql.Execute("demo1.jsql", obj);
    
    if (!string.IsNullOrWhiteSpace(sql))
    {
        Console.WriteLine(sql);
    }
}


static IHostBuilder CreateHostBuilder(string[] args, IConfigurationRoot configuration) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(builder =>
        {
            builder.Sources.Clear();
            builder.AddConfiguration(configuration);
        })
        .ConfigureServices((hostContext, services) =>
        {
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