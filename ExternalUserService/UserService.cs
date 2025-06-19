using ExternalUserService.Models;
using Newtonsoft.Json;
using System.Net;

namespace ExternalUserService
{
    public class UserService
    {
        private readonly IAPIClient _client;

        public UserService(IAPIClient aPIClient)
        {
            _client = aPIClient;
        }

        public async Task<User> GetUserByIdAsync(int userid)
        {
            var userInfo = await _client.GetUserByIdAsync(userid);
            return userInfo;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = new List<User>();
            int page = 1;
            var pagedResponse = await _client.GetAllUserByPageAsync(page);
            
            while(page <= pagedResponse.total_pages)
            {
                pagedResponse = await _client.GetAllUserByPageAsync(page);
                users.AddRange(pagedResponse.data.ToList());
                page++;
            }

            return users;
        }
    }

}
