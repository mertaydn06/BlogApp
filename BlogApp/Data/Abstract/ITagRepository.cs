using BlogApp.Entity;

namespace BlogApp.Data.Abstract
{
    public interface ITagRepository  // Kullanacağımız metotları burada soyut olarak tanımladık.
    {
        IQueryable<Tag> Tags { get; }

        void CreateTag(Tag tag);
    }
}
