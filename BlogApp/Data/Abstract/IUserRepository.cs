using BlogApp.Entity;

namespace BlogApp.Data.Abstract
{
    public interface IUserRepository  // Kullanacağımız metotları burada soyut olarak tanımladık.
    {
        IQueryable<User> Users { get; }

        void CreateUser(User User);
    }
}
