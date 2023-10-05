using JSqlConsole.Results;
using JSqlEngine;
using JSqlEngine.Core;
using Microsoft.Data.SqlClient;

namespace JSqlConsole;

public class GetWeatherService
{
    private readonly SqlConnection _connection;
    private readonly JSql _jSql;
    public GetWeatherService(SqlConnection connection, JSql jSql)
    {
        _connection = connection;
        _jSql = jSql;
    }

    public async Task<WeatherForecast> ExecuteAsync()
    {
        var getWeatherParam = new
        {
            TENANTID = "00000",
            Id = 2 
        };
        return await _connection.jQueryFirstAsync<WeatherForecast>(_jSql, "GET_WEATHER", getWeatherParam);
    }
}