namespace Shopping.Web.Models.Ordering
{
    public class PaginatedResult<TEntity> where TEntity : class
    {
        public int PageIndex { get; }
        public int PageSize { get; }
        public long Count { get; }
        public IEnumerable<TEntity> Data { get; }
    }
}
