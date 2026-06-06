using BookStore.Data;
using BookStore.Models;
using BookStore.Models.DTOs;
using BookStore.Models.ViewModels;
using BookStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BookStore.Services;

public class BookService : IBookService
{
    private readonly AppDbContext _context;
    private readonly IMemoryCache _cache;

    public BookService(AppDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<BookIndexViewModel> GetBooksIndexAsync(
        string? categorySlug, string? authorSlug, string? keyword, int page, int pageSize = 10)
    {
        var query = _context.Books.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(categorySlug))
            query = query.Where(b => b.Category.Slug == categorySlug);

        if (!string.IsNullOrEmpty(authorSlug))
            query = query.Where(b => b.BookAuthors.Any(ba => ba.Author.Slug == authorSlug));

        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(b => b.Title.Contains(keyword) || b.Description.Contains(keyword));

        var total = await query.CountAsync();

        var books = await query
            .OrderByDescending(b => b.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new BookCard(
                b.Id,
                b.Title,
                b.Slug,
                b.Category.Title,
                b.BookAuthors.Select(ba => ba.Author.FullName).ToList()
            ))
            .ToListAsync();

        var categories = await _cache.GetOrCreateAsync("Categories", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
            return await _context.Categories.AsNoTracking().ToListAsync();
        });

        var authors = await _cache.GetOrCreateAsync("Authors", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
            return await _context.Authors.AsNoTracking().ToListAsync();
        });

        return new BookIndexViewModel
        {
            Books = books,
            Categories = categories!,
            Authors = authors!,
            CategorySlug = categorySlug,
            AuthorSlug = authorSlug,
            Keyword = keyword,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling(total / (double)pageSize)
        };
    }

    public async Task<BookDetailViewModel?> GetBookDetailAsync(string slug)
    {
        var book = await _context.Books
            .AsNoTracking()
            .Where(b => b.Slug == slug)
            .Select(b => new
            {
                b.Id,
                b.Title,
                b.Slug,
                CategoryTitle = b.Category.Title,
                b.Description,
                b.PageCount,
                b.FilePath,
                Authors = b.BookAuthors.Select(ba => new AuthorInfo(ba.Author.Id, ba.Author.FullName, ba.Author.Slug)).ToList(),
                Keywords = b.BookKeywords.Select(bk => bk.Keyword.Word).ToList()
            })
            .FirstOrDefaultAsync();

        if (book == null) return null;

        var relatedToIds = await _context.BookRelations
            .AsNoTracking()
            .Where(r => r.BookId == book.Id)
            .Select(r => r.RelatedBookId)
            .ToListAsync();

        var relatedFromIds = await _context.BookRelations
            .AsNoTracking()
            .Where(r => r.RelatedBookId == book.Id)
            .Select(r => r.BookId)
            .ToListAsync();

        var allIds = relatedToIds.Concat(relatedFromIds).Distinct().ToList();

        var related = allIds.Count != 0
            ? await _context.Books
                .AsNoTracking()
                .Where(b => allIds.Contains(b.Id))
                .Select(b => new RelatedBook(b.Title, b.Slug, b.Category.Title))
                .ToListAsync()
            : [];

        return new BookDetailViewModel
        {
            Book = new BookDetail
            {
                Id = book.Id,
                Title = book.Title,
                Slug = book.Slug,
                CategoryTitle = book.CategoryTitle,
                Description = book.Description,
                PageCount = book.PageCount,
                FilePath = book.FilePath,
                Authors = book.Authors,
                Keywords = book.Keywords,
                RelatedBooks = related
            }
        };
    }

    public async Task<(string? FilePath, string? Slug)> GetDownloadInfoAsync(int id)
    {
        var result = await _context.Books
            .AsNoTracking()
            .Where(b => b.Id == id)
            .Select(b => new { b.FilePath, b.Slug })
            .FirstOrDefaultAsync();

        return (result?.FilePath, result?.Slug);
    }
}
