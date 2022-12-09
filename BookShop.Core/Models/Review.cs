using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Core.Models
{
    public class Review : Entity
    {
        public string Name { get; set; }
        public int NumStars { get; set; }
        public string Comment { get; set; }
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }

        public override string ToString()
        {
            return $"{{ Имя: {Name}, Оценки: {NumStars}, Комментарии: {Comment}, Id Книги: {BookId}}}";
        }
    }
}
