using System.Data;
using Dapper;
using Jint;

namespace JSqlEngine.Core;

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

            // 필요하면 주석 해제.
            //var path = Directory.GetCurrentDirectory();
            //options.EnableModules(path);
            //options.DebugMode(true);
        });        
    }
    
    private string Sql(string name, object o)
    {
        var jsql = _jSqlReader.GetJSql(name);
        var v = _engine
            .Execute(JSqlTemplate.JSQL_TEMPLATE
                .Replace(JSqlTemplate.SQL_CODE, jsql))
            .Invoke("jsql", o);

        return v.AsString();
    }

    private string CountSql(string name, object o)
    {
        var jsql = _jSqlReader.GetJSql(name);
        var c = JSqlTemplate.JSQL_COUNT_TEMPLATE
            .Replace(JSqlTemplate.SQL_CODE, jsql);
        var v = _engine
            .Execute(c)
            .Invoke("jsql", o);

        return v.AsString();
    }

    private string PagingSql(string name, object o)
    {
        var jsql = _jSqlReader.GetJSql(name);
        var c = JSqlTemplate.JSQL_PAING_TEMPLATE
            .Replace(JSqlTemplate.SQL_CODE, jsql);
        var v = _engine
            .Execute(c)
            .Invoke("jsql", o);

        return v.AsString();
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string name, object o)
    {
        var sql = this.Sql(name, o);
        var result = await connection.QueryAsync<T>(sql, o);
        return result;
    }

    public async Task<T> QueryFirstOrDefaultAsync<T>(IDbConnection connection, string name, object o)
    {
        var sql = this.Sql(name, o);
        var result = await connection.QueryFirstOrDefaultAsync<T>(sql, o);
        return result;
    }

    public async Task<int> ExecuteAsync(IDbConnection connection, string name, object o)
    {
        var sql = this.Sql(name, o);
        int result = 0;
        try
        {
            result = await connection.ExecuteAsync(sql, o);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return result;
    }

    public async Task<IEnumerable<T>> QueryPagingAsync<T>(IDbConnection connection, string name, object o)
    {
        var sql = this.PagingSql(name, o);
        return await connection.QueryAsync<T>(sql, o);
    }
    
    public async Task<int> QueryCountAsync(IDbConnection connection, string name, object o)
    {
        var sql = this.CountSql(name, o);
        return await connection.ExecuteScalarAsync<int>(sql, o);
    }
}