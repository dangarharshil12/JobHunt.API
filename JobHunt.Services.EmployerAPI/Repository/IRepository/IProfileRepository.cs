using JobHunt.Services.EmployerAPI.Models.Dto;

namespace JobHunt.Services.EmployerAPI.Repository.IRepository
{
    public interface IProfileRepository
    {
        Task<List<UserDto>> GetUsers(List<Guid> users);
    }
}
