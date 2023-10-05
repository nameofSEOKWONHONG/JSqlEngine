using JSqlConsole.Results;
using JSqlEngine;
using Microsoft.Data.SqlClient;

namespace JSqlConsole;

public class GetWeatherPagingService
{
    private readonly SqlConnection _connection;
    private readonly JSql _jSql;
    public GetWeatherPagingService(SqlConnection connection, JSql jSql)
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
            PAGE_NUMBER = 1,
            PAGE_SIZE = 10
        };
        var count = await _connection.jQueryCountAsync(_jSql, "GET_WEATHER_LIST", getWeatherListParam);
        return await _connection.jQueryPagingAsync<WeatherForecast>(_jSql, "GET_WEATHER_LIST", getWeatherListParam);
    }
}