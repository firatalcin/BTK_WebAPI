using System.Reflection;
using System.Text;
using Entities.Models;
using Entities.RequestFeatures;
using System.Linq.Dynamic.Core;

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

    public static IQueryable<Book> Sort(this IQueryable<Book> books, string orderByQueryString)
    {
        if(string.IsNullOrWhiteSpace(orderByQueryString))
            return books.OrderBy(x => x.Id);
        
        var orderParams = orderByQueryString.Trim().Split(',');
        
        var propertyInfos = typeof(Book).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var orderQueryBuilder = new StringBuilder();
        
        foreach (var param in orderParams)
        {
            if(string.IsNullOrWhiteSpace(param))
                continue;

            var propertyFromQueryName = param.Split(' ')[0];

            var objectProperty = propertyInfos.FirstOrDefault(pi =>
                pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

            if (objectProperty is null)
                continue;
            
            var direction = param.EndsWith(" desc", StringComparison.InvariantCultureIgnoreCase) ? "descending" : "ascending";
            
            orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction},");
        }
        
        var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
        
        if(orderQuery is null)
            return books.OrderBy(x => x.Id);
        
        return books.OrderBy(orderQuery);
    }
}