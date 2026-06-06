using BookStore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DashboardController : Controller
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.BookCount = await _context.Books.CountAsync();
        ViewBag.AuthorCount = await _context.Authors.CountAsync();
        ViewBag.CategoryCount = await _context.Categories.CountAsync();
        ViewBag.KeywordCount = await _context.Keywords.CountAsync();
        return View();
    }
}
