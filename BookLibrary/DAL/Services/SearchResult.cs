namespace DAL.Services
{
    public class SearchResult<T>
    {
        public int Count { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<T> Items { get; set; }

        public SearchResult()
        {
        }

        public SearchResult(int count, int page, int total, IEnumerable<T> items)
        {
            this.Count = count;
            this.Page = page;
            this.TotalPages = total;
            this.Items = items;
        }
    }
}
