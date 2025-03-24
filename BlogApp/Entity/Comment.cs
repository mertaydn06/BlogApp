namespace BlogApp.Entity;

public class Comment
{
    public int CommentId { get; set; }

    public string? Text { get; set; }

    public DateTime PublishedOn { get; set; }


    public int PostId { get; set; }

    public Post Post { get; set; } = null!;  // Her comment bir Post'a ait olacak.


    public int UserId { get; set; }

    public User User { get; set; } = null!;  // Her comment bir User'a ait olacak.
}
