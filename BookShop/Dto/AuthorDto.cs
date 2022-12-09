using AutoMapper;
using BookShop.Core.Models;

namespace BookShop.Dto
{
    [AutoMap(typeof(Author), ReverseMap = true)]
    public class AuthorDto:EntityDto
    {
        public string Name { get; set; }
        // public BookDto[] Books { get; set; }  Это не выводится но может быть в запросе при связывании использовано
    }
}
