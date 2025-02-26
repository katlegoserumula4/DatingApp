using System.Security.Cryptography.X509Certificates;

using API.Data;
using Microsoft.EntityFrameworkCore;
using System.IO;

var builder = WebApplication.CreateBuilder(args);


var certificatePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "client", "cert.pfx");
var certificate = X509CertificateLoader.LoadPkcs12FromFile(certificatePath, "cert123");

builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureHttpsDefaults(listenOptions =>
    {
        listenOptions.ServerCertificate = certificate;
    });
});


// Configure services
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add Controllers
builder.Services.AddControllers();

// Swagger setup
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS setup
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Make sure this is correct
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Middleware order - CORS must be before routing and authorization
app.UseHttpsRedirection();
app.UseCors("MyAllowSpecificOrigins");  // CORS must be applied before UseRouting()
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

// Swagger UI configuration (only for Development environment)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "DatingApp API is running!");

app.Run();
