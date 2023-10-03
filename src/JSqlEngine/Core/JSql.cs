using Jint;

namespace JSqlEngine;

public sealed class JSql
{
    private readonly JSqlReader _jSqlReader;
    private readonly Engine _engine;
    
    public JSql(JSqlReader jSqlReader, CancellationToken cancellationToken = new ())
    {
        _jSqlReader = jSqlReader;
        
        _engine = new Engine(options =>
        {
            // Limit memory allocations to MB
            options.LimitMemory(4_000_000);
            
            // Set a timeout to 4 seconds.
            // options.TimeoutInterval(TimeSpan.FromSeconds(4));

            // Set limit of 1000 executed statements.
            options.MaxStatements(1000);
            
            // Use a cancellation token.
            options.CancellationToken(cancellationToken);

            //var path = Directory.GetCurrentDirectory();
            //options.EnableModules(path);
            //options.DebugMode(true);
        });        
    }

    public string Sql(string name, object obj)
    {
        var jsql = _jSqlReader.GetJSql(name);
        var v = _engine
            .Execute(jsql)
            .Invoke("jsql", obj);

        return v.AsString();
    }
}