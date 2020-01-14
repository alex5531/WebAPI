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

namespace WebAPI.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUsersService _usersService;
        private readonly IJwtTokenService _jwtTokenService;

        /// <summary>
        /// Users db api
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="usersService"></param>
        /// <param name="jwtTokenService"></param>
        public UsersController(IMapper mapper, IUsersService usersService, IJwtTokenService jwtTokenService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns created user</returns>           
        [HttpPost("")]
        [SwaggerResponseExample((int)HttpStatusCode.Created, typeof(UserModelExample))]
        [SwaggerResponse((int)HttpStatusCode.Created, Type = typeof(User), Description = "Returns created user")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUserAsync([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(user);
            }
            var existingUser = await GetUserByEmailAsync(user.Email);
            if (!(existingUser is NotFoundObjectResult))
            {
                return BadRequest($"User with email {user.Email} already registered");
            }
            var result = await _usersService.CreateUserAsync(_mapper.Map<BLL.Models.User>(user));

            return CreatedAtAction(
                actionName : "GetUser", 
                routeValues: new {
                    version = HttpContext?.GetRequestedApiVersion().ToString(),
                    result.Id }, 
                value: _mapper.Map<User>(result));
        }

        private async Task<IActionResult> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email) || email.Length < 6)
            {
                return BadRequest();
            }

            var result = await _usersService.GetUserAsync(email);
            if (result == null)
            {
                return NotFound(new { email });
            }

            return Ok(_mapper.Map<User>(result));
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>Returns user</returns>
        [HttpGet("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(User), Description = "Returns user")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(UserModelExample))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid user id")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<IActionResult> GetUserAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            var result = await _usersService.GetUserAsync(id);
            if (result == null)
            {
                return NotFound(new { id });
            }
            return Ok(_mapper.Map<User>(result));
        }

        /// <summary>
        /// Update existing user
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="user">User parameters</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [SwaggerRequestExample(typeof(User), typeof(UserModelExample))]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid user object")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<IActionResult> UpdateUserAsync([FromRoute] int id, [FromBody] User user)
        {
            if (id < 1 || !ModelState.IsValid)
            {
                return BadRequest();
            }

            user.Id = id;
            await _usersService.UpdateUserAsync(_mapper.Map<BLL.Models.User>(user));
            return Ok();
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid user id")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            
            var result = await _usersService.DeleteUserAsync(id);
            if (result)
                return Ok();
            else
                return NotFound();
        }

        /// <summary>
        /// Get users list
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns></returns>
        [HttpGet("")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<User>), Description = "Returns users list")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid pageNumber or pageSize")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<IActionResult> GetUsersListAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
        {
            if (pageNumber == 0 || pageSize == 0)
            {
                return BadRequest();
            }

            var result = await _usersService.GetUsersListAsync(pageNumber, pageSize);
            return Ok(_mapper.Map<IEnumerable<User>>(result));
        }

        [HttpPost("GenerateToken")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Generated token")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateTokenAsync([FromBody] LoginViewModel user)
        {
            var userId = await _usersService.ValidateUserAsync(user.Email, user.Password); 
            if (userId > 0)
            {
                return Ok(_jwtTokenService.GenerateToken(userId, 60));
            }
            return BadRequest("Could not find user with these email/password");
        }
    }
}