using AutoMapper;
using Entities.DTOs;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
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

    public async Task<IEnumerable<BookDto>> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges)
    {
        var books = await _repositoryManager.Book.GetAllBooksAsync(bookParameters, trackChanges);
        return _mapper.Map<IEnumerable<BookDto>>(books);
    }

    public async Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges)
    {
        var book = await GetOneBookAndCheckExistsAsync(id, trackChanges);
        return _mapper.Map<BookDto>(book);
    }

    public async Task<BookDto> CreateOneBookAsync(BookDtoForInsertion bookDtoForInsertion)
    {
        if (bookDtoForInsertion.Title.Length < 2)
        {
            _logger.LogInfo("Book title is more than 2 characters.");
        }

        var bookEntity = _mapper.Map<Book>(bookDtoForInsertion);
        
        _repositoryManager.Book.CreateOneBook(bookEntity);
        await _repositoryManager.SaveAsync();
        return _mapper.Map<BookDto>(bookEntity);
    }

    public async Task UpdateOneBookAsync(int id, BookDtoForUpdate bookDto, bool trackChanges)
    {
        var bookDtoForUpdate = _mapper.Map<Book>(bookDto);
        _repositoryManager.Book.UpdateOneBook(bookDtoForUpdate);
        await _repositoryManager.SaveAsync();
    }

    public async Task DeleteOneBookAsync(int id, bool trackChanges)
    {
        var entity = await GetOneBookAndCheckExistsAsync(id, trackChanges);
        
        _repositoryManager.Book.DeleteOneBook(entity);
        await _repositoryManager.SaveAsync();
    }

    private async Task<Book> GetOneBookAndCheckExistsAsync(int id, bool trackChanges)
    {
        var entity = await _repositoryManager.Book.GetOneBookByIdAsync(id, trackChanges);

        if (entity is null)
            throw new BookNotFoundException(id);

        return entity;
    }
}