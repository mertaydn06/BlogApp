namespace BlogApp.Entity;

public class Post
{
    public int PostId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Content { get; set; }

    public string? Url { get; set; }

    public string? Image { get; set; }

    public DateTime PublishedOn { get; set; }

    public bool IsActive { get; set; }


    public int UserId { get; set; }

    public User User { get; set; } = null!;  // Her post bir User'a ait olacak.


    public List<Tag> Tags { get; set; } = new List<Tag>();   // Bir Post'un birden fazla Tag'i olabileceği için List kullandık. // Tag ile many to many ilişkisi olduğu için veritabanı otomatik olarak PostTag tablosu üretti.


    public List<Comment> Comments { get; set; } = new List<Comment>();   // Bir Post'un birden fazla Comment'i olabileceği için List kullandık.
}
