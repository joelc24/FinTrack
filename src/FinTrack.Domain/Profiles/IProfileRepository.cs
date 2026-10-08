namespace FinTrack.Domain.Profiles;

public interface IProfileRepository
{
    Task<Profile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Profile>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default);

    void Add(Profile profile);
    void Remove(Profile profile);
}