using Microsoft.AspNetCore.Mvc;
using SDU.Contexts.Auth.Application.UseCases.LoginUseCase;

namespace SDU.Contexts.Auth.Presentation;

[ApiController]
[Route("api/[controller]")]
public class UserController (
    LoginUseCase loginUseCase,
    LoginDto loginDto
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
}