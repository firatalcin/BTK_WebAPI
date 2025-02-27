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

    public IEnumerable<BookDto> GetAllBooks(bool trackChanges)
    {
        var books = _repositoryManager.Book.GetAllBooks(trackChanges);
        return _mapper.Map<IEnumerable<BookDto>>(books);
    }

    public BookDto GetOneBookById(int id, bool trackChanges)
    {
        var book = _repositoryManager.Book.GetOneBookById(id, trackChanges);
        if (book is null)
            throw new BookNotFoundException(id);
        return _mapper.Map<BookDto>(book);
    }

    public BookDto CreateOneBook(BookDtoForInsertion bookDtoForInsertion)
    {
        if (bookDtoForInsertion.Title.Length < 2)
        {
            _logger.LogInfo("Book title is more than 2 characters.");
        }

        var bookEntity = _mapper.Map<Book>(bookDtoForInsertion);
        
        _repositoryManager.Book.CreateOneBook(bookEntity);
        _repositoryManager.Save();
        return _mapper.Map<BookDto>(bookEntity);
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