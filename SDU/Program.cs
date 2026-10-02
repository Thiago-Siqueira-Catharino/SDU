using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SDU.Contexts.Auth.Application.UseCases.LoginUseCase;
using SDU.Contexts.Auth.Application.UseCases.NewCitizenUseCase;
using SDU.Contexts.Auth.Application.UseCases.NewEntityUseCase;
using SDU.Contexts.Auth.Application.UseCases.NewProfessionalUseCase;
using SDU.Contexts.Auth.Domain.Entities;
using SDU.Contexts.Auth.Infrastructure.Persistance;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString)));
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//Auth context setup
builder.Services
    .AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();
    
builder.Services.AddScoped<NewCitizenUseCase>();
builder.Services.AddScoped<NewProfessionalUseCase>();
builder.Services.AddScoped<NewEntityUseCase>();
builder.Services.AddScoped<LoginUseCase>();

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
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
    };
});

builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.
        GetRequiredService<RoleManager<IdentityRole>>();
    
    string[] roles = {"Citizen", "Professional", "Entity"};
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => 
        options.SwaggerEndpoint("/openapi/v1.json", "API v1")
    );
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();