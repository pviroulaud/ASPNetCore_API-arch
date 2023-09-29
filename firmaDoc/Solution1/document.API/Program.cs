using documentAPI.AsyncServices;
using documentAPI.SyncServices;
using documentEntities.documentModel;
using fileDTO;
using logDTO;
using Microsoft.EntityFrameworkCore;

string origins = "_myAllowOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // Buscar el Mapeo de objetos de la clase que hereda de Automapper.Profiles

if (builder.Environment.IsProduction())
{
    Console.WriteLine(">>> Production Environment");
}
else
{
    Console.WriteLine(">>> Development Environment");
}
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("documentConnectionString")));
Console.WriteLine(">>>> Connecting to MSSQL via documentConnectionString: " + builder.Configuration.GetConnectionString("documentConnectionString"));



builder.Services.AddCors(options =>
{
    options.AddPolicy(origins,
    builder =>
    {
        //builder.WithOrigins("*");
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddHttpClient<IHttpLocalClient, HttpLocalClient>();
builder.Services.AddSingleton<IRabbitMQClient<operationDTO>, RabbitMQClient<operationDTO>>(
                                            serviceProvider => new RabbitMQClient<operationDTO>(
                                                    configuration: serviceProvider.GetRequiredService<IConfiguration>(),
                                                    exchangeName: "log"));
builder.Services.AddSingleton<IRabbitMQClient<errorDTO>, RabbitMQClient<errorDTO>>(
                                            serviceProvider => new RabbitMQClient<errorDTO>(
                                                    configuration: serviceProvider.GetRequiredService<IConfiguration>(),
                                                    exchangeName: "errorLog"));
builder.Services.AddSingleton<IRabbitMQClient<rabbitMqFileTransferDTO>, RabbitMQClient<rabbitMqFileTransferDTO>>(
                                            serviceProvider => new RabbitMQClient<rabbitMqFileTransferDTO>(
                                                    configuration: serviceProvider.GetRequiredService<IConfiguration>(),
                                                    exchangeName: "file"));

builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddHostedService<RabbitMQFileInfoSubscriber>();

builder.Services.AddControllers();
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
