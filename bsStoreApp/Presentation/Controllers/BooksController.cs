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
    public IActionResult GetAllBooks()
    {
        var books = _manager.BookService.GetAllBooks(false);
        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
    {
        var book = _manager.BookService.GetOneBookById(id, false);
        return Ok(book);
    }

    [HttpPost]
    public IActionResult CreateOneBook([FromBody] Book book)
    {
        if (string.IsNullOrEmpty(book.Title))
            return BadRequest();

        _manager.BookService.CreateOneBook(book);
        return StatusCode(201, book);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] BookDtoForUpdate bookDto)
    {
        if (bookDto is null && id <= 0)
            return BadRequest();

        _manager.BookService.UpdateOneBook(id, bookDto, true);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
    {
        _manager.BookService.DeleteOneBook(id, true);
        return NoContent();
    }
}