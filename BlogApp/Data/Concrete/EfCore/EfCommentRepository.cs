using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;

namespace BlogApp.Data.Concrete
{
    public class EfCommentRepository : ICommentRepository  // Data/Abstract/ICommentRepository.cs sınıfını impletente ettik. ve soyut metotların içlerini doldurduk.
    {
        private BlogContext _context;  // Veritabanı bağlantılarını tutan değişken.

        public EfCommentRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<Comment> Comments => _context.Comments;  // Burada veritabanından sorgulama yapmadık (Tolist gibi). Sorgulamayı action metotları üzerinden yapacağız.

        public void CreateComment(Comment comment)  // Yorum ekleme metodu.
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }
    }
}
