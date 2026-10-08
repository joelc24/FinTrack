using System.Text.RegularExpressions;
using FinTrack.Domain.Common;

namespace FinTrack.Domain.Accounts;

public sealed partial class Account : BaseEntity
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();

    private Account() {}

    public static Result<Account> Create(string email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result.Failure<Account>(AccountErrors.EmptyEmail);

        if (!EmailRegex().IsMatch(email))
            return Result.Failure<Account>(AccountErrors.InvalidEmail);

        if (string.IsNullOrWhiteSpace(passwordHash))
            return Result.Failure<Account>(AccountErrors.EmptyPassword);
    
        var account = new Account
        {
            Id = Guid.CreateVersion7(),
            Email = email.Trim().ToLowerInvariant(),
            PasswordHash = passwordHash
        };

        return Result.Success(account);
    }
}