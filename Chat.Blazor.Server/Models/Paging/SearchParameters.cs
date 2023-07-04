using Microsoft.AspNetCore.Components;

namespace Chat.Blazor.Server.Models.Paging
{
    public class SearchParameters
    {
        [Parameter]
        [SupplyParameterFromQuery(Name = "pageIndex")]
        public int PageIndex { get; set; }
        [Parameter]
        [SupplyParameterFromQuery(Name = "pageSize")]
        public int PageSize { get; set; }
        [Parameter]
        [SupplyParameterFromQuery(Name = "sortcolumn")]
        public string? SortColumn { get; set; }
        [Parameter]
        [SupplyParameterFromQuery(Name = "sortorder")]
        public string? SortOrder { get; set; }
        [Parameter]
        [SupplyParameterFromQuery(Name = "filterquery")]
        public string? FilterQuery { get; set; }
    }
}
