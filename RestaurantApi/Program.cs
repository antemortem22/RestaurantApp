using Microsoft.EntityFrameworkCore;
using RestaurantApi.Repository;
using RestaurantApi.Repository.Interface;
using RestaurantApi.Serialization;
using RestaurantApi.Services;
using RestaurantApi.Services.Interface;
using RestaurantApi.Services.Validation;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        o.JsonSerializerOptions.Converters.Add(new ArgentinaDateTimeJsonConverter());
        o.JsonSerializerOptions.Converters.Add(new NullableArgentinaDateTimeJsonConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IReservaService, ReservaService>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<IReservaValidator, ReservaValidator>();
builder.Services.AddScoped<ICalendarioSemanalService, CalendarioSemanalService>();
builder.Services.AddScoped<ICalendarioSemanalRepository, CalendarioSemanalRepository>();

builder.Services.AddProblemDetails();

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Missing connection string: ConnectionStrings:Default");

builder.Services.AddDbContext<ReservaRestaurantContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseExceptionHandler();

// Seed automático (idempotente)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ReservaRestaurantContext>();
    await SeedData.InitializeAsync(dbContext);
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
