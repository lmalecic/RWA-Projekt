namespace DAL.Services
{
    public class SearchResult<T>
    {
        public int Count;
        public int Page;
        public int Total;
        public IEnumerable<T> Results;

        public SearchResult(int count, int page, int total, IEnumerable<T> results)
        {
            this.Count = count;
            this.Page = page;
            this.Total = total;
            this.Results = results;
        }
    }
}
