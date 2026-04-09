using BlockedCountriesApi.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddSingleton<CountryBlockService>();
builder.Services.AddScoped<TemporalBlockService>();
builder.Services.AddHttpClient<IGeoLocationService, GeoLocationService>();
builder.Services.AddScoped<GeoLocationService>();
builder.Services.AddHostedService<TemporalBlockCleanupService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Map controllers
app.MapControllers();

app.Run();