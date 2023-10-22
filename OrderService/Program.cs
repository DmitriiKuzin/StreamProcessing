using DAL;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MQ;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BillingDbContext>();
builder.Services.AddRabbitMq();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/createOrder", async (Order orderDto, BillingDbContext dbContext, IBus bus) =>
{
    var order = new Order
    {
        Id = orderDto.Id,
        Price = orderDto.Price,
        UserProfileId = orderDto.UserProfileId
    };
    dbContext.Orders.Add(order);
    await dbContext.SaveChangesAsync();
    await bus.Publish(new OrderCreated(order.Id, order.Price, order.UserProfileId));
});


await MqExtension.WaitForRabbitReady();
var dbContext = app.Services.CreateScope().ServiceProvider.GetService<BillingDbContext>();
dbContext.Database.Migrate();
app.Run();