namespace BookStore.Models.DTOs;

public record BookCardSimple(
    int Id,
    string Title,
    string Slug,
    string? CategoryTitle,
    string? Description
);
