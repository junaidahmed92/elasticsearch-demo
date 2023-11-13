namespace ElasticSearchLearning.Models
{
    public class SearchViewModel
    {
        public string Keyword { get; set; }
        public IEnumerable<Book> Books { get; set; } = new List<Book>();
    }
}
