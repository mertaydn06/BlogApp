using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete
{
    public class EfPostRepository : IPostRepository  // Data/Abstract/IPostRepository.cs sınıfını impletente ettik. ve soyut metotların içlerini doldurduk.
    {
        private BlogContext _context;  // Veritabanı bağlantılarını tutan değişken.

        public EfPostRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<Post> Posts => _context.Posts;  // Burada veritabanından sorgulama yapmadık (Tolist gibi). Sorgulamayı action metotları üzerinden yapacağız.

        public void CreatePost(Post post)  // Post ekleme metodu.
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public void EditPost(Post post)  // Post edit metodu.
        {
            var entity = _context.Posts.FirstOrDefault(i => i.PostId == post.PostId);

            if (entity != null)
            {
                entity.Title = post.Title;
                entity.Description = post.Description;
                entity.Content = post.Content;
                entity.Url = post.Url;
                entity.IsActive = post.IsActive;

                _context.SaveChanges();
            }
        }

        public void EditPost(Post post, int[] tagIds)  // Post edit metodu (ekstra parametreli).
        {
            var entity = _context.Posts.Include(i => i.Tags).FirstOrDefault(i => i.PostId == post.PostId);  // Yakalanan post ve tag bilgileri veritabanında varsa döndürür.

            if (entity != null)
            {
                entity.Title = post.Title;
                entity.Description = post.Description;
                entity.Content = post.Content;
                entity.Url = post.Url;
                entity.IsActive = post.IsActive;

                entity.Tags = _context.Tags.Where(tag => tagIds.Contains(tag.TagId)).ToList();  // Seçilen Tag ID'lerini içeren "tagIds" listesine göre, veritabanındaki ilgili etiketleri bulur ve bu etiketleri postun "Tags" özelliğine atar.

                _context.SaveChanges();
            }
        }
    }
}
