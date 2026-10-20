using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Filters
{
    public class QueryStringParameters
    {
        private int _pageSize = 10;
        private const int _maxPageSize = 20;
        private string _sortDirection = "asc";
        public int PageNumber { get; set; } = 1;
        
        public int PageSize 
        {
            get => _pageSize;
            set => _pageSize = Math.Min(Math.Max(1, value), _maxPageSize);
        }
        public string SortDirection 
        {
            get => _sortDirection;
            set => _sortDirection = value?.Trim().ToLower() == "desc" ? "desc" : "asc"; 
        }
    }
}
