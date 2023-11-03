namespace OnlineStore.Server.Entities;

public class EmailTemplate
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string HtmlContent { get; set; } = string.Empty;
}