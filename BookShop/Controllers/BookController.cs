﻿using AutoMapper;
using BookShop.Core;
using BookShop.Core.Models;
using BookShop.Dto;
using BookShop.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace BookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : BookShopController
    {
        private readonly BookShopContext _context;
        private readonly IMapper _mapper;
        public BookController(BookShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet("[action]")]
        public IActionResult GetAll()
        {
            var books = _context.Books.ToList();
            var result = _mapper.Map<IEnumerable<BookDto>>(books);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult Create(BookDto input)
        {
            var book = _mapper.Map<Book>(input);
            _context.Books.Add(book);
            _context.SaveChanges();

            return Ok($"book {book.Id} added");
        }
        [HttpPost("[action]")]
        public IActionResult Remove(int id)
        {
            var book = _context.Books.Find(id);
            _context.Books.Remove(book);
            _context.SaveChanges();
            return Ok($"Book {book.Id} удален ");
        }
        [HttpPost("[action]")]
        public IActionResult Update(BookDto input)
        {
            var book = _mapper.Map<Book>(input);
            _context.Books.Update(book);
            _context.SaveChanges();
            return Ok($"Book {book.Id} updated");
        }

        [HttpPost("[action]")]
        public IActionResult GetPreviewBooks([FromForm]IFormFile input)
        {
            string pathToFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!System.IO.Directory.Exists(Path.Combine(pathToFolder, "TemporaryStorage")))
            {
                Directory.CreateDirectory(Path.Combine(pathToFolder, "TemporaryStorage"));
            }

            string pathToFile = Path.Combine(pathToFolder, "TemporaryStorage", input.FileName);
            using (var stream = System.IO.File.Create(pathToFile))
            {
                input.CopyTo(stream);
            }
            var result = ExcelParcerUtil.ParseBook(pathToFile);
            System.IO.File.Delete(pathToFile);
            return Ok(result);
        }
    }
}
