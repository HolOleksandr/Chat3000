namespace Chat.BLL.Models.Paging
{
    public class SearchParameters
    {
        const int _defaultPageIndex = 0;
        const int _defaultPageSize = 5;
        public int PageIndex { get; set; } = _defaultPageIndex;
        public int PageSize { get; set; } = _defaultPageSize;
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public string? FilterQuery { get; set; }
    }
}
