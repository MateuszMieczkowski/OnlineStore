using Microsoft.AspNetCore.Mvc;
using OnlineStore.Server.Services.Exceptions;

namespace OnlineStore.Server.Controllers;

[Route("api/upload")]
[ApiController]
public class UploadController : ControllerBase
{
    private readonly IBlobStorage _azureStorage;

    public UploadController(IBlobStorage azureStorage)
    {
        _azureStorage = azureStorage;
    }

    [HttpPost]
    public async Task<IActionResult> Upload()
    {
        return Ok();
    }
}