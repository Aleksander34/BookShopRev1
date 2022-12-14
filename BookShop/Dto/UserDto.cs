using AutoMapper;
using BookShop.Core.Models;

namespace BookShop.Dto
{
    [AutoMap(typeof(User), ReverseMap = true)]
    public class UserDto: EntityDto
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public TypeRole Role { get; set; }
    }
}
