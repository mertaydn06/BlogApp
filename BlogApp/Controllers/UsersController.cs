using System.Security.Claims;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)  // Register sayfasından gelen verileri VM eşliğinde burada karşıladık.
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName || x.Email == model.Email);

                if (user == null)  // Eğer mail ve username veritabanında yoksa çalışır.
                {
                    _userRepository.CreateUser(new User
                    {
                        UserName = model.UserName,
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password,
                        Image = "avatar.jpg"
                    });

                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Username ya da Email kullanımda.");
                }
            }
            return View(model);
        }


        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)  // Giriş yapan kullanıcının Login sayfasına tekrar gidememesi için Login actionuna geldiğimizde Login View'ına gitmesi yerine ana sayfaya yönlendirdik.
            {
                return RedirectToAction("Index", "Posts");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isUser = _userRepository.Users.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);  // Girilen mail ve şifre veritabanında kayıtlıysa isUser değişkenine atadık.

                if (isUser != null)  //
                {
                    var userClaims = new List<Claim>();  // "Claim", kullanıcı hakkında sistemde saklanan bir bilgidir.

                    userClaims.Add(new Claim(ClaimTypes.NameIdentifier, isUser.UserId.ToString()));  // Kullanıcının ID'si
                    userClaims.Add(new Claim(ClaimTypes.Name, isUser.UserName ?? ""));  // Kullanıcı UserName'i
                    userClaims.Add(new Claim(ClaimTypes.GivenName, isUser.Name ?? ""));  // Kullanıcı Adı
                    userClaims.Add(new Claim(ClaimTypes.UserData, isUser.Image ?? ""));  // Kullanıcı Resmi

                    if (isUser.Email == "info@mertaydin.com")  // Eğer girilen mail buysa admin yapar.
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                    }

                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);  // Kullanıcının kimlik bilgilerini içeren "userClaims" listesini alır ve  Kullanıcının kimlik doğrulama yöntemi olarak "Cookie Authentication" kullanılacağını belirtir.

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true  // Kullanıcının oturumu kapatıp açsa bile devam etsin.
                    };

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);  // Eğer giriş yapmış kullanıcı varsa çıkış yapılır.

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);  // Kullanıcı başarıyla giriş yaptığında tarayıcıya bir "kimlik doğrulama çerezi" bırakılır. Bundan sonra bu kullanıcı yetkili olarak sistemde gezebilir.

                    return RedirectToAction("Index", "Posts");  // Giriş başarılıysa ana sayfaya yönledirir.
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya şifre yanlış");
                }
            }
            return View(model);
        }


        public async Task<IActionResult> Logout()  // Hesaptan çıkış yapar.
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }


        public IActionResult Profile(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }
            var user = _userRepository
                        .Users
                        .Include(x => x.Posts)
                        .Include(x => x.Comments)
                        .ThenInclude(x => x.Post)
                        .FirstOrDefault(x => x.UserName == username);

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

    }
}
