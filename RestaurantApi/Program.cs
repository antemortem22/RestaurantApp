using RestaurantApi.Repository;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;


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

// Db connection
string connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ReservaRestaurantContext>(config =>
{
    config.UseSqlServer(connectionString);
});



//

var app = builder.Build();

// Configure HTTP request pipeline

    // also swagger confing inside http request
app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();