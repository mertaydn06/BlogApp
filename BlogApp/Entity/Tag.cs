namespace BlogApp.Entity;

public enum TagColors  // Her tag'in farklı bir rengi olması için oluşturduk.
{
    primary, danger, warning, success, secondary, info
}

public class Tag
{
    public int TagId { get; set; }

    public string? Text { get; set; }

    public string? Url { get; set; }

    public TagColors? Color { get; set; }


    public List<Post> Posts { get; set; } = new List<Post>();  // Bir Tag'de birden fazla Post olabileceği için List kullandık. // Post ile many to many ilişkisi olduğu için veritabanı otomatik olarak PostTag tablosu üretti.
}
