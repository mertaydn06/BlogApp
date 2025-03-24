using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;

namespace BlogApp.Data.Concrete
{
    public class EfTagRepository : ITagRepository  // Data/Abstract/ITagRepository.cs sınıfını impletente ettik. ve soyut metotların içlerini doldurduk.
    {
        private BlogContext _context;  // Veritabanı bağlantılarını tutan değişken.

        public EfTagRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<Tag> Tags => _context.Tags;  // Burada veritabanından sorgulama yapmadık (Tolist gibi). Sorgulamayı action metotları üzerinden yapacağız.

        public void CreateTag(Tag Tag)  // Tag ekleme metodu.
        {
            _context.Tags.Add(Tag);
            _context.SaveChanges();
        }
    }
}
