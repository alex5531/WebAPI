using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVCApp.Models;
using System.Net;
using MVCApp.Contracts;
using Microsoft.AspNetCore.Http;

namespace MVCApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostRepository _postRepo;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public HomeController(IMapper mapper, IPostRepository postRepo, IUserService userService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _postRepo = postRepo ?? throw new ArgumentNullException(nameof(postRepo));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _postRepo.GetAllAsync();
            return View(posts);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(UserView user)
        {
            if(!ModelState.IsValid)
            {
                return View(user);
            }
            var error = await _userService.SignUpAsync(user);
            if(!string.IsNullOrEmpty(error))
            {
                ModelState.AddModelError(string.Empty, error);
                return View(user);
            }
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if(!ModelState.IsValid)
            {
                return View(user);
            }

            var res = await _userService.GenerateToken(user);
            var err = res.Item1;
            var token = res.Item2;

            if(!string.IsNullOrEmpty(err))
            {
                ModelState.AddModelError(string.Empty, err);
                return View(user);
            }

            HttpContext.Session.SetString(Startup.JWT_COOKIE_NAME, token);
            //Response.Cookies.Append(JWT_COOKIE_NAME, token);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            //Response.Cookies.Delete(JWT_COOKIE_NAME);
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> DeletePost(int id)
        {
            //var token = Request.Cookies[Startup.JWT_COOKIE_NAME];
            //HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues token);
            var token = HttpContext.Session.GetString(Startup.JWT_COOKIE_NAME);
            var result = await _postRepo.DeletePost(id, token);
            if(result.StatusCode != HttpStatusCode.OK)
            {
                return View();
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult AddPost()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(PostViewModel postView)
        {
            if (!ModelState.IsValid)
            {
                return View(postView);
            }

            //var token = Request.Cookies[Startup.JWT_COOKIE_NAME];
            //HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues token);
            var token = HttpContext.Session.GetString(Startup.JWT_COOKIE_NAME);
            var result = await _postRepo.AddPost(_mapper.Map<Post>(postView), token);
            if (result.StatusCode != HttpStatusCode.Created)
            {
                return View(postView);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditPost(int Id)
        {
            var post = await _postRepo.GetPost(Id);
            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(Post post)
        {
            if (!ModelState.IsValid)
            {
                return View(post);
            }

            //var token = HttpContext.Request.Cookies[JWT_COOKIE_NAME];
            //HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues token);
            var token = HttpContext.Session.GetString(Startup.JWT_COOKIE_NAME);
            var result = await _postRepo.EditPost(post, token);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                return View(post);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}