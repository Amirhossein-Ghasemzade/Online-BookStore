using BookStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Book> Books => Set<Book>();
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Keyword> Keywords => Set<Keyword>();
        public DbSet<BookAuthor> BookAuthors => Set<BookAuthor>();
        public DbSet<BookKeyword> BookKeywords => Set<BookKeyword>();
        public DbSet<BookRelation> BookRelations => Set<BookRelation>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Book>(b =>
            {
                b.ToTable("Books");
                b.HasKey(x => x.Id);
                b.Property(x => x.Title).IsRequired().HasMaxLength(500);
                b.Property(x => x.Slug).IsRequired().HasMaxLength(500);
                b.Property(x => x.FilePath).HasMaxLength(1000);
                b.Property(x => x.Description).HasMaxLength(4000);
                b.HasOne(x => x.Category).WithMany(x => x.Books).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
                b.HasIndex(x => x.Slug).IsUnique();
            });

            builder.Entity<Author>(b =>
            {
                b.ToTable("Authors");
                b.HasKey(x => x.Id);
                b.Property(x => x.FullName).IsRequired().HasMaxLength(300);
                b.Property(x => x.Slug).IsRequired().HasMaxLength(300);
                b.HasIndex(x => x.Slug).IsUnique();
            });

            builder.Entity<Category>(b =>
            {
                b.ToTable("Categories");
                b.HasKey(x => x.Id);
                b.Property(x => x.Title).IsRequired().HasMaxLength(200);
                b.Property(x => x.Slug).IsRequired().HasMaxLength(200);
                b.HasIndex(x => x.Slug).IsUnique();
            });

            builder.Entity<Keyword>(b =>
            {
                b.ToTable("Keywords");
                b.HasKey(x => x.Id);
                b.Property(x => x.Word).IsRequired().HasMaxLength(200);
                b.HasIndex(x => x.Word).IsUnique();
            });

            builder.Entity<BookAuthor>(b =>
            {
                b.ToTable("BookAuthors");
                b.HasKey(x => new { x.BookId, x.AuthorId });
                b.HasOne(x => x.Book).WithMany(x => x.BookAuthors).HasForeignKey(x => x.BookId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.Author).WithMany(x => x.BookAuthors).HasForeignKey(x => x.AuthorId).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<BookKeyword>(b =>
            {
                b.ToTable("BookKeywords");
                b.HasKey(x => new { x.BookId, x.KeywordId });
                b.HasOne(x => x.Book).WithMany(x => x.BookKeywords).HasForeignKey(x => x.BookId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.Keyword).WithMany(x => x.BookKeywords).HasForeignKey(x => x.KeywordId).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<BookRelation>(b =>
            {
                b.ToTable("BookRelations");
                b.HasKey(x => new { x.BookId, x.RelatedBookId });
                b.HasOne(x => x.Book).WithMany(x => x.RelatedTo).HasForeignKey(x => x.BookId).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(x => x.RelatedBook).WithMany(x => x.RelatedFrom).HasForeignKey(x => x.RelatedBookId).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
