using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs;

public abstract record BookDtoForManipulation
{
    [Required(ErrorMessage = "Title is required")]
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters")]
    [MaxLength(50, ErrorMessage = "Title cannot exceed 50 characters")]
    public string Title { get; init; }
    [Required(ErrorMessage = "Price is required")]
    [Range(10,1000)]
    public decimal Price { get; init; }
}