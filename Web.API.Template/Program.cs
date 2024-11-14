using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharedLibraryTemplate.Models;
using System.Text;
using Web.API.Template.Data;
using Web.API.Template.Data.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddDefaultTokenProviders();


//add jwt authentication
JwtSettings? jwtSettings = new();
builder.Configuration.GetRequiredSection("JwtSettings").Bind(jwtSettings);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).
AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer= jwtSettings?.Issuer ?? throw new ArgumentNullException("Jwt:Issuer"),
        ValidAudience= jwtSettings?.Audience ?? throw new ArgumentNullException("Jwt:Audience"),
        IssuerSigningKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key??throw new ArgumentNullException("Jwt:Key")))
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("User", policy => policy.RequireRole("User"));
});
// Add services to the container.
//Allow CORS
string AllowBlazorWasm = "AllowBlazorWasm";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowBlazorWasm,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7243")
                                .AllowAnyHeader()
                                .AllowAnyMethod();

                      });
});



builder.Services.AddControllers();
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

app.UseCors(AllowBlazorWasm);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
