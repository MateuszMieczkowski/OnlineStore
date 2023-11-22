namespace OnlineStore.Server.Options;

public class BlobStorageOptions
{
    public string ConnectionString { get; set; } = default!;

    public string ContainerName { get; set; } = default!;
}