using Microsoft.EntityFrameworkCore;
using TerminosApi.Data;
using QuestPDF.Infrastructure;
using TerminosApi.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Conexion")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular",
        policy =>
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod());
});
QuestPDF.Settings.License = LicenseType.Community;
builder.Services.AddScoped<PdfReportService>();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("Angular");

app.MapControllers();

app.Run();