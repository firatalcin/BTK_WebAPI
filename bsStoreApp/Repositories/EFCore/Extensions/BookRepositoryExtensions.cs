using Entities.Models;
using Entities.RequestFeatures;

namespace Repositories.EFCore;

public static class BookRepositoryExtensions
{
    public static IQueryable<Book> FilterBooks(this IQueryable<Book> books, 
        uint minPrice, uint maxPrice) => 
        books.Where(book => book.Price >= minPrice && book.Price <= maxPrice);

    public static IQueryable<Book> Search(this IQueryable<Book> books, string searchTerm)
    {
        if(string.IsNullOrWhiteSpace(searchTerm))
            return books;
        
        var lowerCaseSearchTerm = searchTerm.Trim().ToLower();
        return books.Where(book => book.Title.ToLower().Contains(lowerCaseSearchTerm));
    }
}