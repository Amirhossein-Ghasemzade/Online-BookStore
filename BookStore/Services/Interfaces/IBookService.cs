using BookStore.Models.DTOs;
using BookStore.Models.ViewModels;

namespace BookStore.Services.Interfaces;

public interface IBookService
{
    Task<BookIndexViewModel> GetBooksIndexAsync(string? categorySlug, string? authorSlug, string? keyword, int page, int pageSize = 10);
    Task<BookDetailViewModel?> GetBookDetailAsync(string slug);
    Task<(string? FilePath, string? Slug)> GetDownloadInfoAsync(int id);
}
