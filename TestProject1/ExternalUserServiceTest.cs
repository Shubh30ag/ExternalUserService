using AutoFixture.Xunit2;
using ExternalUserService;
using ExternalUserService.Models;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace TestProject1
{
    public class ExternalUserServiceTest
    {
        private readonly Mock<IAPIClient> apiClient;
        private readonly Mock<Microsoft.Extensions.Caching.Memory.IMemoryCache> cache;

        public ExternalUserServiceTest()
        {
            apiClient = new Mock<IAPIClient>();
            cache = new Mock<Microsoft.Extensions.Caching.Memory.IMemoryCache>();
        }

        [Theory, AutoData]
        public async  void GetUserByIdAsync(User user, int userid, Object obj)
        {
            //Arrange
            apiClient.Setup(m => m.GetUserByIdAsync(userid)).ReturnsAsync(user);
            cache.Setup(m => m.TryGetValue(It.IsAny<object>(), out obj)).Returns(null);
            
            var mockEntry = new Mock<ICacheEntry>();
            cache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(mockEntry.Object);



            //Act
            var userService = new UserService(apiClient.Object, cache.Object);

            var result = await userService.GetUserByIdAsync(userid);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.last_name, user.last_name);
            Assert.Equal(result.first_name, user.first_name);
            Assert.Equal(result.avatar, user.avatar);
            Assert.Equal(result.email, user.email);
        }

        [Theory, AutoData]
        public async void GetAllUsersAsync(PagedResponse pagedResponse, Object obj)
        {
            //Arrange
            int page = 1;
            pagedResponse.total_pages = 2;
            var users = new List<User>();
            apiClient.Setup(m => m.GetAllUserByPageAsync(It.IsAny<int>())).ReturnsAsync(pagedResponse);

            var mockEntry = new Mock<ICacheEntry>();
            cache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(mockEntry.Object);


            //Act
            var userService = new UserService(apiClient.Object, cache.Object);

            var result = await userService.GetAllUsersAsync();

            //Assert
            Assert.NotNull(result);
        }
    }
}