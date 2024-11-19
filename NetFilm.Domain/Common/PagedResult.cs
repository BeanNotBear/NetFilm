using System;
using System.Collections.Generic;

namespace NetFilm.Domain.Common
{
	/// <summary>
	/// Represents the paged result of a query
	/// </summary>
	/// <typeparam name="TModel">Model type</typeparam>
	public class PagedResult<TModel> where TModel : class
	{
		/// <summary>
		/// Items in the current page
		/// </summary>
		public IEnumerable<TModel> Items { get; private set; }

		/// <summary>
		/// Total number of items across all pages
		/// </summary>
		public int TotalItems { get; private set; }

		/// <summary>
		/// Total number of pages, calculated from TotalItems and PageSize
		/// </summary>
		public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

		/// <summary>
		/// Current page number
		/// </summary>
		public int PageIndex { get; private set; }

		private int _pageSize;
		/// <summary>
		/// Number of items per page (maximum of 30)
		/// </summary>
		public int PageSize
		{
			get => _pageSize;
			private set => _pageSize = Math.Min(value, 50); // Ensure a maximum of 25
		}

		/// <summary>
		/// Indicates if there is a previous page
		/// </summary>
		public bool HasPrevious => PageIndex > 1;

		/// <summary>
		/// Indicates if there is a next page
		/// </summary>
		public bool HasNext => PageIndex < TotalPages;

		/// <summary>
		/// Initializes a new instance of the PagedResult class
		/// </summary>
		/// <param name="items">The items in the current page</param>
		/// <param name="totalItems">The total numberc of items across all pages</param>
		/// <param name="pageIndex">The current page number</param>
		/// <param name="pageSize">The number of items per page</param>
		public PagedResult(IEnumerable<TModel> items, int totalItems, int pageIndex, int pageSize)
		{
			Items = items ?? new List<TModel>();
			TotalItems = totalItems;
			PageIndex = pageIndex;
			PageSize = pageSize > 0 ? Math.Min(pageSize, 50) : 50; // Apply max 25 for pageSize
		}
	}
}
