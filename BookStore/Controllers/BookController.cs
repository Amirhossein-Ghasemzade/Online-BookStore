using BookStore.Data;
using BookStore.Models;
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

        ViewBag.TotalPages = (int)Math.Ceiling(total / (double)pageSize);
        ViewBag.CurrentPage = page;
        ViewBag.CategorySlug = categorySlug;
        ViewBag.AuthorSlug = authorSlug;
        ViewBag.Keyword = keyword;
        ViewBag.Categories = await _context.Categories.ToListAsync();
        ViewBag.Authors = await _context.Authors.ToListAsync();

        return View(books);
    }

    public async Task<IActionResult> Detail(string slug)
    {
        var book = await _context.Books
            .Include(b => b.Category)
            .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
            .Include(b => b.BookKeywords).ThenInclude(bk => bk.Keyword)
            .Include(b => b.RelatedTo).ThenInclude(r => r.RelatedBook).ThenInclude(b => b.Category)
            .Include(b => b.RelatedFrom).ThenInclude(r => r.Book).ThenInclude(b => b.Category)
            .FirstOrDefaultAsync(b => b.Slug == slug);

        if (book == null) return NotFound();

        var relatedBooks = book.RelatedTo.Select(r => r.RelatedBook)
            .Concat(book.RelatedFrom.Select(r => r.Book))
            .Distinct().ToList();

        ViewBag.RelatedBooks = relatedBooks;
        return View(book);
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
