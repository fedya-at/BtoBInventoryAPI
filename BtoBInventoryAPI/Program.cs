using BtoBInventoryAPI.Data;
using BtoBInventoryAPI.Repositories;
using BtoBInventoryAPI.Services;
using BtoBInventoryAPI.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection(nameof(MongoDBSettings)));
builder.Services.AddSingleton<IMongoDBSettings>(sp => sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(sp.GetRequiredService<IMongoDBSettings>().ConnectionString));
builder.Services.AddScoped<IDatabaseContext, AppDbContext>();

// Register UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register other services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IInventoryServices, InventoryService>();

// Register repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(corsPolicyBuilder =>
   corsPolicyBuilder.WithOrigins("http://localhost:4200")
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();



app.Run();
