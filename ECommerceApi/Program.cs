using EcommerceApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using EcommerceApi.Application.Interfaces;
using EcommerceApi.Application.Services;
using ECommerceApi.Infrastructure.Middlewares;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

ExcelPackage.License.SetNonCommercialPersonal("Joe Doe");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IProductService, ProductService>();



builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    );
});


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

app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.MapControllers();

app.Run();