using Microsoft.AspNetCore.Components.Forms;
using OnlineStore.Client.Services;
using OnlineStore.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Client.Products.Models;

public class CreateProductModel : ProductModel<CreateProductFileModel>
{
    public IBrowserFile ThumbnailFile { get; set; }
}
