using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SDU.Contexts.Files.Application.UseCases.UploadUseCase;

namespace SDU.Contexts.Files.Presentation;

[ApiController]
[Route("api/[controller]")]
public class FileController(
    UploadUseCase _uploadUseCase
    ) : ControllerBase
{
    
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [Authorize(Roles = "Entity, Professional")]
    public async Task<IActionResult> ReceiveFile([FromForm] UploadDto uploadDto)
    {
        var userId = User.FindAll(ClaimTypes.NameIdentifier)
            .Select(c => c.Value)
            .FirstOrDefault(v => Guid.TryParse(v, out _));
        
        uploadDto =  uploadDto with{ ProfessionalId =  userId };
        await _uploadUseCase.Run(uploadDto);
        return Ok();
    }
}