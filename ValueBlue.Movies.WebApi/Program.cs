using Microsoft.OpenApi.Models;
using Serilog;
using ValueBlue.Movies.Application.Interfaces;
using ValueBlue.Movies.Application.Services;
using ValueBlue.Movies.Domain.Models;
using ValueBlue.Movies.Infrastructure.ServiceAgent;
using ValueBlue.Movies.Persistence.Repositories;
using ValueBlue.Movies.WebApi.Configuration;

var builder = WebApplication.CreateBuilder(args);

//Service registration.
builder.Services.Configure<OmdbServiceConfig>(builder.Configuration.GetSection("OmdbServiceConfig"));
builder.Services.Configure<MovieSearchHistoryDatabaseSettings>(builder.Configuration.GetSection("MovieSearchHistoryDatabase"));
builder.Services.AddSingleton<ISearchRequestRepository, SearchRequestRepository>();
builder.Services.AddTransient<IRepositoryService, RepositoryService>();
builder.Services.AddTransient<IOmdbServiceAgent, OmdbServiceAgent>();
builder.Services.AddTransient<IMovieSearchService, MovieSearchService>();
builder.Services.AddTransient<IAdminService, AdminService>();

//httpclient
var omdbServiceConfig = builder.Configuration.GetSection("OmdbServiceConfig").Get<OmdbServiceConfig>();
builder.Services.AddHttpClient<IOmdbServiceAgent, OmdbServiceAgent>(client =>
{
    client.Timeout = TimeSpan.FromMinutes(2);
    client.BaseAddress = new Uri(omdbServiceConfig.BaseUrl);
});

//Serilog
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

//Kestrel
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5022);
    options.ListenAnyIP(7054, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();

var app = builder.Build();

// Middleware to enforce API key, only on admin endpoints
app.ConfigureApiKeyAuthenticationForAdminEndpoints();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
//app.UseAuthorization();
app.MapControllers();
app.Run();
