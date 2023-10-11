using System.Data;
using JSqlEngine.Core;

namespace JSqlEngine;

public static class JSqlExecuteExtensions
{
    public static async Task<int> jQueryCountAsync(this IDbConnection connection, JSql sql, string name, object obj
        , CommandType commandType = CommandType.Text)
    {
        return await sql.QueryCountAsync(connection, name, obj);
    }
    public static async Task<IEnumerable<T>> jQueryPagingAsync<T>(this IDbConnection connection, JSql sql, string name, object obj
        , CommandType commandType = CommandType.Text)
    {
        return await sql.QueryPagingAsync<T>(connection, name, obj);
    }
    
    public static async Task<IEnumerable<T>> jQueryAsync<T>(this IDbConnection connection, JSql sql, string name, object obj
        , CommandType commandType = CommandType.Text)
    {
        return await sql.QueryAsync<T>(connection, name, obj);
    }

    public static async Task<T> jQueryFirstAsync<T>(this IDbConnection connection, JSql sql, string name, object obj
        , CommandType commandType = CommandType.Text)
    {
        return await sql.QueryFirstOrDefaultAsync<T>(connection, name, obj);
    }

    public static async Task<int> jExecuteAsync(this IDbConnection connection, JSql sql, string name, object obj
        , CommandType commandType = CommandType.Text)
    {
        return await sql.ExecuteAsync(connection, name, obj);
    }
}