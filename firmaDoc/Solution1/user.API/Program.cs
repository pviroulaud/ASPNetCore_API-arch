using Microsoft.EntityFrameworkCore;
using userEntities.userModel;

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
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("userConnectionString")));
Console.WriteLine(">>>> Connecting to MSSQL via userConnectionString: " + builder.Configuration.GetConnectionString("userConnectionString"));



builder.Services.AddCors(options =>
{
    options.AddPolicy(origins,
    builder =>
    {
        //builder.WithOrigins("*");
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Add services to the container.

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
