namespace BookStore.Models.DTOs;

public record BookCard(
    int Id,
    string Title,
    string Slug,
    string? CategoryTitle,
    List<string> AuthorNames
);
