using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MVCApp.Contracts;
using MVCApp.Controllers;
using MVCApp.Mapping;
using MVCApp.Models;
using MVCApp.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MVCAppTests.Controllers
{
    public class HomeControllerTests
    {
        private readonly Mock<IPostRepository> _mockRepo;
        private readonly HomeControllerFake _controller;

        public HomeControllerTests()
        {
            _mockRepo = new Mock<IPostRepository>();
            var mapper = new Mapper(
                new MapperConfiguration(
                    cfg =>
                    {
                        cfg.AddProfile<PostViewMapping>();
                        cfg.AddProfile<UsersViewMapping>();
                    }
                    )
                );

            var usersService = new UserService(null);

            _controller = new HomeControllerFake(mapper, _mockRepo.Object, usersService);
        }

        [Fact]
        public void Index_ActionExecutes_ReturnsViewForIndex()
        {
            var result = _controller.Index();
            Assert.IsType<ViewResult>(result.Result);
        }
        
        [Fact]
        public void Index_ActionExecutes_ReturnsExactNumberOfEmployees()
        {
            _mockRepo.Setup(repo => repo.GetAllAsync())
                .Returns(Task.FromResult(new List<PostViewModel>() { new PostViewModel(), new PostViewModel() }));

            var result = _controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result.Result);
            var posts = Assert.IsType<List<PostViewModel>>(viewResult.Model);
            Assert.Equal(2, posts.Count);
        }

        [Fact]
        public void Create_ActionExecutes_ReturnsViewForCreate()
        {
            var result = _controller.AddPost();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_InvalidModelState_ReturnsView()
        {
            _controller.ModelState.AddModelError("UserID", "UserId is required");

            var post = new PostViewModel { Title = "Title", Body = "Body" };

            var result = _controller.AddPost(post);

            var viewResult = Assert.IsType<ViewResult>(result.Result);
            var testPost = Assert.IsType<PostViewModel>(viewResult.Model);
            Assert.Equal(post.Title, testPost.Title);
            Assert.Equal(post.Body, testPost.Body);
        }
        [Fact]
        public void Create_InvalidModelState_CreatePostNeverExecutes()
        {
            _controller.ModelState.AddModelError("UserID", "UserID is required");

            var post = new PostViewModel { Title = "Title" };

            _ = _controller.AddPost(post);

            _mockRepo.Verify(x => x.AddPost(It.IsAny<Post>(), string.Empty), Times.Never);
        }
    }
    internal class HomeControllerFake : HomeController
    {
        internal HomeControllerFake(IMapper mapper, IPostRepository postRepo, IUserService userService) : base(mapper, postRepo, userService) { }

        protected override string GetJWTCookie()
        {
            return string.Empty;
        }
    }
}
