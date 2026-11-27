using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SDU.Contexts.Auth.Application.UseCases.LoginUseCase;
using SDU.Contexts.Auth.Application.UseCases.NewCitizenUseCase;
using SDU.Contexts.Auth.Application.UseCases.NewEntityUseCase;
using SDU.Contexts.Auth.Application.UseCases.NewProfessionalUseCase;
using SDU.Contexts.Auth.Domain.Entities;
using SDU.Contexts.Auth.Infrastructure.Persistance;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using SDU.Contexts.Appointments.Application.UseCases.CancelAppointmentUseCase;
using SDU.Contexts.Appointments.Application.UseCases.CompleteAppointmentUseCase;
using SDU.Contexts.Appointments.Application.UseCases.NewAppointmentUseCase;
using SDU.Contexts.Appointments.Application.UseCases.SearchAppointmentsUseCase;
using SDU.Contexts.Appointments.Application.UseCases.SearchOwnAppointments;
using SDU.Contexts.Appointments.Domain.IRepositories;
using SDU.Contexts.Appointments.Infrastructure.Persistance;
using SDU.Contexts.Appointments.Infrastructure.Repositories;
using SDU.Contexts.Auth.Application.UseCases.GetAllProfessionals;
using SDU.Contexts.Auth.Application.UseCases.GetDashboardStats;
using SDU.Contexts.Auth.Application.UseCases.GetProfessionals;
using SDU.Contexts.Auth.Application.UseCases.IncrementEntityCounterUseCase;
using SDU.Contexts.Auth.Contracts;
using SDU.Contexts.Auth.Domain.IRepositories;
using SDU.Contexts.Auth.Infrastructure.Repositories;
using SDU.Contexts.Auth.Infrastructure.Services;
using SDU.Contexts.Files.Application.UseCases.DiagnosticUpload;
using SDU.Contexts.Files.Application.UseCases.DownloadUseCase;
using SDU.Contexts.Files.Application.UseCases.ExamUpload;
using SDU.Contexts.Files.Application.UseCases.GetExamFilesByCivilianId;
using SDU.Contexts.Files.Infrastructure.Persistance;
using FileRepository = SDU.Contexts.Files.Infrastructure.Repositories.FileRepository;
using IFileRepository = SDU.Contexts.Files.Domain.IRepositories.IFileRepository;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString)));

builder.Services.AddDbContext<FileDbContext>(options =>
    options.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString)));

builder.Services.AddDbContext<AppointmentDbContext>(options =>
    options.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString)));
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
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
builder.Services.AddScoped<GetDashboardStatsUseCase>();
builder.Services.AddScoped<IncrementEntityCounterUseCase>();
builder.Services.AddScoped<GetProfessionalsUseCase>();
builder.Services.AddScoped<GetAllProfessionalsUseCase>();
builder.Services.AddScoped<IAuthModuleApi, AuthModuleApi>();
builder.Services.AddScoped<IEntityRepository, EntityRepository>();
builder.Services.AddScoped<IProfessionalRepository, ProfessionalRepository>();

builder.Services.AddScoped<ExamUploadUseCase>();
builder.Services.AddScoped<DiagnosticUploadUseCase>();
builder.Services.AddScoped<GetCitizenFilesUseCase>();
builder.Services.AddScoped<DownloadUseCase>();
builder.Services.AddScoped<IFileRepository, FileRepository>();

builder.Services.AddScoped<NewAppointmentUseCase>();
builder.Services.AddScoped<CancelAppointmentUseCase>();
builder.Services.AddScoped<CompleteAppointmentUseCase>();
builder.Services.AddScoped<SearchAppointmentsUseCase>();
builder.Services.AddScoped<SearchOwnAppointmentsUseCase>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();


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

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, CancellationToken) =>
    {
        document.Components ??= new OpenApiComponents(); 
        document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme> 
        { 
            ["Bearer"] = new OpenApiSecurityScheme 
            { 
                Type = SecuritySchemeType.Http, 
                Scheme = "bearer",
                BearerFormat = "JWT"
            }
        };
        
        document.Security =
        [
            new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            }
        ];
        return Task.CompletedTask;
    });
});

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