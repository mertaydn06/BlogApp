namespace BlogApp.Entity;

public class User
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Image { get; set; }


    public List<Post> Posts { get; set; } = new List<Post>();  // Bir kullanıcının birden fazla post'u olabileceği için List kullandık.


    public List<Comment> Comments { get; set; } = new List<Comment>();  // Bir kullanıcının birden fazla comment'i olabileceği için List kullandık.
}
