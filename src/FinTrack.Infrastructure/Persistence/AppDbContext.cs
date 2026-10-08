using FinTrack.Domain.Accounts;
using FinTrack.Domain.Common;
using FinTrack.Domain.ExpenseCategories;
using FinTrack.Domain.Expenses;
using FinTrack.Domain.Incomes;
using FinTrack.Domain.Profiles;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    private readonly TimeProvider _timeProvider;
    public AppDbContext(DbContextOptions<AppDbContext> options, TimeProvider timeProvider) : base(options)
    {
        _timeProvider = timeProvider;
    }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Profile> Profiles => Set<Profile>();
    public DbSet<Income> Incomes => Set<Income>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<ExpenseAllocation> ExpenseAllocations => Set<ExpenseAllocation>();
    public DbSet<ExpenseCategory> ExpenseCategories => Set<ExpenseCategory>();
    public DbSet<ExpenseSubCategory> ExpenseSubCategories => Set<ExpenseSubCategory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        var now = _timeProvider.GetUtcNow().UtcDateTime;
        foreach(var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if(entry.State == EntityState.Added)
                entry.Entity.CreatedAtUtc = now;
            if(entry.State == EntityState.Modified)
                entry.Entity.UpdatedAtUtc = now;
        }
        return base.SaveChangesAsync(cancellationToken);
    }

}