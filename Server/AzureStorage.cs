using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Options;
using OnlineStore.Shared.Models;

namespace OnlineStore.Server;

public interface IBlobStorage
{
    Task<string> UploadAsync(Guid fileId, string fileName, string fileBase64);
    Task<bool> RemoveAsync(Guid id);
}

public class AzureStorage : IBlobStorage
{
    private readonly ILogger<AzureStorage> _logger;
    private readonly BlobStorageOptions _storageOptions;

    public AzureStorage(IOptions<BlobStorageOptions> storageOptions, ILogger<AzureStorage> logger)
    {
        _storageOptions = storageOptions.Value;
        _logger = logger;
    }

    public async Task<string> UploadAsync(Guid fileId, string fileName, string fileBase64)
    {
        var container = await GetContainerAsync();
        var blobFilename = fileId + Path.GetExtension(fileName);
        var blob = container.GetBlobClient(blobFilename);
        await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
        var fileBinary = Convert.FromBase64String(fileBase64);
        await using (var fileStream = await new StreamContent(new MemoryStream(fileBinary)).ReadAsStreamAsync())
        {
            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out var contentType);
            await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType });
        }

        return blob.Uri.ToString();
    }

    public async Task<bool> RemoveAsync(Guid id)
    {
        var container = await GetContainerAsync();
        var blob = container.GetBlobClient(id.ToString());
        
        return await blob.DeleteIfExistsAsync();
    }

    private async Task<BlobContainerClient> GetContainerAsync()
    {
        var container = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.ContainerName);
        var createResponse = await container.CreateIfNotExistsAsync();
        if (createResponse != null && createResponse.GetRawResponse().Status == 201)
            await container.SetAccessPolicyAsync(PublicAccessType.Blob);

        return container;
    }
}