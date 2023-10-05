using System.Data;

namespace JSqlEngine;

public static class JSqlExecuteExtensions
{
    public static async Task<int> jQueryCountAsync(this IDbConnection connection, JSql sql, string name, object obj)
    {
        return await sql.QueryCountAsync(connection, name, obj);
    }
    public static async Task<IEnumerable<T>> jQueryPagingAsync<T>(this IDbConnection connection, JSql sql, string name, object obj)
    {
        return await sql.QueryPagingAsync<T>(connection, name, obj);
    }
    
    public static async Task<IEnumerable<T>> jQueryAsync<T>(this IDbConnection connection, JSql sql, string name, object obj)
    {
        return await sql.QueryAsync<T>(connection, name, obj);
    }

    public static async Task<T> jQueryFirstAsync<T>(this IDbConnection connection, JSql sql, string name, object obj)
    {
        return await sql.QueryFirstOrDefaultAsync<T>(connection, name, obj);
    }

    public static async Task<int> jQueryExecuteAsync(this IDbConnection connection, JSql sql, string name, object obj)
    {
        return await sql.ExecuteAsync(connection, name, obj);
    }
}