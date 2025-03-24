using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;

namespace BlogApp.Data.Concrete
{
    public class EfUserRepository : IUserRepository  // Data/Abstract/IUserRepository.cs sınıfını impletente ettik. ve soyut metotların içlerini doldurduk.
    {
        private BlogContext _context;  // Veritabanı bağlantılarını tutan değişken.

        public EfUserRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<User> Users => _context.Users;  // Burada veritabanından sorgulama yapmadık (Tolist gibi). Sorgulamayı action metotları üzerinden yapacağız.

        public void CreateUser(User user)  // User ekleme metodu.
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}
