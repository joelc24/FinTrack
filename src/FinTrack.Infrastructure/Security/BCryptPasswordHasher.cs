using FinTrack.Application.Common.Interfaces;

namespace FinTrack.Infrastructure.Security;

public class BCryptPasswordHasher : IPasswordHasher
{
    private const int workFactor = 12;
    public string Hash(string password)
    { 
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: workFactor);
    }

    public bool Verify(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}