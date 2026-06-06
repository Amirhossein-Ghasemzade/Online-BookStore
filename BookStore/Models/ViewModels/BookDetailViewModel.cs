using BookStore.Models.DTOs;

namespace BookStore.Models.ViewModels;

public class BookDetailViewModel
{
    public BookDetail Book { get; set; } = null!;
    public List<RelatedBook> RelatedBooks { get; set; } = [];
}
