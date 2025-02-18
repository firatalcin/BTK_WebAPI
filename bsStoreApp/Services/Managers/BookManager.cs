using AutoMapper;
using Entities.DTOs;
using Entities.Exceptions;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services.Managers;

public class BookManager : IBookService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerService _logger;
    private readonly IMapper _mapper;

    public BookManager(IRepositoryManager repositoryManager, ILoggerService logger, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _logger = logger;
        _mapper = mapper;
    }

    public IEnumerable<Book> GetAllBooks(bool trackChanges)
    {
        return _repositoryManager.Book.GetAllBooks(trackChanges);
    }

    public Book GetOneBookById(int id, bool trackChanges)
    {
        var book = _repositoryManager.Book.GetOneBookById(id, trackChanges);
        if (book is null)
            throw new BookNotFoundException(id);
        return book;
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

    public void UpdateOneBook(int id, BookDtoForUpdate bookDto, bool trackChanges)
    {
        var entity = _repositoryManager.Book.GetOneBookById(id, trackChanges);

        if (entity is null)
            throw new BookNotFoundException(id);
        
        // entity.Title = book.Title;
        // entity.Price = book.Price;

        entity = _mapper.Map<Book>(bookDto);
        
        _repositoryManager.Book.UpdateOneBook(entity);
        _repositoryManager.Save();

    }

    public void DeleteOneBook(int id, bool trackChanges)
    {
        var entity = _repositoryManager.Book.GetOneBookById(id, trackChanges);
        
        if (entity is null)
            throw new BookNotFoundException(id);
        
        _repositoryManager.Book.DeleteOneBook(entity);
        _repositoryManager.Save();
    }
}