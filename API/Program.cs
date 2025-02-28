using System.Security.Cryptography.X509Certificates;

using API.Data;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using API.Interfaces;
using API.Services;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;



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


builder.Services.AddScoped<ITokenService, TokenService>();

var tokenKey = builder.Configuration["TokenKey"];
if (string.IsNullOrEmpty(tokenKey))
{
    throw new ArgumentException("TokenKey is missing from the configuration.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>{
    options.TokenValidationParameters = new TokenValidationParameters{
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
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
app.UseAuthentication();
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
