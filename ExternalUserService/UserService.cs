using ExternalUserService.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace ExternalUserService
{
    public class UserService
    {
        private readonly IAPIClient _client;
        private readonly Microsoft.Extensions.Caching.Memory.IMemoryCache _cache;

        public UserService(IAPIClient aPIClient, Microsoft.Extensions.Caching.Memory.IMemoryCache cache)
        {
            _client = aPIClient;
            _cache = cache;
        }

        public async Task<User> GetUserByIdAsync(int userid)
        {
            var key = userid.ToString();
            if(_cache.TryGetValue(key, out var user))
            {
                return user as User;
            }

            var userInfo = await _client.GetUserByIdAsync(userid);

            var entry = _cache.CreateEntry(key);
            entry.Value = userInfo;


            return userInfo;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {

            var key = "AllUsers";


            if (_cache.TryGetValue(key, out var users))
            {
                return users as IEnumerable<User>;
            }

            var user = new List<User>();
            int page = 1;
            var pagedResponse = await _client.GetAllUserByPageAsync(page);
            
            while(page <= pagedResponse.total_pages)
            {
                pagedResponse = await _client.GetAllUserByPageAsync(page);
                user.AddRange(pagedResponse.data.ToList());
                page++;
            }

            var entry = _cache.CreateEntry(key);
            entry.Value = user;

            return user;
        }
    }

}
