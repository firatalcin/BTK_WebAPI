﻿using Entities.DTOs;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Presentation.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    private readonly IServiceManager _manager;

    public BooksController(IServiceManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBooksAsync()
    {
        var books = await _manager.BookService.GetAllBooksAsync(false);
        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOneBookAsync([FromRoute(Name = "id")] int id)
    {
        var book = await _manager.BookService.GetOneBookByIdAsync(id, false);
        return Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOneBookAsync([FromBody] BookDtoForInsertion bookDto)
    {
        if (string.IsNullOrEmpty(bookDto.Title))
            return BadRequest();

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var book = await _manager.BookService.CreateOneBookAsync(bookDto);
        return StatusCode(201, book);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateOneBookAsync([FromRoute(Name = "id")] int id, [FromBody] BookDtoForUpdate bookDto)
    {
        if (bookDto is null && id <= 0)
            return BadRequest();
        
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        await _manager.BookService.UpdateOneBookAsync(id, bookDto, false);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteOneBookAsync([FromRoute(Name = "id")] int id)
    {
        await _manager.BookService.DeleteOneBookAsync(id, true);
        return NoContent();
    }
}