using EquipmentLoanSystem.Data;
using EquipmentLoanSystem.Core.Interfaces;
using EquipmentLoanSystem.Data.Repositories;
using EquipmentLoanSystem.Service.Services;
using EquipmentLoanSystem.API.Profiles;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

// 1. יצירת ה-builder תחילה
var builder = WebApplication.CreateBuilder(args);

// 2. קריאת הגדרות ה-JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Secret"] ?? "DEFAULT_SECRET_KEY_IF_NOT_FOUND_IN_CONFIG_123456";
var key = Encoding.UTF8.GetBytes(secretKey);

// 3. הגדרת Authentication & Authorization
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

// 4. הגדרת CORS
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? [];
// הגדרת מדיניות CORS פתוחה לכל המקורות (כולל http://localhost:3000)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});
// 5. רישום שירותים, Controllers, DbContext ו-AutoMapper
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IItemCopyRepository, ItemCopyRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<ILoanService, LoanService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<AuthService>(); // רישום שירות ה-Auth

builder.Services.AddAutoMapper(_ => { }, typeof(MappingProfile).Assembly);

// 6. בניית האפליקציה
var app = builder.Build();
app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Equipment Loan API V1");
    c.RoutePrefix = "swagger";
});



// 7. הפעלת ה-Authentication וה-Authorization בסדר הנכון
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();