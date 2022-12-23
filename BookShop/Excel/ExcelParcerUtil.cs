using BookShop.Dto;
using OfficeOpenXml;
using System.Text;

namespace BookShop.Excel
{
    public static class ExcelParcerUtil
    {
        public static ExcelInputDto ParseBook(string pathToFile)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var file = new FileInfo(pathToFile);
            var result = new ExcelInputDto()
            {
                Books = new List<BookDto>(),
                Properties = new List<PropertyDto>(),
                Authors = new List<AuthorDto>(),
            };

            using (var excel = new ExcelPackage(file))
            {
                var worksheet = excel.Workbook.Worksheets[1];
                for (int row = 3; ; row++) // начинаем с 3 строки и каждый раз увеличиваем на 1
                {
                    string end = worksheet.Cells[$"A{row}"].Value.ToString().Trim();
                    if (end == "End of typing")
                        break;
                    result.Books.Add(GetBook(worksheet, row));
                    result.Properties.Add(GetProperty(worksheet, row));
                    result.Authors.Add(GetAuthor(worksheet, row));
                }
            }
            return result;
        }
        static BookDto GetBook(ExcelWorksheet worksheet, int row)
        {
            var result = new BookDto();
            result.Title= worksheet.Cells[$"A{row}"].Value.ToString().Trim();
            result.Description = worksheet.Cells[$"B{row}"].Value.ToString().Trim();
            result.PublishedOn = DateTime.Parse(worksheet.Cells[$"C{row}"].Value.ToString().Trim());
            result.Category = worksheet.Cells[$"D{row}"].Value.ToString().Trim();
            result.ImageUrl = worksheet.Cells[$"E{row}"].Value.ToString().Trim();
            result.Price = decimal.Parse(worksheet.Cells[$"F{row}"].Value.ToString().Trim());
            return result;
        }
        static AuthorDto GetAuthor(ExcelWorksheet worksheet, int row)
        {
            var result = new AuthorDto();
            result.Name = worksheet.Cells[$"G{row}"].Value.ToString().Trim();
            return result;
        }
        static PropertyDto GetProperty(ExcelWorksheet worksheet, int row)
        {
            var result = new PropertyDto();
            result.Color = worksheet.Cells[$"H{row}"].Value.ToString().Trim();
            result.BindingType = worksheet.Cells[$"I{row}"].Value.ToString().Trim();
            result.Condition = worksheet.Cells[$"J{row}"].Value.ToString().Trim();
            return result;
        }
    }
}
