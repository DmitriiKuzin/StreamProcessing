using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MQ;
using NotificationService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BillingDbContext>();
builder.Services.AddRabbitMq(x =>
{
    x.AddConsumer<EnoughFundsConsumer>();
    x.AddConsumer<InsufficientFundsConsumer>();
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/notifications", async ([FromQuery] long userProfileId, BillingDbContext dbContext) =>
{
    var notifications = await dbContext
        .Notifications
        .AsNoTracking()
        .Where(x => x.UserProfileId == userProfileId)
        .OrderByDescending(x => x.Id)
        .Select(x => x.Message)
        .ToListAsync();
   return notifications;
});


await MqExtension.WaitForRabbitReady();
app.Run();
