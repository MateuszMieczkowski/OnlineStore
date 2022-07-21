using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SneakersBase.Server.Entities;
using SneakersBase.Shared.Models;

namespace SneakersBase.Server
{
    public interface IAzureStorage
    {
        Task<string> Upload(IFormCollection formCollection);
        Task Upload(IEnumerable<CreateProductDto> products);
    }

    public class AzureStorage : IAzureStorage
    {
        private readonly string _storageConnectionString;
        private readonly string _storageContainerName;
        private readonly ILogger<AzureStorage> _logger;
        public AzureStorage(IConfiguration configuration, ILogger<AzureStorage> logger)
        {
            _storageConnectionString = configuration.GetValue<string>("BlobConnectionString");
            _storageContainerName = configuration.GetValue<string>("BlobContainerName");
            _logger = logger;
        }

        public async Task<string> Upload(IFormCollection formCollection)
        {
            var container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
            var createResponse = await container.CreateIfNotExistsAsync();
            if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                await container.SetAccessPolicyAsync(PublicAccessType.Blob);

            var file = formCollection.Files.First();
            //   foreach (var file in formCollection.Files)
            // {
            var blob = container.GetBlobClient(file.FileName);
            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
            using (var fileStream = file.OpenReadStream())
            {
                await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = file.ContentType });
            }
            return blob.Uri.ToString();
            //   }
        }
        public async Task Upload(IEnumerable<CreateProductDto> products)
        {
            var container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
            var createResponse = await container.CreateIfNotExistsAsync();
            if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                await container.SetAccessPolicyAsync(PublicAccessType.Blob);

            string contentType = "image/png";
            foreach (var dto in products)
            {
                var blob = container.GetBlobClient(dto.ReferenceNumber);
                await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
                var bytes = Convert.FromBase64String(dto.ThumbnailPath);
                using (var fileStream = new StreamContent(new MemoryStream(bytes)).ReadAsStream())
                {
                    await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType });
                }
            }
        }
    }

}

