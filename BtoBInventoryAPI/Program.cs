using BtoBInventoryAPI.Data;
using BtoBInventoryAPI.Hubs;
using BtoBInventoryAPI.Repositories;
using BtoBInventoryAPI.Services;
using BtoBInventoryAPI.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection(nameof(MongoDBSettings)));
builder.Services.AddSingleton<IMongoDBSettings>(sp => sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(sp.GetRequiredService<IMongoDBSettings>().ConnectionString));
builder.Services.AddScoped<IDatabaseContext, AppDbContext>();

// Register UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register other services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IInventoryServices, InventoryService>();
builder.Services.AddScoped<IExportService, ExportService>();
builder.Services.AddScoped<IImportService, ImportService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();


// Register repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IExportRepository, ExportRepository>();
builder.Services.AddScoped<IImportRepository, ImportRepository>();  
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseDefaultFiles();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors(corsPolicyBuilder =>
   corsPolicyBuilder.WithOrigins("http://localhost:4200", "http://localhost:60479","http://localhost:8000")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

app.UseAuthorization();

app.MapControllers();
app.MapHub<ProductHub>("/producthub");
app.MapHub<InventoryHub>("/inventoryhub");



app.Run();
