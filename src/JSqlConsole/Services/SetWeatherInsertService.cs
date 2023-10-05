using JSqlEngine;
using JSqlEngine.Core;
using Microsoft.Data.SqlClient;

namespace JSqlConsole;

public class SetWeatherInsertService
{
    private readonly SqlConnection _connection;
    private readonly JSql _jSql;
    
    private readonly List<string> _cities = new List<string>()
    {
        "seoul", "incheon", "gangwon", "gyonggi", "busan"
    };
    
    public SetWeatherInsertService(SqlConnection connection, JSql jSql)
    {
        _connection = connection;
        _jSql = jSql;
    }
    
    public async Task<int> ExecuteAsync(int i)
    {
        var insObj = new
        {
            TENANTID = "00000",
            ID = i,
            CITY = _cities[Random.Shared.Next(0, 4)],
            TEMPERATUREC = Random.Shared.Next(1, 100),
            DATE = DateTime.Now.AddDays(Random.Shared.Next(-30, 30)),
            SUMMARY = "test",
            CREATEDBY = "system",
            CREATEDON = DateTime.Now,
        };

        var result = 0;
        try
        {
            result = await _connection.jQueryExecuteAsync(_jSql, "INSERT_WEATHER", insObj);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return result;
    }
}