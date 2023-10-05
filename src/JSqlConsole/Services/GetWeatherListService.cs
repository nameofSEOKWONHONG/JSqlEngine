using JSqlConsole.Results;
using JSqlEngine;
using JSqlEngine.Core;
using Microsoft.Data.SqlClient;

namespace JSqlConsole;

public class GetWeatherListService
{
    private readonly SqlConnection _connection;
    private readonly JSql _jSql;
    public GetWeatherListService(SqlConnection connection, JSql jSql)
    {
        _connection = connection;
        _jSql = jSql;
    }
    
    public async Task<IEnumerable<WeatherForecast>> ExecuteAsync()
    {
        var getWeatherListParam = new
        {
            TENANTID = "00000",
            CITY = "Seoul",
        };
        return await _connection.jQueryAsync<WeatherForecast>(_jSql, "GET_WEATHER_LIST", getWeatherListParam);
    }
}