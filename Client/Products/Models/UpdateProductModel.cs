namespace OnlineStore.Client.Products.Models;

public class UpdateProductModel : ProductModel<UpdateProductFileModel>
{
    public int Id { get; set; }
}

public class UpdateProductFileModel : ProductFileModel
{
    public int? Id { get; set; }
    
    public string? ThumbnailBlobUri { get; set; }

    public override string ImageSource => ThumbnailBlobUri ?? base.ImageSource;
}