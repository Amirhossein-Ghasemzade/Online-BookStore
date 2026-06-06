using BookStore.Data;
using BookStore.Models;
using BookStore.Models.ViewModels.Admin;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services.Admin;

public class AdminService
{
    private readonly AppDbContext _context;

    public AdminService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<Book>> GetBooksListAsync(string? search, int page, int pageSize = 10)
    {
        var query = _context.Books
            .Include(b => b.Category)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(search))
            query = query.Where(b => b.Title.Contains(search));

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(b => b.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedList<Book>(items, total, page, pageSize);
    }

    public async Task<AdminBookViewModel> GetEmptyBookFormAsync()
    {
        var vm = new AdminBookViewModel();
        await PopulateLookupsAsync(vm);
        return vm;
    }

    public async Task<AdminBookViewModel?> GetBookFormAsync(string slug)
    {
        var book = await _context.Books
            .Include(b => b.BookAuthors)
            .Include(b => b.BookKeywords)
            .Include(b => b.RelatedTo)
            .Include(b => b.RelatedFrom)
            .FirstOrDefaultAsync(b => b.Slug == slug);

        if (book == null) return null;

        var relatedIds = book.RelatedTo.Select(r => r.RelatedBookId)
            .Concat(book.RelatedFrom.Select(r => r.BookId))
            .Distinct()
            .ToList();

        var vm = new AdminBookViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Slug = book.Slug,
            Description = book.Description,
            PageCount = book.PageCount,
            CategoryId = book.CategoryId,
            FilePath = book.FilePath,
            AuthorIds = book.BookAuthors.Select(ba => ba.AuthorId).ToList(),
            KeywordIds = book.BookKeywords.Select(bk => bk.KeywordId).ToList(),
            RelatedBookIds = relatedIds
        };

        await PopulateLookupsAsync(vm);
        return vm;
    }

    public async Task PopulateLookupsAsync(AdminBookViewModel vm)
    {
        vm.AllCategories = await _context.Categories.AsNoTracking().ToListAsync();
        vm.AllAuthors = await _context.Authors.AsNoTracking().ToListAsync();
        vm.AllKeywords = await _context.Keywords.AsNoTracking().ToListAsync();
        vm.AllBooks = await _context.Books
            .AsNoTracking()
            .Where(b => b.Id != vm.Id)
            .Select(b => new BookItem(b.Id, b.Title))
            .ToListAsync();
    }

    public async Task<string> CreateBookAsync(AdminBookViewModel vm)
    {
        var slug = await GenerateSlugAsync(vm.Title);

        var book = new Book
        {
            Title = vm.Title,
            Slug = slug,
            Description = vm.Description,
            PageCount = vm.PageCount,
            CategoryId = vm.CategoryId,
            FilePath = vm.FilePath
        };

        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        book.BookAuthors = vm.AuthorIds.Select(aId => new BookAuthor { BookId = book.Id, AuthorId = aId }).ToList();
        book.BookKeywords = vm.KeywordIds.Select(kId => new BookKeyword { BookId = book.Id, KeywordId = kId }).ToList();
        book.RelatedTo = vm.RelatedBookIds.Select(rId => new BookRelation { BookId = book.Id, RelatedBookId = rId }).ToList();

        await _context.SaveChangesAsync();
        return slug;
    }

    public async Task UpdateBookAsync(AdminBookViewModel vm)
    {
        var book = await _context.Books
            .Include(b => b.BookAuthors)
            .Include(b => b.BookKeywords)
            .Include(b => b.RelatedTo)
            .Include(b => b.RelatedFrom)
            .FirstOrDefaultAsync(b => b.Id == vm.Id);

        if (book == null) return;

        book.Title = vm.Title;
        book.Description = vm.Description;
        book.PageCount = vm.PageCount;
        book.CategoryId = vm.CategoryId;
        book.FilePath = vm.FilePath;

        _context.BookAuthors.RemoveRange(book.BookAuthors);
        _context.BookKeywords.RemoveRange(book.BookKeywords);
        _context.BookRelations.RemoveRange(book.RelatedTo);
        // RelatedFrom entries belong to other books, don't delete them

        book.BookAuthors = vm.AuthorIds.Select(aId => new BookAuthor { BookId = book.Id, AuthorId = aId }).ToList();
        book.BookKeywords = vm.KeywordIds.Select(kId => new BookKeyword { BookId = book.Id, KeywordId = kId }).ToList();
        book.RelatedTo = vm.RelatedBookIds.Select(rId => new BookRelation { BookId = book.Id, RelatedBookId = rId }).ToList();

        await _context.SaveChangesAsync();
    }

    public async Task DeleteBookAsync(int id)
    {
        var book = await _context.Books
            .Include(b => b.BookAuthors)
            .Include(b => b.BookKeywords)
            .Include(b => b.RelatedTo)
            .Include(b => b.RelatedFrom)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book == null) return;

        _context.BookRelations.RemoveRange(book.RelatedFrom);
        await _context.SaveChangesAsync(); // must save before removing book
        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
    }

    private async Task<string> GenerateSlugAsync(string title)
    {
        var slug = title.ToLower()
            .Replace(' ', '-')
            .Replace("--", "-");

        if (!await _context.Books.AnyAsync(b => b.Slug == slug))
            return slug;

        var counter = 1;
        while (await _context.Books.AnyAsync(b => b.Slug == $"{slug}-{counter}"))
            counter++;

        return $"{slug}-{counter}";
    }
}
