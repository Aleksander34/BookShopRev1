using AutoMapper;
using BookShop.Core;
using BookShop.Core.Models;
using BookShop.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BookShopController
    {
        private readonly BookShopContext _context;
        private readonly IMapper _mapper;
        public UserController(BookShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet("[action]")]
        public IActionResult GetAll()
        {
            var users = _context.Users.ToList();
            var result = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult Create(UserDto input)
        {
            var user = _mapper.Map<User>(input);
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok($"user {user.Id} added");
        }
        [HttpPost("[action]")]
        public IActionResult Remove(int id)
        {
            var user = _context.Users.Find(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
            return Ok($"User {user.Id} удален ");
        }
        [HttpPost("[action]")]
        public IActionResult Update(UserDto input)
        {
            var user = _mapper.Map<User>(input);
            _context.Users.Update(user);
            _context.SaveChanges();
            return Ok($"User {user.Id} updated");
        }
    }
}
