using BookShop.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Core
{
    public class BookShopContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BookShop1;Trusted_Connection=True;");
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<BookAuthor>().HasKey(x => new { x.BookId, x.AuthorId });
        //    modelBuilder.Entity<BookAuthor>().HasOne(x => x.Book).WithMany(x => x.BookAuthors).HasForeignKey(x => x.BookId);
        //    modelBuilder.Entity<BookAuthor>().HasOne(x => x.Author).WithMany(x => x.BookAuthors).HasForeignKey(x => x.AuthorId);
        //}
    }
}
