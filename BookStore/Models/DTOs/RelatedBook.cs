namespace BookStore.Models.DTOs;

public record RelatedBook(
    string Title,
    string Slug,
    string? CategoryTitle
);
