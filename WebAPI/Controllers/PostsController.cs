using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Swagger;
using WebAPI.BLL.Contracts;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;

namespace WebAPI.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPostsService _postsService;
        private readonly IUsersService _usersService;

        /// <summary>
        /// Posts db api
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="postsService"></param>
        public PostsController(IMapper mapper, IPostsService postsService, IUsersService usersService, IJwtTokenService jwtTokenService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _postsService = postsService ?? throw new ArgumentNullException(nameof(postsService));
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        /// <summary>
        /// Create a new post
        /// </summary>
        /// <param name="post"></param>
        /// <returns>Returns created post</returns>           
        [HttpPost("")]
        [SwaggerResponseExample((int)HttpStatusCode.Created, typeof(PostModelExample))]
        [SwaggerResponse((int)HttpStatusCode.Created, Type = typeof(Post), Description = "Returns created post")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<IActionResult> CreatePostAsync([FromBody] Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var userId = GetCurrentUserId(HttpContext);
            if (userId == null)
            {
                return BadRequest();
            }
            post.UserId = Convert.ToInt32(userId);
            var result = await _postsService.CreatePostAsync(_mapper.Map<BLL.Models.Post>(post));
            
            //return Created($"{result.Id}", _mapper.Map<Post>(result));
            return CreatedAtAction(
                actionName: "GetPost",
                routeValues: new
                {
                    version = HttpContext?.GetRequestedApiVersion().ToString(),
                    result.Id
                },
                value: _mapper.Map<Post>(result));
        }

        /// <summary>
        /// Get post by id
        /// </summary>
        /// <param name="id">Post Id</param>
        /// <returns>Returns post</returns>
        [HttpGet("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Post), Description = "Returns post")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(PostModelExample))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid post id")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            var result = await _postsService.GetPostAsync(id);
            if (result == null)
            {
                return NotFound(new { id });
            }

            return Ok(_mapper.Map<Post>(result));
        }

        /// <summary>
        /// Update existing post
        /// </summary>
        /// <param name="id">post id</param>
        /// <param name="post">post parameters</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [SwaggerRequestExample(typeof(Post), typeof(PostModelExample))]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid post object")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<IActionResult> UpdatePostAsync([FromRoute] int id, [FromBody] Post post)
        {
            if (id < 1 || !ModelState.IsValid)
            {
                return BadRequest();
            }

            post.Id = id;

            var userId = GetCurrentUserId(HttpContext);
            if (userId == null)
            {
                return BadRequest();
            }

            var result = await _postsService.UpdatePostAsync(_mapper.Map<BLL.Models.Post>(post), (int)userId);
            if (result < 1)
                return NotFound();
            else
                return Ok();
        }

        /// <summary>
        /// Delete post
        /// </summary>
        /// <param name="id">Post id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid post id")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<IActionResult> DeletePostAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            var userId = GetCurrentUserId(HttpContext);
            if(userId == null)
            {
                return BadRequest();
            }

            var result = await _postsService.DeletePostAsync(id, (int)userId);
            if (result < 1)
                return NotFound();
            else
                return Ok();
        }

        /// <summary>
        /// Get posts list
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns></returns>
        [HttpGet("")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Post>), Description = "Returns posts list")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid pageNumber or pageSize")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostsListAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
        {
            if (pageNumber == 0 || pageSize == 0)
            {
                return BadRequest();
            }

            var posts = await _postsService.GetPostsListAsync(pageNumber, pageSize);
            var users = await _usersService.GetUsersListAsync(1, int.MaxValue);

            var result = posts.Join(users, 
                post => post.UserId, user => user.Id, 
                (post, user) => 
                new PostViewModel() {
                    Id = post.Id, 
                    UserId = post.UserId, 
                    FirstName = user.FirstName, 
                    LastName = user.LastName, 
                    Body = post.Body, 
                    Title = post.Title }
                );

            return Ok(result);
        }

        private static int? GetCurrentUserId(Microsoft.AspNetCore.Http.HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;
            var userId = identity?.FindFirst(ClaimTypes.SerialNumber)?.Value;
            if (userId == null)
            {
                return null;
            }
            return Convert.ToInt32(userId);
        }
    }
}