namespace BookShop.Core.Models
{
    public class Author : Entity
    {
        public string Name { get; set; }
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
        public override string ToString()
        {
            return $"{{ Id автора: {Id}, Автор: {Name}}}";
        }
    }
}
