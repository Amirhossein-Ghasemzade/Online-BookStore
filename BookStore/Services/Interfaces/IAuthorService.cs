using BookStore.Models.DTOs;
using BookStore.Models.ViewModels;

namespace BookStore.Services.Interfaces;

public interface IAuthorService
{
    Task<List<AuthorCard>> GetAuthorsAsync();
    Task<AuthorBooksViewModel?> GetAuthorBooksAsync(string slug);
}
