using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using WebAPI.BLL;
using WebAPI.BLL.Contracts;
using WebAPI.BLL.Mappings;
using WebAPI.BLL.Models;
using WebAPI.Controllers;
using WebAPI.DAL.MSSQL;
using WebAPI.Mapping;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using WebAPI.BLL.PasswordSecurity;

namespace WebAPITest
{
    public class UsersControllerTest
    {
        private UsersController _usersController;
        public UsersControllerTest()
        {
            IMapper _mapper = new Mapper(
                new MapperConfiguration(
                    cfg =>
                    {
                        cfg.AddProfile<UserMapping>();
                        cfg.AddProfile<UsersViewMapping>();
                    }
                    )
                );
            IJwtTokenService _jwtTokenService = new JwtTokenService(
                new TestOptionsMonitor<UsersBLLOptions>(
                    new UsersBLLOptions()
                    {
                        JwtSecretKey = string.Empty
                    })
                );

            IUsersService _usersService = new UsersService(_mapper, new UsersRepositoryFake(null), new PasswordStorage());

            _usersController = new UsersController(_mapper, _usersService, _jwtTokenService);
        }

        [Fact]
        public void GetById_WhenCalled_ReturnsOk()
        {
            var id = UsersRepositoryFake._userEntities.First().Id;
            //Act
            var okResult = _usersController.GetUserAsync(id);
            //Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
            var okObjectResult = okResult.Result as OkObjectResult;
            Assert.IsType<WebAPI.Models.User>(okObjectResult.Value);
            Assert.Equal(id, (okObjectResult.Value as WebAPI.Models.User).Id);
        }

        [Fact]
        public void GetById_WhenCalled_ReturnsNotFound()
        {
            //Act
            var notFound = _usersController.GetUserAsync(int.MaxValue);
            //Assert
            Assert.IsType<NotFoundObjectResult>(notFound.Result);
        }

        [Fact]
        public void GetById_WhenCalled_ReturnsBadRequestResult()
        {
            //Act
            var badRequest = _usersController.GetUserAsync(0);
            //Assert
            Assert.IsType<BadRequestResult>(badRequest.Result);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okResult = _usersController.GetUsersListAsync().Result as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<WebAPI.Models.User>>(okResult.Value);
            Assert.Equal(UsersRepositoryFake._userEntities.Count, items.Count);
        }

        [Fact]
        public void Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            var nameMissingItem = new WebAPI.Models.User()
            {
                FirstName = string.Empty,
                LastName = "LastName",
                Email = "test@test.com",
                DoB = new DateTime(2001, 01, 01),
                Password = "Password"
            };
            _usersController.ModelState.AddModelError("FirstName", "Required");

            //Act
            var badResponse = _usersController.CreateUserAsync(nameMissingItem).Result;

            //Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        [Fact]
        public void Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            var newUser = new WebAPI.Models.User()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "test@test.com",
                DoB = new DateTime(2001, 01, 01),
                Password = "Password"
            };

            // Act
            var createdResponse = _usersController.CreateUserAsync(newUser).Result;

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse);

            var item = ((CreatedAtActionResult)createdResponse).Value as WebAPI.Models.User;
            // Assert
            Assert.IsType<WebAPI.Models.User>(item);
            Assert.Equal(newUser.FirstName, item.FirstName);
        }

        [Fact]
        public void Remove_NotExistingIdPassed_ReturnsNotFoundResponse()
        {
            // Arrange
            var notExistingId = int.MaxValue;

            // Act
            var badResponse = _usersController.DeleteUserAsync(notExistingId).Result;

            // Assert
            Assert.IsType<NotFoundResult>(badResponse);
        }

        [Fact]
        public void Remove_ExistingIdPassed_ReturnsOkResult()
        {
            // Arrange
            var existingId = 1;

            // Act
            var okResponse = _usersController.DeleteUserAsync(existingId).Result;

            // Assert
            Assert.IsType<OkResult>(okResponse);
        }

        [Fact]
        public void Remove_ExistingIdPassed_RemovesOneItem()
        {
            var user = UsersRepositoryFake._userEntities.First();
            var cnt = UsersRepositoryFake._userEntities.Count;
            // Arrange
            var existingId = user.Id;

            try
            {
                // Act
                var okResponse = _usersController.DeleteUserAsync(existingId);

                // Assert
                Assert.Equal(cnt - 1, UsersRepositoryFake._userEntities.Count);
            }
            finally
            {
                UsersRepositoryFake._userEntities.Insert(0, user);
            }
        }
    }
}
