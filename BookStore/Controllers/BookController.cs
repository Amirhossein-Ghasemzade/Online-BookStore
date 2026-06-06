using BookStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers;

[Authorize]
public class BookController : Controller
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    public async Task<IActionResult> Index(string? categorySlug, string? authorSlug, string? keyword, int page = 1)
    {
        var vm = await _bookService.GetBooksIndexAsync(categorySlug, authorSlug, keyword, page);
        return View(vm);
    }

    public async Task<IActionResult> Detail(string slug)
    {
        var vm = await _bookService.GetBookDetailAsync(slug);
        if (vm == null) return NotFound();
        return View(vm);
    }

    public async Task<IActionResult> Download(int id)
    {
        var (filePath, slug) = await _bookService.GetDownloadInfoAsync(id);
        if (filePath == null) return NotFound();

        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'));
        if (!System.IO.File.Exists(path)) return NotFound();

        var ext = Path.GetExtension(path);
        var mime = ext?.ToLower() switch
        {
            ".pdf" => "application/pdf",
            ".zip" => "application/zip",
            _ => "application/octet-stream"
        };

        return PhysicalFile(path, mime, $"{slug}{ext}");
    }
}
