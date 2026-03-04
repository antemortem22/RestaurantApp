using RestaurantApi.Repository;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using RestaurantApi.Services.Interface;
using RestaurantApi.Services;
using RestaurantApi.Repository.Interface;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services.AddControllers();

// Swagger config
builder.Services.AddSwaggerGen();

//System Text Json config
builder.Services.AddMvc()
        .AddJsonOptions(o => {
            //ignoreCircle
            o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });


builder.Services.AddScoped<IReservaService, ReservaService>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<ICalendarioSemanalRepository, CalendarioSemanalRepository>();
builder.Services.AddScoped<ICalendarioSemanalService, CalendarioSemanalService>();



// Db connection
string connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ReservaRestaurantContext>(config =>
{
    config.UseSqlServer(connectionString);
});



//

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ReservaRestaurantContext>();
    await SeedData.InitializeAsync(dbContext);
}



app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();