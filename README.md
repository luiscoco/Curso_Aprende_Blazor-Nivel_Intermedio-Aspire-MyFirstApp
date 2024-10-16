# How convert a Blazor Web App and a .NET Web API into a .NET Aspire project

For more information to learn .NET Aspire visit this URL: 

https://learn.microsoft.com/en-us/training/paths/dotnet-aspire/

## 1. Create a Blazor Web App

We run Visual Studio 2022 Community Edition and create a new project

![image](https://github.com/user-attachments/assets/ade8b6f0-ba7a-4fc5-b865-80e1af1eb750)

We select the Blazor Web App project template

![image](https://github.com/user-attachments/assets/4132c37f-a79f-4fc3-bce9-370266d06ef4)

We input the project name and location

![image](https://github.com/user-attachments/assets/2f71aa66-e300-46f9-b2d3-564c388f78f8)

We select the .NET 9 framework

![image](https://github.com/user-attachments/assets/00d33db5-d835-4a79-a93c-597276d13089)

We verify the project folders structure

![image](https://github.com/user-attachments/assets/11b68cd4-decd-49db-b12a-6f8c1ee2b29a)

## 2. Add a new project with the template .NET Core Web API

We right click on the Solution and we select the menu option add new project

![image](https://github.com/user-attachments/assets/c05ca930-dc51-43d9-94d4-0616c6fc2db5)

We search for the Web API project templates and we select the **ASP.NET Core Web API**

![image](https://github.com/user-attachments/assets/f530a1ec-f2d0-40b6-9f09-041154ffe07f)

We input the project name, we select the project folder and press the next button

![image](https://github.com/user-attachments/assets/213b2de9-f70a-4534-9055-3a4bc6f34e28)

We select the .NET 9 framework and we leave the other fields with the default values

![image](https://github.com/user-attachments/assets/5e5122b5-3977-417c-aa0b-14b36170854b)

We verify in the Solution Explorer both projects inside my solution

![image](https://github.com/user-attachments/assets/0dbc2495-cca9-49dd-8489-781c5b8d0d3d)

## 3. Move the BlazorWebApp project into a new folder

We create a new folder **BlazorWebApp**

![image](https://github.com/user-attachments/assets/cd1dbfab-d63c-4431-a6aa-49fdc4a4575b)

We move the Blazor Web Application code inside to that folder

![image](https://github.com/user-attachments/assets/a0b07848-c490-42a2-a472-829831ded4f0)

We edit  the solution file to include the new Blazor Web App location

![image](https://github.com/user-attachments/assets/1ee15a5f-6b84-4f00-98a7-8195ae7c8d5c)

We save the modified solution file

![image](https://github.com/user-attachments/assets/1ac51282-c849-434e-92f2-fdad5ef0c1ec)

We open the solution in Visual Studio 2022

![image](https://github.com/user-attachments/assets/12134b70-7f1a-47f9-9190-8f422e9681a5)

## 4. Run both projects to verify them

First we right click on **BlazorWebApp.csproj** and select the menu option **Set as StartUp Project**

![image](https://github.com/user-attachments/assets/12d08a70-c7f9-4306-b1f3-be73dba1591b)

We run the project and see the result:

![image](https://github.com/user-attachments/assets/a09664d1-53ca-412b-8f44-0a9978c7eb8f)

Now we right click on **WebAPIWeatherForecast** and select the menu option **Set as StartUp Project**. See the result:

![image](https://github.com/user-attachments/assets/c0e61242-a93d-4d50-9cf2-ab89002ea4c0)

We run the project and see the result:

![image](https://github.com/user-attachments/assets/746ddf30-b7e4-4718-8757-b8a9078b4fd8)

## 5. Modify the Blazor Web App to consume the .NET Core Web API

We first create a Service and a Data model for consuming the API

We create a **Service** and a **Data** folders

![image](https://github.com/user-attachments/assets/10bde17c-4142-48b4-9d5a-426af6e89bd7)

We create the **Data model** in the file **WeatherForecast.cs**

![image](https://github.com/user-attachments/assets/e26b9198-8d0e-4b45-99fc-b0a3ad8606f0)

```csharp
namespace BlazorWebApp.Data
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}
```

We also define the **Service** in the file **WeatherForecastService.cs**

![image](https://github.com/user-attachments/assets/86abfed3-c516-4b89-83df-bd6e6e001435)

```csharp
using BlazorWebApp.Data;

namespace BlazorWebApp.Service
{
    public class WeatherApiClient
    {
        private readonly HttpClient _httpClient;

        public WeatherApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherForecast[]> GetWeatherAsync()
        {
            return await _httpClient.GetFromJsonAsync<WeatherForecast[]>("/weatherforecast");
        }
    }
}
```

Now we are going to modify the **Weather.razor** component (defined inside the **BlazorWebApp.csproj**) to consume the API (defined inside the WebAPIWeatherForecast.csproj)

**Weather.razor**

```razor
@page "/weather"
@attribute [StreamRendering(true)]
@using BlazorWebApp.Data
@using BlazorWebApp.Service

@inject WeatherApiClient WeatherApi

<PageTitle>Weather</PageTitle>

<h1>Weather</h1>

<p>This component demonstrates showing data loaded from a backend API service.</p>

@if (forecasts == null)
{
    <p><em>Loading...</em></p>
}
else
{
    <table class="table">
        <thead>
            <tr>
                <th>Date</th>
                <th>Temp. (C)</th>
                <th>Temp. (F)</th>
                <th>Summary</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var forecast in forecasts)
            {
                <tr>
                    <td>@forecast.Date.ToShortDateString()</td>
                    <td>@forecast.TemperatureC</td>
                    <td>@forecast.TemperatureF</td>
                    <td>@forecast.Summary</td>
                </tr>
            }
        </tbody>
    </table>
}

@code {
    private WeatherForecast[]? forecasts;

    protected override async Task OnInitializedAsync()
    {
        forecasts = await WeatherApi.GetWeatherAsync();
    }
}
```

We also has to modify the Blazor Web App **middleware** (Program.cs) file for adding the **API URL** and configure the **CORS**

```csharp
// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWeatherApi", builder =>
    {
        builder.WithOrigins("https://localhost:7266") // Replace with your actual API URL if different
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials(); // Enable if your app uses authentication cookies
    });
});

// Configure HttpClient for WeatherApiClient
builder.Services.AddHttpClient<WeatherApiClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7266"); // API base address
});
```

This is the **Blazor Web App** whole **Program.cs** file

```csharp
using BlazorWebApp.Components;
using BlazorWebApp.Service;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWeatherApi", builder =>
    {
        builder.WithOrigins("https://localhost:7266") // Replace with your actual API URL if different
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials(); // Enable if your app uses authentication cookies
    });
});

// Configure HttpClient for WeatherApiClient
builder.Services.AddHttpClient<WeatherApiClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7266"); // API base address
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
```

## 6. Add the Aspire.NET Host project

We right click on the solution and select the menu option add new project

![image](https://github.com/user-attachments/assets/04e5d817-93a7-4355-a9b4-05902c8e29f6)

We search for the Aspire project templates and selec the **.NET Aspire App Host** and we click on the Next button

![image](https://github.com/user-attachments/assets/138109d8-c6d4-4715-9335-858f71886fb0)

Then we input the project name and location and we press the Next button

![image](https://github.com/user-attachments/assets/09faa2b1-caae-4d2f-aeec-87db6bc00b2c)

Then we select the .NET 8 framework and we press the Create button

![image](https://github.com/user-attachments/assets/15959733-f45c-4565-b14f-1fb9cb3f25a3)

We update the Nuget packages

![image](https://github.com/user-attachments/assets/3d9b82ef-f2bf-4cbf-8da7-b4954b3707fd)

We select the Updates tab and select all and then press the Update button

![image](https://github.com/user-attachments/assets/e5356e70-a89c-49cd-b464-b94c227e46d6)

We confirm all the Nuget packages were updated

![image](https://github.com/user-attachments/assets/ae19a2c1-ea44-4106-8c4d-619ebd940cf0)

## 7. Add the ServerDefault Aspire project

We right click on the solution and select the menu option add new project

![image](https://github.com/user-attachments/assets/82675e50-e0bf-4e4c-b230-54c8d2ee5316)

We select the project template **.NET Aspire Service Defaults** and we press the Next button 

![image](https://github.com/user-attachments/assets/36f585f2-b0e2-4bd3-bf0c-8a9e35861abb)

We input the project name and location and we press the Next button 

![image](https://github.com/user-attachments/assets/a9b621c6-a3e7-4795-9e0b-7c6d552c65ca)

We select the .NET8 framework and we press the Create button

![image](https://github.com/user-attachments/assets/e838a974-cb82-4b24-b6c3-8e27043abc48)

This is our solution projects structure

![image](https://github.com/user-attachments/assets/12a63047-0dff-4e67-a6dc-f3f869ee16ef)

Now if we review the **Blazor Web App** and the **Web API** projects, we can verify that automatically in our projects were include a reference to the **ServerDefault** project

![image](https://github.com/user-attachments/assets/bbb90eaa-9e10-4cb2-a6e4-3d2171d61359)

Also if we review the middleware in both projects we confirm new code was added:

**Blazor Web App middleware**

![image](https://github.com/user-attachments/assets/452d90df-b159-4427-a807-24b08a7d9507)

**Web API middleware**

![image](https://github.com/user-attachments/assets/03110281-7018-4db8-87f6-b3b4db5df62d)

## 8. Add in both project .NET Aspire Orchestrator Support

We first right click on the API project name and select the menu option **Add->.NET Aspire Orchestrator Support...**

![image](https://github.com/user-attachments/assets/b0bc392b-0567-435c-a9ce-a430253e5443)

![image](https://github.com/user-attachments/assets/8164cf46-9c3d-4a04-8213-954964c261b6)

We also right click on the Blazor Web App project name and select the menu option **Add->.NET Aspire Orchestrator Support...**

![image](https://github.com/user-attachments/assets/c0f74827-b24d-4f35-a684-786907c3d5d7)

![image](https://github.com/user-attachments/assets/63b7891d-f1d8-45a5-aa2b-994ab15f5669)

We verify both projects references were added in the **AppHost1.csproj** file

![image](https://github.com/user-attachments/assets/ec51d80a-e3b3-4864-b812-77bf25fa5bea)

And also the **Program.cs** in the **AppHost1** was modified

![image](https://github.com/user-attachments/assets/5d5de4c0-510c-4c10-8aa0-9252cfdb62c3)

## 9. Run the application

We press on the HTTPS protocol after setting as start up project the Host 

![image](https://github.com/user-attachments/assets/0e311fb6-492f-47ab-b7e7-ad0ef0814e9b)

We first see the Dashboard running in this URL: https://localhost:17291/

![image](https://github.com/user-attachments/assets/1b50c379-30c5-4564-a099-0b4a9acedc31)

We can navigate to the **Blazor Web App URL**: https://localhost:7159/

![image](https://github.com/user-attachments/assets/5fb430c1-95fa-4fd1-8783-1b877dc3c00f)

We can also navigate to the **Web API URL**:

![image](https://github.com/user-attachments/assets/00159171-bcf5-4995-9fbe-67c07438fde9)
