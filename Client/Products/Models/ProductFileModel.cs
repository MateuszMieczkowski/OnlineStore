using Microsoft.AspNetCore.Components.Forms;
using OnlineStore.Shared.Products;

namespace OnlineStore.Client.Products.Models;

public abstract class ProductFileModel
{
    public string FileName { get; set; }

    public IBrowserFile File { get; set; }
    public string? FileBase64 { get; set; }

    public virtual string ImageSource => $"data:image/jpeg;base64,{FileBase64}";
    
    public ProductFileTypeDto ProductFileType { get; set; }

    public string? Description { get; set; }
}
