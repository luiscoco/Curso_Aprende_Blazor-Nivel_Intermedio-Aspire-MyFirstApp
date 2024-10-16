var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.WebAPIWeatherForecast>("webapiweatherforecast");

builder.AddProject<Projects.BlazorWebApp>("blazorwebapp");

builder.Build().Run();
