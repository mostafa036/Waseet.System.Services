namespace Waseet.System.Services.APIs.Helper
{
    public class Pagination<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> products { get; set; }

        public Pagination(int pageIndex, int pageSize, int count, IReadOnlyList<T> product)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            products = product;
        }
    }
}