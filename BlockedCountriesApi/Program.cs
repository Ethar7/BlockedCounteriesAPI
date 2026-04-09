using BlockedCountriesApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddScoped<CountryBlockService>();
builder.Services.AddScoped<TemporalBlockService>();

builder.Services.AddHttpClient<IGeoLocationService, GeoLocationService>();

builder.Services.AddHostedService<TemporalBlockCleanupService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
