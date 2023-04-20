namespace Chat.Blazor.Server.Models.Paging
{
    public class PagingResponse<T> where T : class
    {
        public List<T>? Data { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public string? FilterQuery { get; set; }
    }
}
