using FinTrack.Domain.Accounts;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence.Repositories;

public sealed class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _dbContext;

    public AccountRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _dbContext.Accounts
            .FirstOrDefaultAsync(account => account.Id == id, cancellationToken);

    public async Task<Account?> GetByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        await _dbContext.Accounts
            .FirstOrDefaultAsync(account => account.Email == email.Trim().ToLower(), cancellationToken);

    public void Add(Account account) => _dbContext.Accounts.Add(account);
}