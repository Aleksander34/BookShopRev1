using BookShop.Core;
using BookShop.Core.Models;
using Microsoft.EntityFrameworkCore;

var context = new BookShopContext();
//достать все книги красного цвета
var books = context.Books
    .Include(x => x.Property)
    .Where(x => x.Property.Color == "red");

//получение всех авторов конкретной книги
var bookName = "";
var authors = context.Books
    .Include(x=>x.BookAuthors)
    .ThenInclude(x=>x.Author)
    .First(x=>x.Title==bookName)
    .BookAuthors
    .Select(x=>x.Author.Name);

//получить все названия книги которым всегда передают три следующие фильтры, сразу передают все фильтры
var color = "red";
var condition = "used";
int? numStars = 3;

var books2 = context.Books
    .Include(x => x.Property)
    .Include(x => x.Reviews)
    .Where(x => x.Property.Color == color && x.Property.Condition == condition)
    .Where(x => Math.Round(x.Reviews.Average(y => y.NumStars)) == numStars)
    .Select(x => x.Title);

//получить все названия книги которым передают три следующие фильтры (фильтры могут прийти да не все), сразу передают все фильтры. Пример с неправильным подходом
color = "red";
condition = "used";
numStars = 3;
decimal? price = 44.5m;

IEnumerable<Book> books3 = context.Books;  //! В случае использования IEnumerable мы тянем из БД все данные, необходимо работать с IQuerable
if (color != null)
{
    books3 = books3
        .Where(x => x.Property.Color == color);
}
if (price != null)
{
    books3 = books3
        .Where(x => x.Price == price);
}

//получить все названия книги которым передают три следующие фильтры (фильтры могут прийти да не все), сразу передают все фильтры.
color = "red";
condition = "used";
numStars = 3;
price = 44.5m;

IQueryable<Book> books4 = context.Books;  //! В случае использования IEnumerable мы тянем из БД все данные, необходимо работать с IQuerable
if (color != null)
{
    books4 = books4
        .Include(x => x.Property)
        .Where(x => x.Property.Color == color);
}
if (price != null)
{
    books4 = books4
        .Where(x => x.Price == price);
}
if (condition != null)
{
    books4 = books4
        .Include(x => x.Property)
        .Where(x => x.Property.Condition == condition);
}
if (numStars != null)
{
    books4 = books4
        .Include(x => x.Reviews)
        .Where(x => Math.Round(x.Reviews.Average(y => y.NumStars)) == numStars);
}

var titles = books4.Select(x => x.Title).ToList();

var bookInfo = books4.Select(x => new
{
    x.Title,  // автосвойства сокращенные
    PriceOfBook = x.Price
})
.ToList(); // берем название и цену книг

// пагинация постраничное отображение или скрол
int countBookOnPage = 10;
int numPage = 10; // какую страницу мы хотим видеть и запросить
int countTitles = books4.Select(x => x.Title).Count();
titles = books4.Skip((numPage-1)*countBookOnPage).Take(countBookOnPage)
    .Select(x => x.Title).ToList();