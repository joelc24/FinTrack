using FinTrack.Domain.Common;

namespace FinTrack.Domain.Profiles;

public sealed class Profile : BaseEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string UserName { get; private set; } = string.Empty;
    public string? ImageUrl { get; private set; }
    public bool IsAdmin { get; private set; }
    public string? PasswordHash { get; private set; }
    public bool IsProtected => PasswordHash is not null;
    public Guid AccountId { get; private set; }

    private Profile() { }

    public static Result<Profile> Create(
        string name, string lastName, string userName, Guid accountId,
        bool isAdmin = false, string? imageUrl = null, string? passwordHash = null)
    {
        var nameValidation = ValidateName(name);
        if (nameValidation.IsFailure) return Result.Failure<Profile>(nameValidation.Error);

        var lastNameValidation = ValidateLastName(lastName);
        if (lastNameValidation.IsFailure) return Result.Failure<Profile>(lastNameValidation.Error);

        if (string.IsNullOrWhiteSpace(userName))
            return Result.Failure<Profile>(ProfileErrors.EmptyUserName);

        if (accountId == Guid.Empty)
            return Result.Failure<Profile>(ProfileErrors.InvalidAccount);

        return Result.Success(new Profile
        {
            Id = Guid.CreateVersion7(),
            Name = name.Trim(),
            LastName = lastName.Trim(),
            UserName = userName.Trim(),
            ImageUrl = imageUrl,
            IsAdmin = isAdmin,
            PasswordHash = passwordHash,
            AccountId = accountId
        });
    }

    public Result UpdateImageUrl(string? imageUrl)
    {
        ImageUrl = imageUrl;
        return Result.Success();
    }

    public Result UpdateName(string name)
    {
        var validation = ValidateName(name);
        if (validation.IsFailure) return validation;

        Name = name.Trim();
        return Result.Success();
    }

    public Result UpdateLastName(string lastName)
    {
        var validation = ValidateLastName(lastName);
        if (validation.IsFailure) return validation;

        LastName = lastName.Trim();
        return Result.Success();
    }


    public Result ActivateProtection(string passwordHash)
    {
        if (IsProtected)
            return Result.Failure(ProfileErrors.ProfileProtected);

        if (string.IsNullOrWhiteSpace(passwordHash))
            return Result.Failure(ProfileErrors.PasswordInvalid);

        PasswordHash = passwordHash;
        return Result.Success();
    }

    public Result DeactivateProtection()
    {
        if (!IsProtected)
            return Result.Failure(ProfileErrors.ProfileUnprotected);

        PasswordHash = null;
        return Result.Success();
    }

    private static Result ValidateName(string name) =>
        string.IsNullOrWhiteSpace(name)
            ? Result.Failure(ProfileErrors.EmptyName)
            : Result.Success();

    private static Result ValidateLastName(string lastName) =>
        string.IsNullOrWhiteSpace(lastName)
            ? Result.Failure(ProfileErrors.EmptyLastName)
            : Result.Success();
}