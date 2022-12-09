using AutoMapper;
using BookShop.Core.Models;

namespace BookShop.Dto
{
    [AutoMap(typeof(Property), ReverseMap = true)]
    public class PropertyDto: EntityDto
    {
        public string Color { get; set; }
        public string BindingType { get; set; }
        public string Condition { get; set; }
    }
}
