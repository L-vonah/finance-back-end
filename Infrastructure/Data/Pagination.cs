namespace Infrastructure.Data
{
    public enum SortOrder { Ascending, Descending }

    public class PageFilter
    {
        /// <summary>Page number</summary>
        public int Page { get; set; }

        /// <summary>Number of items per page</summary>
        public int PageSize { get; set; } = 10;

        /// <summary>Sort by property name</summary>
        public string? SortBy { get; set; }

        /// <summary>Sort order</summary>
        public SortOrder? SortOrder { get; set; }

        public void Validate(Type classType)
        {
            if (Page < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(Page), "Page number must be greater than 0");
            }
            if (PageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(PageSize), "Page size must be greater than 0");
            }
            if (SortBy != null)
            {
                var property = classType.GetProperty(SortBy);
                if (property == null)
                {
                    throw new ArgumentException($"Property {SortBy} does not exist in {classType.Name}");
                }
            }
        }
    }

    public class SearchableFilter : PageFilter
    {
        public string Search { get; set; } = string.Empty;
    }

    public class PaginatedList<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }
    }
}
