using BlogApp.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.ViewComponents
{
    public class TagsMenu : ViewComponent
    {
        private ITagRepository _tagRepository;  // Nesneyi esneklik olması için interface'den türettik. EfTagRepository'den türetseydik, EfTagRepository'yi değiştirdiğimizde her yerde kodu güncellememiz gerekirdi.

        public TagsMenu(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()  // Action gibi bir metod tanımladık.
        {
            return View(await _tagRepository.Tags.ToListAsync());  // Tag listesini TagsMenu'ye ait Default.cshtml sayfasına gönderdik.
        }
    }
}
