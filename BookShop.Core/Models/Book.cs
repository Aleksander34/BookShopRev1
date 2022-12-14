using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Core.Models
{
    public class Book : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishedOn { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public int PropertyId { get; set; }
        [ForeignKey("PropertyId")]
        public Property Property { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }

        public override string ToString()
        {
            return $"{{ Заголовок: {Title}, Описание: {Description} Опубликованно: {PublishedOn}, Издатель: {Category}, Цена: {Price}, Ссылка: {ImageUrl}, Id cвойства: {PropertyId}}}";
        }
    }
}
