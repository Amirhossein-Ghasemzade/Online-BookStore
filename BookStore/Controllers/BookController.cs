using BookStore.Data;
using BookStore.Models;
using BookStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers;

[Authorize]
public class BookController : Controller
{
    private readonly AppDbContext _context;

    public BookController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? categorySlug, string? authorSlug, string? keyword, int page = 1)
    {
        int pageSize = 10;
        var query = _context.Books
            .Include(b => b.Category)
            .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
            .AsQueryable();

        if (!string.IsNullOrEmpty(categorySlug))
            query = query.Where(b => b.Category.Slug == categorySlug);

        if (!string.IsNullOrEmpty(authorSlug))
            query = query.Where(b => b.BookAuthors.Any(ba => ba.Author.Slug == authorSlug));

        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(b => b.Title.Contains(keyword) || b.Description.Contains(keyword));

        int total = await query.CountAsync();
        var books = await query
            .OrderByDescending(b => b.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var vm = new BookIndexViewModel
        {
            Books = books,
            Categories = await _context.Categories.ToListAsync(),
            Authors = await _context.Authors.ToListAsync(),
            CategorySlug = categorySlug,
            AuthorSlug = authorSlug,
            Keyword = keyword,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling(total / (double)pageSize)
        };

        return View(vm);
    }

    public async Task<IActionResult> Detail(string slug)
    {
        var book = await _context.Books
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.Slug == slug);

        if (book == null) return NotFound();

        var bookAuthors = await _context.BookAuthors
            .Include(ba => ba.Author)
            .Where(ba => ba.BookId == book.Id)
            .ToListAsync();

        var bookKeywords = await _context.BookKeywords
            .Include(bk => bk.Keyword)
            .Where(bk => bk.BookId == book.Id)
            .ToListAsync();

        var relatedToIds = await _context.BookRelations
            .Where(r => r.BookId == book.Id)
            .Select(r => r.RelatedBookId)
            .ToListAsync();

        var relatedFromIds = await _context.BookRelations
            .Where(r => r.RelatedBookId == book.Id)
            .Select(r => r.BookId)
            .ToListAsync();

        var allRelatedIds = relatedToIds.Concat(relatedFromIds).Distinct().ToList();

        var relatedBooks = allRelatedIds.Count != 0
            ? await _context.Books
                .Include(b => b.Category)
                .Where(b => allRelatedIds.Contains(b.Id))
                .ToListAsync()
            : [];

        book.BookAuthors = bookAuthors;
        book.BookKeywords = bookKeywords;

        var vm = new BookDetailViewModel
        {
            Book = book,
            RelatedBooks = relatedBooks
        };

        return View(vm);
    }

    public async Task<IActionResult> Download(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null || string.IsNullOrEmpty(book.FilePath))
            return NotFound();

        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", book.FilePath.TrimStart('/'));
        if (!System.IO.File.Exists(path))
            return NotFound();

        var ext = Path.GetExtension(path);
        var mime = ext?.ToLower() switch
        {
            ".pdf" => "application/pdf",
            ".zip" => "application/zip",
            _ => "application/octet-stream"
        };

        return PhysicalFile(path, mime, $"{book.Slug}{ext}");
    }
}
