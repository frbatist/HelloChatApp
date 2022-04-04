using FluentAssertions;
using HelloChatApp.Server.Domain.Abstractions;
using HelloChatApp.Server.Models;
using HelloChatApp.Server.Services;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HelloChatApp.Tests.Services
{
    public class UserServiceTest
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public UserServiceTest()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _userService = new UserService(_userRepository);
        }

        [Fact]
        public async Task GetLoggedUserName_should_return_logged_in_user_name()
        {
            //Arrange
            var name = "Lourenço";
            var appUser = new ApplicationUser(name);
            HttpContext context = new DefaultHttpContext();
            var principal = Substitute.For<ClaimsPrincipal>();
            context.User = principal;
            _userRepository.FindByClaimsPrincipal(principal).Returns(appUser);

            //Act
            var loggedInUserName = await _userService.GetLoggedUserName(context);

            //Assert
            loggedInUserName.Should().Be(name);
        }

        [Fact]
        public async Task GetLoggedUserName_should_return_null_if_null_httpContext_is_provided()
        {
            //Act
            var loggedInUserName = await _userService.GetLoggedUserName(null);

            //Assert
            loggedInUserName.Should().BeNull();
        }

        [Fact]
        public async Task GetLoggedUserName_should_return_null_if_principal_provided_is_not_found_in_database()
        {
            //Arrange
            var name = "Lourenço";
            var appUser = new ApplicationUser(name);
            HttpContext context = new DefaultHttpContext();

            //Act
            var loggedInUserName = await _userService.GetLoggedUserName(context);

            //Assert
            loggedInUserName.Should().BeNull();
        }
    }
}
