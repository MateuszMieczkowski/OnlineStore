﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.Identity.Client;
using SneakersBase.Server.Entities;
using SneakersBase.Shared.Models;

namespace SneakersBase.Server
{
    public interface IAzureStorage
    {
        Task<string> Upload(IFormCollection formCollection);
        Task<bool> Upload(IEnumerable<CreateProductDto> dtos, IEnumerable<Product> products);
        Task<bool> Update(int id, UpdateProductDto dto);
        Task<bool> Remove(int id);
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
            var blob = container.GetBlobClient(file.FileName);
            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
            using (var fileStream = file.OpenReadStream())
            {
                await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = file.ContentType });
            }
            return blob.Uri.ToString();
        }
        public async Task<bool> Upload(IEnumerable<CreateProductDto> dtos, IEnumerable<Product> products)
        {
            if (dtos.Count() != products.Count())
                throw new Exception("Something went wrong during uploading images");

            var container = await GetContainter();

            string contentType = "image/png";

            var em = dtos.GetEnumerator();
            foreach (var product in products)
            {
                em.MoveNext();
                var dto = em.Current;

                var blob = container.GetBlobClient(product.Id.ToString());
                await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
                var bytes = Convert.FromBase64String(dto.ThumbnailPath);
                using (var fileStream = new StreamContent(new MemoryStream(bytes)).ReadAsStream())
                {
                    await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType });
                }
            }

            return true;
        }
        public async Task<bool> Update(int id, UpdateProductDto dto)
        {
            var container = await GetContainter();

            string contentType = "image/png";


            var blob = container.GetBlobClient(id.ToString());
            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
            var bytes = Convert.FromBase64String(dto.ThumbnailPath);
            using (var fileStream = new StreamContent(new MemoryStream(bytes)).ReadAsStream())
            {
                await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType });
            }

            return true;
        }
        private async Task<BlobContainerClient> GetContainter()
        {
            var container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
            var createResponse = await container.CreateIfNotExistsAsync();
            if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                await container.SetAccessPolicyAsync(PublicAccessType.Blob);

            return container;
        }
        public async Task<bool> Remove(int id)
        {
            var container = await GetContainter();

            var blob = container.GetBlobClient(id.ToString());
            await blob.DeleteIfExistsAsync();
            return true;
        }
    }

}
