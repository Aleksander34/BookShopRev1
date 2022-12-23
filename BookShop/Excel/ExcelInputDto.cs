using BookShop.Dto;

namespace BookShop.Excel
{
    public class ExcelInputDto
    {
        public IEnumerable<BookDto>Books { get; set; }
    }
}
