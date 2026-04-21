using OnlineStore.Server.Entities;
using OnlineStore.Server.Features.Accounts.Repositories;
using OnlineStore.Server.Features.Accounts.Services;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Server.Services.Exceptions;
using OnlineStore.Shared.Accounts;

namespace OnlineStore.Server.Features.Accounts.RequestPartialPasswordFeature;

public class RequestPartialPasswordQueryHandler : IQueryHandler<RequestPartialPassword, PartialPasswordResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPartialPasswordChallengeStore _challengeStore;

    public RequestPartialPasswordQueryHandler(IUserRepository userRepository, IPartialPasswordChallengeStore challengeStore)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _challengeStore = challengeStore ?? throw new ArgumentNullException(nameof(challengeStore));
    }

    public async Task<PartialPasswordResponse> Handle(RequestPartialPassword request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserByEmail(request.Email);
        if (user == null || !user.PartialPasswords.Any())
        {
            // Dummy response to avoid user enumeration
            return new PartialPasswordResponse { StartPosition = 0, Length = 6, ChallengeToken = Guid.NewGuid().ToString("N") };
        }

        var random = new Random();
        var selected = user.PartialPasswords.ElementAt(random.Next(user.PartialPasswords.Count));

        var token = _challengeStore.CreateChallenge(user.Id, selected.Id);

        return new PartialPasswordResponse
        {
            StartPosition = selected.StartPosition,
            Length = selected.Length,
            ChallengeToken = token
        };
    }
}
