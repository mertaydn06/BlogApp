using BlogApp.Entity;

namespace BlogApp.Data.Abstract
{
    public interface IPostRepository  // Kullanacağımız metotları burada soyut olarak tanımladık.
    {
        IQueryable<Post> Posts { get; }

        void CreatePost(Post post);

        void EditPost(Post post);
        
        void EditPost(Post post, int[] tagIds);
    }
}
