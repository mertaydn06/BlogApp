using BlogApp.Entity;

namespace BlogApp.Data.Abstract
{
    public interface ICommentRepository  // Kullanacağımız metotları burada soyut olarak tanımladık.
    {
        IQueryable<Comment> Comments { get; }

        void CreateComment(Comment comment);
    }
}
