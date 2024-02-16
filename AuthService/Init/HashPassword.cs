using AuthService.Domain;
using Microsoft.AspNetCore.Identity;
using System;

namespace AuthService.Init;

public class HashPassword
{
    private readonly PasswordHasher<InsuranceAgent> passwordHasher;
    public HashPassword(PasswordHasher<InsuranceAgent> passwordHasher)
    {
        this.passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    }
    public string Hash(InsuranceAgent agent)
    {
        return passwordHasher.HashPassword(agent, agent.Password);
    }

    public bool Verify(InsuranceAgent agent, string providedPassword)
    {
        var result = passwordHasher.VerifyHashedPassword(agent, agent.Password, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}
