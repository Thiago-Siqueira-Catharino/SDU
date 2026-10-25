using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SDU.Contexts.Auth.Application.UseCases.DownloadUseCase;
using SDU.Contexts.Auth.Application.UseCases.UploadUseCase;

namespace SDU.Contexts.Auth.Presentation;

public class CitizenFileController : ControllerBase
{
    private readonly UploadFileUseCase _uploadUseCase;
    private readonly DownloadFileUseCase _downloadUseCase;

    public CitizenFileController(
        UploadFileUseCase uploadUseCase, 
        DownloadFileUseCase downloadUseCase)
    {
        _uploadUseCase = uploadUseCase;
        _downloadUseCase = downloadUseCase;
    }

    // Bloqueia no nível da API: Apenas usuários com a role "Professional" podem acessar
    [HttpPost("upload")]
    [Authorize(Roles = "Professional")] 
    public async Task<IActionResult> Upload(
        [FromForm] IFormFile file, 
        [FromForm] Guid ownerId)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Nenhum arquivo foi enviado.");

        var uploaderIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(uploaderIdString, out Guid uploaderId))
            return Unauthorized("Usuário inválido.");

        using var stream = file.OpenReadStream();

        var input = new UploadFileInput
        {
            OwnerId = ownerId,
            UploaderId = uploaderId, // Este ID agora tem garantia de ser de um Professional devido à anotação [Authorize]
            FileName = file.FileName,
            ContentType = file.ContentType,
            Size = file.Length,
            ContentStream = stream
        };

        var result = await _uploadUseCase.ExecuteAsync(input);
        return Ok(result);
    }

    // Acesso liberado para "Citizen" e "Professional" (ou qualquer usuário autenticado)
    [HttpGet("download/{fileId}")]
    [Authorize] 
    public async Task<IActionResult> Download(Guid fileId)
    {
        var requesterIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(requesterIdString, out Guid requesterId))
            return Unauthorized("Usuário inválido.");

        var input = new DownloadFileInput
        {
            FileId = fileId,
            RequesterId = requesterId
        };

        try
        {
            var result = await _downloadUseCase.ExecuteAsync(input);
            return File(result.ContentStream, result.ContentType, result.FileName);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}