using Microsoft.EntityFrameworkCore;
using Moviles;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregar servicios al contenedor
builder.Services.AddControllers();

// 2. Configurar la conexión a la base de datos
builder.Services.AddDbContext<MovilDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// 3. Configurar Swagger para documentación (opcional)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Configurar middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
