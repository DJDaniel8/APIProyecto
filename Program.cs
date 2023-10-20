using APIREST.Config;
using APIREST.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDBConection>(p => new DBConection(@"Server=tcp:proyectserver.database.windows.net,1433;Initial Catalog=Proyecto;
Persist Security Info=False;User ID=User;Password=Password123;
MultipleActiveResultSets=False;Encrypt=True;
TrustServerCertificate=False;Connection Timeout=30;"));

builder.Services.AddScoped<ICategoriaDAO, CategoriaDAO>();
builder.Services.AddScoped<ITrabajadorDAO, TrabajadorDAO>();
builder.Services.AddScoped<IClienteDAO, ClienteDAO>();
builder.Services.AddScoped<IProveedorDAO, ProveedorDAO>();
builder.Services.AddScoped<IVentaDAO, VentaDAO>();
builder.Services.AddScoped<IProductoDAO, ProductoDAO>();
builder.Services.AddScoped<IStockDAO, StockDAO>();

var app = builder.Build();

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
