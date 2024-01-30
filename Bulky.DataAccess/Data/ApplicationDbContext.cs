using Bulky.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Action1", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Action2", DisplayOrder = 3 },
                new Category { Id = 4, Name = "Action3", DisplayOrder = 4 }
                );

            modelBuilder.Entity<Product>().HasData(
              new Product
              {
                  Id = 1,
                  Title = "Fortune of time",
                  Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
                  Author = "Billy Spark",
                  ISBN = "SWD9999001",
                  ListPrice = 99,
                  Price = 90,
                  Price50 = 85,
                  Price100 = 80,
                  CategoryId=1,
                  ImageUrl=""
              },
              new Product
              {
                  Id = 2,
                  Title = "Title 2",
                  Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
                  Author = "Bill Gate",
                  ISBN = "SWD9999002",
                  ListPrice = 100,
                  Price = 99,
                  Price50 = 80,
                  Price100 = 85,
                  CategoryId = 1,
                  ImageUrl = ""
              }
              );
        }
    }
}
