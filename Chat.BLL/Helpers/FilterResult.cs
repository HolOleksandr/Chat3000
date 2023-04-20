using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;


namespace Chat.BLL.Helpers
{
    public class FilterResult<T>
    {
        private FilterResult(
        List<T> data,
        int count,
        int pageIndex,
        int pageSize,
        string? sortColumn,
        string? sortOrder,
        string? filterQuery)
        {
            Data = data;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            SortColumn = sortColumn;
            SortOrder = sortOrder;
            FilterQuery = filterQuery;
        }
        public static async Task<FilterResult<T>> CreateAsync(
            IEnumerable<T> source,
            int pageIndex,
            int pageSize,
            string? sortColumn = null,
            string? sortOrder = null,
            string? filterColumn = null,
            string? filterQuery = null)
        {

            if (!string.IsNullOrEmpty(filterColumn)
                && !string.IsNullOrEmpty(filterQuery)
                && IsValidProperty(filterColumn))
            {
                source = source.AsQueryable().Where(
                    string.Format("{0}.StartsWith(@0)",
                    filterColumn),
                    filterQuery);
            }

            var count = source.AsQueryable().Count();

            if (!string.IsNullOrEmpty(sortColumn)
                && IsValidProperty(sortColumn))
            {
                sortOrder = !string.IsNullOrEmpty(sortOrder)
                    && sortOrder.ToUpper() == "ASC"
                    ? "ASC"
                    : "DESC";

                source = source.AsQueryable().OrderBy(
                    string.Format("{0} {1}", sortColumn, sortOrder));
            }

            source = source
                .Skip(pageIndex * pageSize)
                .Take(pageSize);

            var data = await Task.FromResult(source.ToList());

            return new FilterResult<T>(
            data,
            count,
            pageIndex,
            pageSize,
            sortColumn,
            sortOrder,
            filterQuery);
        }

        public static bool IsValidProperty(
        string propertyName,
        bool throwExceptionIfNotFound = true)
        {
            var prop = typeof(T).GetProperty(
            propertyName,
            BindingFlags.IgnoreCase |
            BindingFlags.Public |
            BindingFlags.Instance);
            if (prop == null && throwExceptionIfNotFound)
                throw new NotSupportedException(
                string.Format(
                $"ERROR: Property '{propertyName}' does not exist.")
                );
            return prop != null;
        }
        
        
        public List<T> Data { get; private set; }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 0);
            }
        }
        public bool HasNextPage
        {
            get
            {
                return ((PageIndex + 1) < TotalPages);
            }
        }

        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public string? FilterQuery { get; set; }
    }
}
