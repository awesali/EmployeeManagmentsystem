using System.Diagnostics;
using System.Text;
using EmployeeManagmentsystem.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("https://localhost:7140") // Yahan frontend ka origin dalen
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add Authentication and configure JWT Bearer options
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("jdfoiogfxfcghilkfjgxhciuojfhxgbnbcmvbjyf"))
        };
    });
builder.Services.AddScoped<JwtService>();

// Configure Swagger to generate API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure the database context
builder.Services.AddDbContext<EmployeeDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeConnection")));

var app = builder.Build();

// Swagger configuration should be before Authentication and Authorization
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Use CORS middleware
app.UseCors("AllowSpecificOrigins"); // Apply the CORS policy

// Authentication and Authorization middlewares
app.UseAuthentication(); // Ensure Authentication middleware is added
app.UseAuthorization();  // Ensure Authorization middleware is added

// Map controllers for the API
app.MapControllers();

app.Run();
