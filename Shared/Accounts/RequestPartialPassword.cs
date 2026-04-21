using MediatR;
using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Accounts;

public class RequestPartialPassword : IQuery<PartialPasswordResponse>
{
    public string Email { get; set; } = string.Empty;
}

public class PartialPasswordResponse
{
    public string Fragment { get; set; } = string.Empty;
    public int StartPosition { get; set; }
    public int Length { get; set; }
}
