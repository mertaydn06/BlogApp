using System.Security.Claims;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        // private readonly BlogContext _context;

        private readonly IPostRepository _postRepository;  // Interface'den nesne türeterek Dependency Injection yaptık.
        private readonly ICommentRepository _commentRepository;
        private readonly ITagRepository _tagRepository;

        public PostsController(IPostRepository postRepository, ICommentRepository commentRepository, ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _tagRepository = tagRepository;
        }

        public async Task<IActionResult> Index(string tag)  // Tag'in url değişkeni burada tag adında yakalanır.
        {
            var posts = _postRepository.Posts.Where(i => i.IsActive);  // Aktive olan postları posts nesnesine gönderdik.

            if (!string.IsNullOrEmpty(tag))
            {
                posts = posts.Where(x => x.Tags.Any(t => t.Url == tag));  // Verilen tag'in hangi postlar içinde dahil olduğu tutularak posts değişkenine yüklenir.
            }

            return View(new PostsViewModel { Posts = await posts.ToListAsync() });  // Seçili tag'in dahil olduğu postlar View'da gösterilir.
        }


        public async Task<IActionResult> Details(string url)  // Postun başlığına tıklanıldığında gönderilen url Post içinde sorgulanır ve bulunun post View'a gönderilir.
        {
            return View(await _postRepository
                        .Posts  // Postun kendisi
                        .Include(x => x.User)  // Post'un User'i
                        .Include(x => x.Tags)  // Post'un Tag'leri
                        .Include(x => x.Comments)  // Post'un Comment'leri
                        .ThenInclude(x => x.User)  // Post'a yorum yapan User'ların bilgileri (Posttan Comment'e Comment'ten de Comment'i yazan kullanıcıya)
                        .FirstOrDefaultAsync(p => p.Url == url));  // En sonunda da eşleşen bu değerler View'a gönderilir.
        }


        [HttpPost]
        public JsonResult AddComment(int PostId, string Text)  // Yorum yap'a bastıktan sonra formdan gelen veriler burada yakalanır.
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);  // Claim'den gelen bilgileri burada aldık.
            var username = User.FindFirstValue(ClaimTypes.Name);
            var avatar = User.FindFirstValue(ClaimTypes.UserData);

            var entity = new Comment  // Formdan gelen veriler burada Comment.cs sınıfının değişkenlerine atanır.
            {
                PostId = PostId,
                Text = Text,
                PublishedOn = DateTime.Now,
                UserId = int.Parse(userId ?? "")
            };

            _commentRepository.CreateComment(entity);  // Formdan gelen veriler veritabanına kayıt edildi.

            return Json(new
            {
                username,
                Text,
                entity.PublishedOn,
                avatar
            });  // Details.cshtml sayfasına bir View döndürmek yerine, JSON formatında bir cevap göndererek sayfa yeniden yüklenmeden güncelleniyor. JavaScript tarafından alınıp. AJAX sayesinde yorum, yorum listesine ekleniyor.
        }


        [Authorize]  // Siteye giriş yapıldığı takdirde çalışır.
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]  // Siteye giriş yapıldığı takdirde çalışır.
        public IActionResult Create(PostCreateViewModel model)  // Create sayfasından gelen verileri VM eşliğinde burada karşıladık.
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);  // Claim'den gelen userId bilgisini burada aldık.

                _postRepository.CreatePost(
                    new Post
                    {
                        Title = model.Title,
                        Content = model.Content,
                        Url = model.Url,
                        UserId = int.Parse(userId ?? ""),
                        PublishedOn = DateTime.Now,
                        Image = "1.jpg",
                        IsActive = false
                    }
                );
                return RedirectToAction("Index");
            }
            return View(model);
        }


        [Authorize]  // Siteye giriş yapıldığı takdirde çalışır.
        public async Task<IActionResult> List()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var role = User.FindFirstValue(ClaimTypes.Role);  // Giriş yapmış kullanıcının role bilgisini aldık.

            var posts = _postRepository.Posts;

            if (string.IsNullOrEmpty(role))  // Eğer kullanıcı rolü boşsa (yani kullanıcı bir admin değilse veya rol verilmemişse), bu koşul çalışır.
            {
                posts = posts.Where(i => i.UserId == userId);  // Bu, filtreleme işlemi yaparak sadece kullanıcının (userId) ait olduğu yazıları getirir. Yani, admin olmayan bir kullanıcı sadece kendi yazılarını görebilir.
            }

            return View(await posts.ToListAsync());  // Admin ise direk burası çalışır ve tüm Post'lar gösterilir.
        }


        [Authorize]
        public IActionResult Edit(int? id)  // Edit butonuna tıklanıldığında gelen id burada yakalanır.
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _postRepository.Posts.Include(i => i.Tags).FirstOrDefault(i => i.PostId == id);  // Verilen id'ye ait varsa Post ve Post'un Tag'lerini döndürür.

            if (post == null)
            {
                return NotFound();
            }

            ViewBag.Tags = _tagRepository.Tags.ToList();

            return View(new PostCreateViewModel  // Postun bilgilerini View'da göstermek için gönerdik.
            {
                PostId = post.PostId,
                Title = post.Title,
                Description = post.Description,
                Content = post.Content,
                Url = post.Url,
                IsActive = post.IsActive,
                Tags = post.Tags  // Tag'lerde gönderildi. 
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(PostCreateViewModel model, int[] tagIds)  // Formdan gelen post bilgileriyle birlikte tag bilgileride yakalanacak.
        {
            if (ModelState.IsValid)
            {
                var entityToUpdate = new Post  // Güncellenen veriler burada yeni nesneye aktarılır.
                {
                    PostId = model.PostId,
                    Title = model.Title,
                    Description = model.Description,
                    Content = model.Content,
                    Url = model.Url
                };

                if (User.FindFirstValue(ClaimTypes.Role) == "admin")  // Kullanıcının rolü admin ise Aktive durumunu değiştirebilir.
                {
                    entityToUpdate.IsActive = model.IsActive;
                }

                _postRepository.EditPost(entityToUpdate, tagIds);

                return RedirectToAction("List");
            }
            ViewBag.Tags = _tagRepository.Tags.ToList();
            return View(model);
        }
    }
}
