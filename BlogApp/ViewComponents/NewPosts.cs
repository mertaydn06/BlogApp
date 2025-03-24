using BlogApp.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.ViewComponents
{
    public class NewPosts : ViewComponent
    {
        private IPostRepository _postRepository;  // Nesneyi esneklik olması için interface'den türettik. EfPostRepository'den türetseydik, EfPostRepository'yi değiştirdiğimizde her yerde kodu güncellememiz gerekirdi.

        public NewPosts(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()  // Action gibi bir metod tanımladık.
        {
            return View(await _postRepository
                                .Posts
                                .OrderByDescending(p => p.PublishedOn)
                                .Take(5)
                                .ToListAsync()  // Siteye yüklenmiş en son 5 postu liste şeklinde View'a gönderdik.
            );
        }
    }
}
