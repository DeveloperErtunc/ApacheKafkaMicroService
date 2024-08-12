using Microsoft.Extensions.Hosting;
using Shared.Models.Constants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IProducerBusService, ProducerBusService>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();
var busService = app.Services.GetService<IProducerBusService>();
await busService.CreateTopic();
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
