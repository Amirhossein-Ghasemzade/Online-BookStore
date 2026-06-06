namespace BookStore.Models.DTOs;

public record AuthorCard(
    int Id,
    string FullName,
    string Slug,
    int BookCount
);
