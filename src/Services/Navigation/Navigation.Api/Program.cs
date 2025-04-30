using Navigation.Api;
using Navigation.Api.NotificationService;
using Navigation.Application;
using Navigation.Application.Notifications;
using Navigation.Infrastructure;
using Navigation.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseApiServices();
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}
app.Run();
