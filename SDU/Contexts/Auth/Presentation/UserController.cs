using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SDU.Contexts.Auth.Application.UseCases.GetDashboardStats;
using SDU.Contexts.Auth.Application.UseCases.GetProfessionals;
using SDU.Contexts.Auth.Application.UseCases.LoginUseCase;
using SDU.Contexts.Auth.Application.UseCases.NewCitizenUseCase;
using SDU.Contexts.Auth.Application.UseCases.NewEntityUseCase;
using SDU.Contexts.Auth.Application.UseCases.NewProfessionalUseCase;

namespace SDU.Contexts.Auth.Presentation;

[ApiController]
[Route("api/[controller]")]
public class UserController (
    LoginUseCase loginUseCase,
    NewCitizenUseCase newCitizen,
    NewProfessionalUseCase newProfessional,
    NewEntityUseCase newEntity,
    GetDashboardStatsUseCase getDashboardStatsUseCase,
    GetProfessionalsUseCase getProfessionalsUseCase
    ) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var token = await loginUseCase.Run(loginDto);

        if (String.IsNullOrEmpty(token))
            return Unauthorized();
        
        return Ok(token);
    }

    [HttpPost("register/citizen")]
    public async Task<IActionResult> CitizenSignup(NewCitizenDto newCitizenDto)
    {
        await newCitizen.Run(newCitizenDto);
        return Ok();
    }

    [Authorize(Roles = "Entity")]
    [HttpPost("register/professional")]
    public async Task<IActionResult> ProfessionalSignup(NewProfessionalDto newProfessionalDto)
    {
        var userIdStr = User.FindAll(ClaimTypes.NameIdentifier)
            .Select(c => c.Value)
            .FirstOrDefault(v => Guid.TryParse(v, out _));
        
        if (!Guid.TryParse(userIdStr, out Guid userId))
            return Unauthorized();
        
        await newProfessional.Run(newProfessionalDto, userId);
        return Ok();
    }

    [HttpPost("register/entity")]
    public async Task<IActionResult> EntitySignup(NewEntityDto newEntityDto)
    {
        await newEntity.Run(newEntityDto);
        return Ok();
    }

    [Authorize(Roles = "Entity")]
    [HttpGet("entity/getdashboard")]
    public async Task<IActionResult> GetDashboardStats()
    {
        var userIdStr = User.FindAll(ClaimTypes.NameIdentifier)
            .Select(c => c.Value)
            .FirstOrDefault(v => Guid.TryParse(v, out _));
        
        if (!Guid.TryParse(userIdStr, out Guid userId))
            return Unauthorized();
        
        return Ok(await getDashboardStatsUseCase.Run(userId));
    }

    [Authorize(Roles = "Entity")]
    [HttpGet("entity/getprofessionals")]
    public async Task<IActionResult> GetProfessionals()
    {
        var userIdStr = User.FindAll(ClaimTypes.NameIdentifier)
            .Select(c => c.Value)
            .FirstOrDefault(v => Guid.TryParse(v, out _));
        
        if (!Guid.TryParse(userIdStr, out Guid userId))
            return Unauthorized();

        return Ok(await getProfessionalsUseCase.Run(userId.ToString()));
    }
}