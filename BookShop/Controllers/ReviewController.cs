﻿using AutoMapper;
using BookShop.Core;
using BookShop.Core.Models;
using BookShop.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : BookShopController
    {
        private readonly BookShopContext _context;
        private readonly IMapper _mapper;
        public ReviewController(BookShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet("[action]")]
        public IActionResult GetAll()
        {
            var reviews = _context.Reviews.ToList();
            var result = _mapper.Map<IEnumerable<ReviewDto>>(reviews);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult Create(ReviewDto input)
        {
            var review = _mapper.Map<Review>(input);
            _context.Reviews.Add(review);
            _context.SaveChanges();

            return Ok($"Просмотр {review.Id} добавлен");
        }
        [HttpPost("[action]")]
        public IActionResult Remove(int id)
        {
            var review = _context.Reviews.Find(id);
            _context.Reviews.Remove(review);
            _context.SaveChanges();
            return Ok($"Просмотр {review.Id} удален ");
        }
        [HttpPost("[action]")]
        public IActionResult Update(ReviewDto input)
        {
            var review = _mapper.Map<Review>(input);
            _context.Reviews.Update(review);
            _context.SaveChanges();
            return Ok($"Просмотр {review.Id} обновлен");
        }
    }
}
