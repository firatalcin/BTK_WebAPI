using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services.Managers;

public class BookManager : IBookService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerService _logger;

    public BookManager(IRepositoryManager repositoryManager, ILoggerService logger)
    {
        _repositoryManager = repositoryManager;
        _logger = logger;
    }

    public IEnumerable<Book> GetAllBooks(bool trackChanges)
    {
        return _repositoryManager.Book.GetAllBooks(trackChanges);
    }

    public Book GetOneBookById(int id, bool trackChanges)
    {
        return _repositoryManager.Book.GetOneBookById(id, trackChanges);
    }

    public Book CreateOneBook(Book book)
    {
        if (book.Title.Length < 2)
        {
            _logger.LogInfo("Book title is more than 2 characters.");
        }
        
        _repositoryManager.Book.CreateOneBook(book);
        _repositoryManager.Save();
        return book;
    }

    public void UpdateOneBook(int id, Book book, bool trackChanges)
    {
        var entity = _repositoryManager.Book.GetOneBookById(id, trackChanges);

        if (entity is null)
        {
            var message = $"Book with id: {id} could not found";
            _logger.LogInfo(message);
            throw new Exception(message);
        }
            

        
        if(book is null)
            throw new ArgumentNullException(nameof(book));
        
        entity.Title = book.Title;
        entity.Price = book.Price;
        
        _repositoryManager.Book.UpdateOneBook(entity);
        _repositoryManager.Save();

    }

    public void DeleteOneBook(int id, bool trackChanges)
    {
        var entity = _repositoryManager.Book.GetOneBookById(id, trackChanges);

        if (entity is null)
        {
            _logger.LogInfo($"Book with id: {id} could not found");
            throw new Exception($"Book with id: {id} could not found");
        }
        
        _repositoryManager.Book.DeleteOneBook(entity);
        _repositoryManager.Save();
    }
}