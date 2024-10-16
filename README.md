# How convert a Blazor Web App and a .NET Web API into a .NET Aspire project

For more information to learn .NET Aspire visit this URL: 

https://learn.microsoft.com/en-us/training/paths/dotnet-aspire/

## 1. Create a Blazor Web App

We run Visual Studio 2022 Community Edition and create a new project

![image](https://github.com/user-attachments/assets/ade8b6f0-ba7a-4fc5-b865-80e1af1eb750)

We select the Blazor Web App project template

![image](https://github.com/user-attachments/assets/0efe8a25-d67f-452a-8130-e0984dc46d3f)

We input the project name and location

![image](https://github.com/user-attachments/assets/a9168373-669e-4e2c-9526-9271520a9d2e)

We select the .NET 9 framework

![image](https://github.com/user-attachments/assets/f371adf6-a0bd-43fe-8881-244dd74517ae)

We verify the project folders structure

![image](https://github.com/user-attachments/assets/58b1ca48-9e2f-4081-a8a5-ecf98e643a5c)

## 2. Add a new project with the template .NET Core Web API

We right click on the Solution and we select the menu option add new project

![image](https://github.com/user-attachments/assets/2a89ddfa-582f-47d8-9b14-9ab19a8bf8e1)

We search for the Web API project templates and we select the **ASP.NET Core Web API**

![image](https://github.com/user-attachments/assets/632357fc-64c7-42e5-ade6-4e9fcc8eeb38)

We input the project name, we select the project folder and press the next button

![image](https://github.com/user-attachments/assets/91f588db-c9bd-4d5f-b4e0-a1eeb0366b03)

We select the .NET 9 framework and we leave the other fields with the default values

![image](https://github.com/user-attachments/assets/380e3e29-3306-4dd2-883f-c83a9e16960e)

We verify in the Solution Explorer both projects inside my solution

![image](https://github.com/user-attachments/assets/e181c7c4-41db-42d6-95db-b3ce6a7cbd4a)

## 3. Move the BlazorWebApp project into a new folder

We create a new folder **BlazorWebApp**

![image](https://github.com/user-attachments/assets/c89ec71b-3f5d-4a98-96d7-ab99a96e3ec4)

We move the Blazor Web Application code inside to that folder

![image](https://github.com/user-attachments/assets/34682cb0-d563-471b-b4c8-b23ecdfa4c57)

We edit  the solution file to include the new Blazor Web App location

![image](https://github.com/user-attachments/assets/9cbcb805-5c5e-4d2f-bb7e-fd99e55e15c9)

We save the modified solution file

![image](https://github.com/user-attachments/assets/3d29a9cf-ae01-4f64-8902-4b05225d103c)

We open the solution in Visual Studio 2022

![image](https://github.com/user-attachments/assets/c0bc8e56-867d-41b3-aef3-9490c2d51d8b)

## 4. Run both projects to verify them

First we right click on **BlazorWebApp.csproj** and select the menu option **Set as StartUp Project**

![image](https://github.com/user-attachments/assets/c39e2112-552a-4f42-bf1d-29bf0bb97efb)

We run the project and see the result:

![image](https://github.com/user-attachments/assets/72b67be6-b2cf-4544-b435-38675e6274f0)

Now we right click on **WebAPIWeatherForecast** and select the menu option **Set as StartUp Project**. See the result:

![image](https://github.com/user-attachments/assets/00ecfe4d-faaa-48c0-8da8-06579181fb5a)

We run the project and see the result:

![image](https://github.com/user-attachments/assets/20887317-89ca-4121-add6-0baf4b1667c0)

## 5. Modify the Blazor Web App to consume the .NET Core Web API

We first create a Service and a Data model for consuming the API

We create a **Service** and a **Data** folders

![image](https://github.com/user-attachments/assets/8f34ccb0-c2c9-4b7b-b259-efa286245973)

We create the **Data model** in the file **WeatherForecast.cs**

![image](https://github.com/user-attachments/assets/b76e57e9-ede7-43f6-96e6-7a541d35e50c)

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

![image](https://github.com/user-attachments/assets/63db0ccf-c970-4690-867a-3045c0f92694)

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

![image](https://github.com/user-attachments/assets/7532ad65-d680-4c4b-91f0-824e3aa23ff0)

We search for the Aspire project templates and selec the **.NET Aspire App Host** and we click on the Next button

![image](https://github.com/user-attachments/assets/039aa36b-f859-4420-9399-9e3395c97956)

Then we input the project name and location and we press the Next button

![image](https://github.com/user-attachments/assets/ef3fe023-5611-4bcf-9e14-3d8b0a08e2f8)

Then we select the .NET 8 framework and we press the Create button

![image](https://github.com/user-attachments/assets/8184935e-a8b7-4a17-8521-9acdfa56f601)

We update the Nuget packages

![image](https://github.com/user-attachments/assets/efa8ba23-2b32-433a-a513-95dbdc9b0d59)

We select the Updates tab and select all and then press the Updat button

![image](https://github.com/user-attachments/assets/95636146-a0db-4185-b0ba-2849b61f58cf)

We confirm all the Nuget packages were updated

![image](https://github.com/user-attachments/assets/c002c3b5-530e-4481-9015-c027c1fd4405)

## 7. Add the ServerDefault Aspire project

We right click on the solution and select the menu option add new project

![image](https://github.com/user-attachments/assets/7532ad65-d680-4c4b-91f0-824e3aa23ff0)

We select the project template **.NET Aspire Service Defaults** and we press the Next button 

![image](https://github.com/user-attachments/assets/074af83f-76d5-44d2-a686-39dec8cc8c61)

We input the project name and location and we press the Next button 

![image](https://github.com/user-attachments/assets/c1e6f1af-0344-4d32-b259-ed9893109a3a)

We select the .NET8 framework and we press the Create button

![image](https://github.com/user-attachments/assets/c2022cf0-7332-4203-bd1f-fa170a03f8f4)

This is our solution projects structure

![image](https://github.com/user-attachments/assets/957f8cdf-1ca6-43ef-b188-8b835775f585)

Now if we review the **Blazor Web App** and the **Web API** projects, we can verify that automatically in our projects were include a reference to the **ServerDefault** project

![image](https://github.com/user-attachments/assets/2e6a9126-756c-4610-82a3-ac61e06f056a)

Also if we review the middleware in both projects we confirm new code was added:

**Blazor Web App middleware**

![image](https://github.com/user-attachments/assets/703f2f6e-0d2f-4f50-a61e-6a890b0a54c3)

**Web API middleware**

![image](https://github.com/user-attachments/assets/1903b7e5-fdf0-4465-b350-13064bbede25)

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
