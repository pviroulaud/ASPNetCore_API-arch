using log.API.AsyncServices;
using log.API.Repositories;
using logEntities.logModel;
using Microsoft.EntityFrameworkCore;

string origins = "_myAllowOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // Buscar el Mapeo de objetos de la clase que hereda de Automapper.Profiles

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("logConnectionString")));

builder.Services.AddCors(options =>
{
    options.AddPolicy(origins,
    builder =>
    {
        //builder.WithOrigins("*");
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddControllers();

builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddSingleton<ILogRepository, LogRepository>();


builder.Services.AddHostedService<RabbitMQLogSubscriber>(serviceProvider => new RabbitMQLogSubscriber(
    configuration:serviceProvider.GetRequiredService<IConfiguration>(),
    eventProcessor:serviceProvider.GetRequiredService<IEventProcessor>(),
    context:serviceProvider.GetRequiredService<ILogRepository>()));

builder.Services.AddHostedService<RabbitMQErrorSubscriber>(serviceProvider => new RabbitMQErrorSubscriber(
    configuration: serviceProvider.GetRequiredService<IConfiguration>(),
    eventProcessor: serviceProvider.GetRequiredService<IEventProcessor>(),
    context: serviceProvider.GetRequiredService<ILogRepository>()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
