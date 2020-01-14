using System;
using Swashbuckle.AspNetCore.Filters;
using WebAPI.Models;

namespace WebAPI.Swagger
{
    public class UserModelExample : IExamplesProvider<User>
    {
        public User GetExamples()
        {
            return new User
            {
                Id = 1,
                FirstName = "Joe",
                LastName = "Doe",
                Email = "a@a.com",
                Password = "1234567!",
                DoB = new DateTime(2001, 1, 1)
            };
        }
    }
    public class PostModelExample : IExamplesProvider<Post>
    {
        public Post GetExamples()
        {
            return new Post
            {
                Id = 1,
                UserId = 1,
                Title = "Title",
                Body = "Body"
            };
        }
    }
}
