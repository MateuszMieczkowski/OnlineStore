using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using OnlineStore.Server.Entities;
using OnlineStore.Shared.Models;

namespace OnlineStore.Server;

public interface IBlobStorage
{
    Task<string> Upload(IFormCollection formCollection);
    Task<bool> Upload(IEnumerable<CreateProductDto> dtos, IEnumerable<Product> products);
    Task<bool> Update(int id, UpdateProductDto dto);
    Task<bool> Remove(int id);
}

public class AzureStorage : IBlobStorage
{
    private readonly ILogger<AzureStorage> _logger;
    private readonly string _storageConnectionString;
    private readonly string _storageContainerName;

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

        var contentType = "image/png";

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
        if (string.IsNullOrEmpty(dto.ThumbnailPath)) return false;
        var container = await GetContainter();

        var contentType = "image/png";


        var blob = container.GetBlobClient(id.ToString());
        await blob.DeleteIfExistsAsync();
        var bytes = Convert.FromBase64String(dto.ThumbnailPath);
        using (var fileStream = new StreamContent(new MemoryStream(bytes)).ReadAsStream())
        {
            await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType });
        }

        return true;
    }

    public async Task<bool> Remove(int id)
    {
        var container = await GetContainter();

        var blob = container.GetBlobClient(id.ToString());
        await blob.DeleteIfExistsAsync();
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
}