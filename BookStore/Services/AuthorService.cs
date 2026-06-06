using BookStore.Data;
using BookStore.Models.DTOs;
using BookStore.Models.ViewModels;
using BookStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services;

public class AuthorService : IAuthorService
{
    private readonly AppDbContext _context;

    public AuthorService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AuthorCard>> GetAuthorsAsync()
    {
        return await _context.Authors
            .AsNoTracking()
            .Select(a => new AuthorCard(
                a.Id,
                a.FullName,
                a.Slug,
                a.BookAuthors.Count
            ))
            .ToListAsync();
    }

    public async Task<AuthorBooksViewModel?> GetAuthorBooksAsync(string slug)
    {
        var author = await _context.Authors
            .AsNoTracking()
            .Where(a => a.Slug == slug)
            .Select(a => new AuthorInfo(a.Id, a.FullName, a.Slug))
            .FirstOrDefaultAsync();

        if (author == null) return null;

        var books = await _context.BookAuthors
            .AsNoTracking()
            .Where(ba => ba.AuthorId == author.Id)
            .Select(ba => new BookCardSimple(
                ba.Book.Id,
                ba.Book.Title,
                ba.Book.Slug,
                ba.Book.Category.Title,
                ba.Book.Description
            ))
            .ToListAsync();

        return new AuthorBooksViewModel
        {
            Author = author,
            Books = books
        };
    }
}
