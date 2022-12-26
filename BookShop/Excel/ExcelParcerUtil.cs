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
            var result = new ExcelInputDto();
            var books = new List<BookDto>();
            using (var excel = new ExcelPackage(file))
            {
                var worksheet = excel.Workbook.Worksheets[1];
                for (int row = 3; row < worksheet.Dimension.End.Row ; row++) // начинаем с 3 строки и каждый раз увеличиваем на 1
                {
                    

                    books.Add(GetBook(worksheet, row));
                }
            }
            result.Books=books;
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
            result.Authors = worksheet.Cells[$"G{row}"].Value.ToString().Trim();
            result.Property=GetProperty(worksheet, row);
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
