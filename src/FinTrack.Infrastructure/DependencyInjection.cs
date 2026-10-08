using FinTrack.Application.Common.Interfaces;
using FinTrack.Domain.Common;
using FinTrack.Domain.Accounts;
using FinTrack.Domain.ExpenseCategories;
using FinTrack.Domain.Expenses;
using FinTrack.Domain.Incomes;
using FinTrack.Domain.Profiles;
using FinTrack.Infrastructure.Persistence;
using FinTrack.Infrastructure.Persistence.Repositories;
using FinTrack.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinTrack.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("No se configuró la connection string 'DefaultConnection'.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString, sqlOptions => 
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 3)
            ));
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IIncomeRepository, IncomeRepository>();
        services.AddScoped<IExpenseRepository, ExpenseRepository>();
        services.AddScoped<IExpenseCategoryRepository, ExpenseCategoryRepository>();
        services.AddSingleton(TimeProvider.System);
        
        var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>() 
            ?? throw new InvalidOperationException("No se configuro la seccion de 'Jwt'");

        services.AddSingleton(jwtSettings);
        services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}