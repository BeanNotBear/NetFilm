using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Application.DTOs.CommentDTOs
{
    public class CommentQueryParams
    {
        private int _pageSize = 10;
        private int _pageIndex = 1;

        public int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = value <= 0 ? 1 : value;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value <= 0 ? 10 : Math.Min(value, 25);
        }

        public string? SearchTerm { get; set; }

        public string? SortBy { get; set; }

        public bool Ascending { get; set; } = true;

        // Validation method to ensure values are within acceptable ranges
        public void Validate()
        {
            if (PageIndex < 1)
                throw new ArgumentException("Page index cannot be less than 1", nameof(PageIndex));

            if (PageSize < 1)
                throw new ArgumentException("Page size cannot be less than 1", nameof(PageSize));

            // Validate sortBy if provided
            if (!string.IsNullOrWhiteSpace(SortBy))
            {
                var validSortFields = new[]
                {
                "content",
                "username",
                "movie",
                "date"

            };

                if (!validSortFields.Contains(SortBy.ToLower()))
                {
                    throw new ArgumentException(
                        $"Invalid sort field. Valid values are: {string.Join(", ", validSortFields)}",
                        nameof(SortBy));
                }
            }
        }
    }
}
