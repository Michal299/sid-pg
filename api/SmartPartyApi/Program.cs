using SmartPartyApi.Models;
using SmartPartyApi.Repositories;
using SmartPartyApi.Services;
using SmartPartyApi.Services.SensorListeners;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile("database.json",
            optional: false,
            reloadOnChange: true);
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.WithOrigins("*").AllowAnyHeader().AllowAnyHeader())
);
builder.Services.AddSingleton<PeopleCounterSensorService>();
builder.Services.AddSingleton<TemperatureSensorService>();
builder.Services.AddSingleton<PeopleCounterSensorRepository>();
builder.Services.AddSingleton<TemperatureSensorRepository>();
builder.Services.AddHostedService<TemperatureSensorListener>();
builder.Services.AddHostedService<PeopleCounterListener>();
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseConfiguration"));

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
