using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Client.Models.Accounts;

public class UpsertAddressModel
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Ulica jest wymagana.")]
    public string Street { get; set; } = string.Empty;

    [Required(ErrorMessage = "Numer ulicy jest wymagany.")]
    public string StreetNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Miasto jest wymagane.")]
    public string City { get; set; } = string.Empty;

    public string? State { get; set; }

    [Required(ErrorMessage = "Kod pocztowy jest wymagany.")]
    public string PostalCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kraj jest wymagany.")]
    public string Country { get; set; } = string.Empty;
}

