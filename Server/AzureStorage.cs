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
    Task<string> UploadAsync(BlobFileName blobFileName, string fileBase64,
        CancellationToken cancellationToken = default);

    Task<bool> RemoveAsync(BlobFileName blobFileName, CancellationToken cancellationToken = default);
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

    public async Task<string> UploadAsync(BlobFileName blobFileName, string fileBase64,
        CancellationToken cancellationToken = default)
    {
        var container = await GetContainerAsync(cancellationToken);
        var blob = container.GetBlobClient(blobFileName.ToString());
        
        await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: cancellationToken);
        
        var fileBinary = Convert.FromBase64String(fileBase64);
        await using (var fileStream = await new StreamContent(new MemoryStream(fileBinary)).ReadAsStreamAsync(cancellationToken))
        {
            new FileExtensionContentTypeProvider().TryGetContentType(blobFileName.ToString(), out var contentType);
            await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: cancellationToken);
        }

        return blob.Uri.ToString();
    }

    public async Task<bool> RemoveAsync(BlobFileName blobFileName, CancellationToken cancellationToken = default)
    {
        var container = await GetContainerAsync(cancellationToken);
        var blob = container.GetBlobClient(blobFileName.ToString());

        return await blob.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    private async Task<BlobContainerClient> GetContainerAsync(CancellationToken cancellationToken = default)
    {
        var container = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.ContainerName);
        var createResponse = await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        if (createResponse != null && createResponse.GetRawResponse().Status == 201)
            await container.SetAccessPolicyAsync(PublicAccessType.Blob, cancellationToken: cancellationToken);

        return container;
    }
}

public class BlobFileName
{
    private readonly Guid _blobId;
    private readonly string _fileName;

    public BlobFileName(Guid blobId, string fileName)
    {
        _blobId = blobId;
        _fileName = fileName;
    }

    public override string ToString() => $"{_blobId}{Path.GetExtension(_fileName)}";
}