using CareerAnalytics.Domain.Users;

namespace CareerAnalytics.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
