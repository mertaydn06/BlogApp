using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete;
using BlogApp.Data.Concrete.EfCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BlogContext>(options =>
{
    options.UseSqlite(builder.Configuration["ConnectionStrings:Sql_connection"]);
});  //  BlogContext sınıfı SQLite veritabanına bağlanacak şekilde yapılandırılmış olur. Uygulama çalıştırıldığında blog.db veritabanı kullanılır ve EF Core üzerinden veritabanı işlemleri gerçekleştirilebilir.


builder.Services.AddScoped<IPostRepository, EfPostRepository>(); // Burada interface için soyuta(abstract) karşılık somut(concrete) versiyonunu oluşturup gönderdik.
builder.Services.AddScoped<ITagRepository, EfTagRepository>();
builder.Services.AddScoped<ICommentRepository, EfCommentRepository>();
builder.Services.AddScoped<IUserRepository, EfUserRepository>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>  // Cookie'leri aktif ettik.
{
    options.LoginPath = "/Users/Login";  // Giriş yapılmadan korunan bir kaynağa (örneğin post ekleme) erişilmeye çalışıldığında, kullanıcıyı Login sayfasına yönlendirir.
});


var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

SeedData.TestVerileriniDoldur(app);  // Eğer tablolarda veri yoksa tablolara veri ekler.


app.MapControllerRoute(
    name: "post_details",
    pattern: "posts/details/{url}",
    defaults: new { controller = "Posts", action = "Details" }  // http://localhost:5127/posts/details/{url} yazıldığında Details'e 
);

app.MapControllerRoute(
    name: "posts_by_tag",
    pattern: "posts/tag/{tag}",
    defaults: new { controller = "Posts", action = "Index" }  // http://localhost:5127/posts/tag/{tag} yazıldığında Index'e gider.
);

app.MapControllerRoute(
    name: "user_profile",
    pattern: "profile/{username}",
    defaults: new { controller = "Users", action = "Profile" }  // http://localhost:5127/profile/{username} yazıldığında Index'e gider.
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Posts}/{action=Index}/{id?}"
);

app.Run();
