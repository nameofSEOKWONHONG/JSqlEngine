﻿namespace JSqlConsole.Results;

public class TenantBase
{
    public string TenantId { get; set; }
}

public class IdentityBase : TenantBase
{
    public int Id { get; set; }
}

public class WeatherForecast : IdentityBase
{
    public string City { get; set; }
    
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}