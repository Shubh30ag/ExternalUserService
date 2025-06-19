using AutoFixture.Xunit2;
using ExternalUserService;
using ExternalUserService.Models;
using Moq;

namespace TestProject1
{
    public class ExternalUserServiceTest
    {
        private readonly Mock<IAPIClient> apiClient;
        public ExternalUserServiceTest()
        {
            apiClient = new Mock<IAPIClient>();
        }

        [Theory, AutoData]
        public async  void GetUserByIdAsync(User user, int userid)
        {
            //Arrange
            apiClient.Setup(m => m.GetUserByIdAsync(userid)).ReturnsAsync(user);

            //Act
            var userService = new UserService(apiClient.Object);

            var result = await userService.GetUserByIdAsync(userid);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.last_name, user.last_name);
            Assert.Equal(result.first_name, user.first_name);
            Assert.Equal(result.avatar, user.avatar);
            Assert.Equal(result.email, user.email);
        }

        [Theory, AutoData]
        public async void GetAllUsersAsync(User user, int userid, PagedResponse pagedResponse)
        {
            //Arrange
            int page = 1;
            pagedResponse.total_pages = 2;
            apiClient.Setup(m => m.GetAllUserByPageAsync(It.IsAny<int>())).ReturnsAsync(pagedResponse);

            //Act
            var userService = new UserService(apiClient.Object);

            var result = await userService.GetAllUsersAsync();

            //Assert
            Assert.NotNull(result);
        }
    }
}