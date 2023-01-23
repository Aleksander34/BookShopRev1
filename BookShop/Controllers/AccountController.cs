using AutoMapper;
using BookShop.Core;
using BookShop.Core.Models;
using BookShop.Dto;
using BookShop.Dto.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookShop.Controllers
{
    public class AccountController : BookShopController
    {
        private readonly BookShopContext _context;

        private readonly IMapper _mapper;
        public AccountController(BookShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("[action]")]
        public IActionResult Login(AccountInput input)
        {
            string token = GetToken(input.Login, input.Password);
            ClaimsIdentity identity = GetIdentity(input.Login, input.Password);
            return Ok(new { Token=token, Name=identity.Name, Role=identity.Claims.First(x=>x.Type== ClaimsIdentity.DefaultRoleClaimType).Value });
        }

        private string GetToken(string login, string password)
        {
            ClaimsIdentity identity = GetIdentity(login, password);
            if (identity == null)
            {
                throw new Exception("Неправильно введен логин или пароль");
            }
            var nowTime = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: "NdtLabxxx",
                audience: "NdtLabWebxxx",
                notBefore: nowTime,
                expires: nowTime.AddHours(10),
                claims: identity.Claims,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes("asdfghjkl123456j")), SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private ClaimsIdentity GetIdentity(string Name, string password)
        {
            var employee = _context.Users.SingleOrDefault(x => x.Name == Name && x.Password == password);
            if (employee == null)
            {
                return null;
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, employee.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, employee.Role.ToString())
            };
            return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }

        [HttpPost("[action]")]
        public IActionResult Registration(UserDto input) 
        {
            var employee = _context.Users.SingleOrDefault(e => e.Name == input.Name && e.Password == input.Password);
            if (employee != null)
            {
                return BadRequest("Аккаунт занят");
            }
            employee = _mapper.Map<User>(input);
            _context.Users.Add(employee);
            _context.SaveChanges();
            return Login(new AccountInput { Login = employee.Name, Password = employee.Password });
        }
    }
}
