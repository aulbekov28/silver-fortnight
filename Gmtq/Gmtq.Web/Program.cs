using Gmtq.Data;
using Gmtq.Web.Services;
using Gmtq.Web.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TIDE API", Version = "v1" });
});
builder.Services.AddCors();

builder.Services.AddDbContext<CurrencyContext>(optionActions =>
    optionActions.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")!));

builder.Services.AddScoped<ICurrencyRateService, CurrencyRateService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(options =>
    options.WithOrigins("https://localhost:44430")
        .AllowAnyMethod()
        .AllowAnyHeader());

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapFallbackToFile("index.html");

app.Run();