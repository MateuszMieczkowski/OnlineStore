using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using SneakersBase.Server.Services.Exceptions;

namespace SneakersBase.Server.Controllers
{
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
            var formCollection = await Request.ReadFormAsync();
            foreach (var file in formCollection.Files.Where(s => s.Length == 0))
            {
                throw new BadRequestException($"File {file.Name} is empty");
            }

            var uri = await _azureStorage.Upload(formCollection);
            return Ok(uri);
        }
    }
}
