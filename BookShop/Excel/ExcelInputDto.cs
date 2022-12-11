using BookShop.Dto;

namespace BookShop.Excel
{
    public class ExcelInputDto
    {
        public List<BookDto>Books { get; set; }
        public List<PropertyDto> Properties { get; set; }
        public List<AuthorDto> Authors { get; set; }
    }
}
