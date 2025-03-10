﻿using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories.EFCore;

public class BookRepository : RepositoryBase<Book>, IBookRepository
{
    public BookRepository(RepositoryContext context) : base(context)
    {
    }


    public async Task<IEnumerable<Book>> GetAllBooksAsync(bool trackChanges)
    {
        return await FindAll(trackChanges)
            .OrderBy(b => b.Id)
            .ToListAsync();
    }

    public async Task<Book> GetOneBookByIdAsync(int id, bool trackChanges)
    {
        return await FindByCondition(b => b.Id == id, trackChanges).SingleOrDefaultAsync();
    }

    public void CreateOneBook(Book book)
    {
        Create(book);
    }

    public void UpdateOneBook(Book book)
    {
        Update(book);
    }

    public void DeleteOneBook(Book book)
    {
        Delete(book);
    }
}