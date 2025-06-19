using ExternalUserService.Models;

namespace ExternalUserService
{
    public interface IAPIClient
    {
        Task<User> GetUserByIdAsync(int userid);
        Task<PagedResponse> GetAllUserByPageAsync(int page);
    }
}
