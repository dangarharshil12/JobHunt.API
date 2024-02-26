using JobHunt.Services.AuthAPI.Models;

namespace JobHunt.Services.AuthAPI.Repositories.IRepositories
{
    public interface ITokenRepository
    {
        string CreateJwtToken(ApplicationUser user, List<string> roles);
    }
}
