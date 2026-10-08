using FinTrack.Domain.Accounts;

namespace FinTrack.Domain.UnitTests.Accounts;

public class AccountTests
{
    //=======================================
    // ERROR SCENARIOS (VALIDATION RULES)
    //=======================================

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_EmailIsEmptyOrWhiteSpaces_ReturnEmailFailure(string? InvalidEmail)
    {
        var passwordHash = "hashed_secure_password";

        var result = Account.Create(InvalidEmail!, passwordHash);

        Assert.True(result.IsFailure);
        Assert.Equal(AccountErrors.EmptyEmail, result.Error);

    }

    [Theory]
    [InlineData("usuario-sin-arroba.com")]
    [InlineData("usuario@")]
    [InlineData("usuario@com.")]
    [InlineData("usuario @dominio.com")]
    public void Create_EmialIsFormatInvalid_ReturnsInvalidEmailFailure(string email)
    {
        var passwordHash = "hashed_secure_password";


        var result = Account.Create(email, passwordHash);


        Assert.True(result.IsFailure);
        Assert.Equal(AccountErrors.InvalidEmail, result.Error);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("      ")]
    public void Create_PasswordHashIsEmptyOrWhiteSpace_ReturnEmptyPasswordFailure(string? invalidPassowrdHash)
    {
        // Arrange
        var validEmail = "usuario@fintrack.com";


        // Act
        var result = Account.Create(validEmail, invalidPassowrdHash!);


        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AccountErrors.EmptyPassword, result.Error);
    }



    //==========================================
    // HAPPY PATH NO (VALIDATION ERRORS)
    //==========================================
    [Fact]
    public void Create_ValidParameters_ReturnsPopulatedAccount()
    {
        // Arrange
        var email = "test.user@fintrack.com";
        var passwordHash = "secure_password_hash123";


        // Act
        var result = Account.Create(email, passwordHash);


        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
        Assert.Equal(email, result.Value.Email);
        Assert.Equal(passwordHash, result.Value.PasswordHash);
    }

    [Fact]
    public void Create_ValidEmailWithSpacesAndUppercase_NormalizesEmailToTrailingAndLowercase()
    {
        // Arrange
        var rawEmail = "User.NAME@FinTrack.COM";
        var expectedNormalizedEmail = "user.name@fintrack.com";
        var passwordHash = "secure_hash_123";

        // Act
        var result = Account.Create(rawEmail, passwordHash);

        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedNormalizedEmail, result.Value.Email);
    }

}
