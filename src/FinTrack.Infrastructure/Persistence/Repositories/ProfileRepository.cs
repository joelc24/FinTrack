using FinTrack.Domain.Profiles;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence.Repositories;

public sealed class ProfileRepository : IProfileRepository
{
    private readonly AppDbContext _dbContext;

    public ProfileRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Profile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _dbContext.Profiles
            .FirstOrDefaultAsync(profile => profile.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Profile>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default) =>
        await _dbContext.Profiles
            .Where(profile => profile.AccountId == accountId)
            .ToListAsync(cancellationToken);

    public void Add(Profile profile) => _dbContext.Profiles.Add(profile);

    public void Remove(Profile profile) => _dbContext.Profiles.Remove(profile);
}