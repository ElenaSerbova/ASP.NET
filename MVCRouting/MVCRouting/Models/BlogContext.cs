using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCRouting.Models
{
    public class BlogContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostTag> PostTags { get; set; }

        public BlogContext(DbContextOptions<BlogContext> options)
           : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostTag>()
                .HasKey(pt => new { pt.PostId, pt.TagId });

            var tags = new Tag[]
            {
                new Tag{ Id=1, Name=".NET"},
                new Tag{ Id=2, Name="ASP.NET"},
                new Tag{ Id=3, Name="ADO.NET"}
            };

            var posts = new Post[]
            {
                new Post {
                    Id=1,
                    Title="ASP.NET Core",
                    Content="ASP.NET Core — свободно-распространяемый кросс-платформенный фреймворк " +
                    "для создания веб-приложений с открытым исходным кодом. Данная платформа " +
                    "разрабатывается компанией Майкрософт совместно с сообществом и имеет большую " +
                    "производительность по сравнению с ASP.NET.",
                    DateTime=DateTime.Now
                },
                new Post {
                    Id=2,
                    Title="ADO.NET",
                    Content="ADO.NET — технология, предоставляющая доступ и управление данными, " +
                    "хранящимся в базе данных или других источниках, основанных на платформе .NET " +
                    "Framework и входящая в состав .NET Framework 2.0, представляет собой набор " +
                    "библиотек.",
                    DateTime=DateTime.Now
                },
                new Post {
                    Id=3,
                    Title="ADO.NET Entity Framework",
                    Content="ADO.NET Entity Framework — объектно-ориентированная технология " +
                    "доступа к данным, является object-relational mapping решением для .NET " +
                    "Framework от Microsoft. Предоставляет возможность взаимодействия с объектами " +
                    "как посредством LINQ в виде LINQ to Entities, так и с использованием Entity SQL.",
                    DateTime=DateTime.Now
                }
            };

            var postTags = new PostTag[]
            {
                new PostTag{PostId=1, TagId=1},
                new PostTag{PostId=1, TagId=2},
                new PostTag{PostId=2, TagId=1},
                new PostTag{PostId=2, TagId=3},
                new PostTag{PostId=3, TagId=1},
                new PostTag{PostId=3, TagId=3},
            };

            modelBuilder.Entity<Tag>().HasData(tags);
            modelBuilder.Entity<Post>().HasData(posts);
            modelBuilder.Entity<PostTag>().HasData(postTags);

            base.OnModelCreating(modelBuilder);
        }
    }
}
