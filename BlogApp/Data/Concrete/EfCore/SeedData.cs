using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public static class SeedData  // Eğer tablolarda veri yoksa tablolara veri ekler.
    {
        public static void TestVerileriniDoldur(IApplicationBuilder app)  // Program.cs'de ki app'i alarak onun üzerinde işlem yaptık.
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();

            if (context != null)
            {
                if (context.Database.GetPendingMigrations().Any())  // Eğer oluşturulmuş ama veritabanına gönderilmemiş(dotnet ef database update) Migrations varsa true döner.
                {
                    context.Database.Migrate();  // Uygulama her çalıştırıldığında veritabanı sıfırdan oluşturulsun.
                }

                if (!context.Tags.Any())  // Eğer hiç Tags tablosunda hiç veri yoksa
                {
                    context.Tags.AddRange(
                        new Tag { Text = "web programlama", Url = "web-programlama", Color = TagColors.warning },
                        new Tag { Text = "backend", Url = "backend", Color = TagColors.info },
                        new Tag { Text = "frontend", Url = "frontend", Color = TagColors.success },
                        new Tag { Text = "fullstack", Url = "fullstack", Color = TagColors.secondary },
                        new Tag { Text = "php", Url = "php", Color = TagColors.primary }
                    );
                    context.SaveChanges();
                }

                if (!context.Users.Any())  // Eğer hiç Users tablosunda hiç veri yoksa
                {
                    context.Users.AddRange(
                        new User { UserName = "mertaydin", Name = "Mert Aydın", Email = "info@mertaydin.com", Password = "123456", Image = "p1.jpg" },
                        new User { UserName = "tayfunaydin", Name = "Tayfun Aydın", Email = "info@tayfunaydin.com", Password = "123456", Image = "p2.jpg" }
                    );
                    context.SaveChanges();
                }


                if (!context.Posts.Any())
                {
                    context.Posts.AddRange(
                        new Post
                        {
                            Title = "Asp.net core",
                            Description = "Asp.net core dersleri",
                            Content = "Asp.net core dersleri",
                            Url = "aspnet-core",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-10),
                            Tags = context.Tags.Take(3).ToList(),  // Tags tablosundan 3 tane veri aldık ve ilişkilendirdik.
                            Image = "1.jpg",
                            UserId = 1,
                            Comments = new List<Comment> {
                                new Comment { Text = "iyi bir kurs", PublishedOn = DateTime.Now.AddDays(-20), UserId = 1},
                                new Comment { Text = "çok faydalandığım bir kurs", PublishedOn = DateTime.Now.AddDays(-10), UserId = 2},
                            }
                        },
                        new Post
                        {
                            Title = "Php",
                            Description = "Php core dersleri",
                            Content = "Php core dersleri",
                            Url = "php",
                            IsActive = true,
                            Image = "2.jpg",
                            PublishedOn = DateTime.Now.AddDays(-20),
                            Tags = context.Tags.Take(2).ToList(),
                            UserId = 1
                        },
                        new Post
                        {
                            Title = "Django",
                            Description = "Django dersleri",
                            Content = "Django dersleri",
                            Url = "django",
                            IsActive = true,
                            Image = "3.jpg",
                            PublishedOn = DateTime.Now.AddDays(-30),
                            Tags = context.Tags.Take(4).ToList(),
                            UserId = 2
                        }
                        ,
                        new Post
                        {
                            Title = "React Dersleri",
                            Content = "React dersleri",
                            Url = "react-dersleri",
                            IsActive = true,
                            Image = "4.jpg",
                            PublishedOn = DateTime.Now.AddDays(-40),
                            Tags = context.Tags.Take(4).ToList(),
                            UserId = 2
                        }
                        ,
                        new Post
                        {
                            Title = "Angular",
                            Content = "Angular dersleri",
                            Url = "angular",
                            IsActive = true,
                            Image = "5.jpg",
                            PublishedOn = DateTime.Now.AddDays(-50),
                            Tags = context.Tags.Take(4).ToList(),
                            UserId = 2
                        }
                        ,
                        new Post
                        {
                            Title = "Web Tasarım",
                            Content = "Web tasarım dersleri",
                            Url = "web-tasarim",
                            IsActive = true,
                            Image = "6.jpg",
                            PublishedOn = DateTime.Now.AddDays(-60),
                            Tags = context.Tags.Take(4).ToList(),
                            UserId = 2
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
