namespace OnlineStore.Server.Features.Accounts.Services;

public interface IPartialPasswordChallengeStore
{
    string CreateChallenge(int userId, int partialPasswordId);
    (int UserId, int PartialPasswordId)? ConsumeChallenge(string token);
}

public class PartialPasswordChallengeStore : IPartialPasswordChallengeStore
{
    private readonly Dictionary<string, (int UserId, int PartialPasswordId, DateTime ExpiresAt)> _challenges = new();
    private readonly object _lock = new();
    private static readonly TimeSpan ChallengeTimeout = TimeSpan.FromMinutes(5);

    public string CreateChallenge(int userId, int partialPasswordId)
    {
        var token = Guid.NewGuid().ToString("N");
        lock (_lock)
        {
            // Cleanup expired
            var expired = _challenges.Where(x => x.Value.ExpiresAt < DateTime.UtcNow).Select(x => x.Key).ToList();
            foreach (var k in expired) _challenges.Remove(k);

            _challenges[token] = (userId, partialPasswordId, DateTime.UtcNow.Add(ChallengeTimeout));
        }
        return token;
    }

    public (int UserId, int PartialPasswordId)? ConsumeChallenge(string token)
    {
        lock (_lock)
        {
            if (!_challenges.TryGetValue(token, out var entry)) return null;
            _challenges.Remove(token);
            if (entry.ExpiresAt < DateTime.UtcNow) return null;
            return (entry.UserId, entry.PartialPasswordId);
        }
    }
}
