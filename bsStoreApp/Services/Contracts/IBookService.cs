﻿using System.Dynamic;
using Entities.DTOs;
using Entities.Models;
using Entities.RequestFeatures;

namespace Services.Contracts;

public interface IBookService
{
    Task<(IEnumerable<ShapedEntity> books, MetaData metaData)> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges);
    Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges);
    Task<BookDto> CreateOneBookAsync(BookDtoForInsertion book);
    Task UpdateOneBookAsync(int id, BookDtoForUpdate bookDto, bool trackChanges);
    Task DeleteOneBookAsync(int id, bool trackChanges);
}