using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elias.Shared.Collections
{
    /// <summary>
    /// This class is used for dividing collections into pages. It stores a page part for a collection and information like current page, total pages, etc.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SCIPagedList<T> : ISCIPagedList, IEnumerable<T>
    {
        private IEnumerable<T> _collection;

        public int CurrentPage { get; set; }
        public int FirstPageItem { get; set; }
        public int LastPageItem { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }

        public string SearchTerm { get; set; }
        public string SortBy { get; set; }
        public bool IsAscendingOrder { get; set; }

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Accepts an IQueryable collection and stores the appropriate page part
        /// </summary>
        /// <param name="collection">The collection to use</param>
        /// <param name="page">The number of the page to store</param>
        /// <param name="pageSize">The size for each page</param>
        /// <param name="totalItems">The total number of items in the collection</param>
        /// <param name="searchTerm">The term used for searching in the collection</param>
        /// <param name="sortBy">The field used to sort the collection</param>
        /// <param name="isAscendingOrder">Indicates if the collection is sorted in an ascending order or not</param>
        public SCIPagedList(IEnumerable<T> collection, int page, int pageSize, int totalItems, string searchTerm, string sortBy, bool isAscendingOrder)
        {
            this.SearchTerm = searchTerm;
            this.SortBy = sortBy;
            this.IsAscendingOrder = isAscendingOrder;

            this.PageSize = pageSize;
            this.TotalItems = totalItems;
            this.TotalPages = (int)Math.Ceiling(this.TotalItems / (double)pageSize);

            this.CurrentPage = page;
            if (this.CurrentPage < 1)
            {
                this.CurrentPage = 1;
            }
            else if (this.CurrentPage > this.TotalPages)
            {
                this.CurrentPage = this.TotalPages;
            }

            this.FirstPageItem = (this.CurrentPage - 1) * this.PageSize + 1;
            this.LastPageItem = Math.Min(this.FirstPageItem + this.PageSize - 1, this.TotalItems);

            _collection = collection;
        }

        /// <summary>
        /// Accepts an IQueryable collection and stores the appropriate page part
        /// </summary>
        /// <param name="collection">The collection to use</param>
        /// <param name="page">The number of the page to store</param>
        /// <param name="pageSize">The size for each page</param>
        /// <param name="searchTerm">The term used for searching in the collection</param>
        /// <param name="sortBy">The field used to sort the collection</param>
        /// <param name="isAscendingOrder">Indicates if the collection is sorted in an ascending order or not</param>
        public SCIPagedList(IQueryable<T> collection, int page, int pageSize, string searchTerm, string sortBy, bool isAscendingOrder)
        {
            this.SearchTerm = searchTerm;
            this.SortBy = sortBy;
            this.IsAscendingOrder = isAscendingOrder;

            this.PageSize = pageSize;
            this.TotalItems = collection.Count();
            this.TotalPages = (int)Math.Ceiling(this.TotalItems / (double)pageSize);

            this.CurrentPage = page;
            if (this.CurrentPage < 1)
            {
                this.CurrentPage = 1;
            }
            else if (this.CurrentPage > this.TotalPages)
            {
                this.CurrentPage = this.TotalPages;
            }

            this.FirstPageItem = (this.CurrentPage - 1) * this.PageSize + 1;
            this.LastPageItem = Math.Min(this.FirstPageItem + this.PageSize - 1, this.TotalItems);

            if (this.TotalItems == 0)
            {
                _collection = new List<T>();
            }
            else
            {
                _collection = collection.Skip(this.FirstPageItem - 1).Take(this.PageSize).ToList();
            }
        }

        /// <summary>
        /// Accepts an IQueryable collection and stores the appropriate page part
        /// </summary>
        /// <param name="collection">The collection to use</param>
        /// <param name="page">The number of the page to store</param>
        /// <param name="pageSize">The size for each page</param>
        public SCIPagedList(IQueryable<T> collection, int page, int pageSize)
            : this(collection, page, pageSize, null, null, false)
        {

        }

        /// <summary>
        /// Accepts an IQueryable collection and stores the appropriate page part
        /// </summary>
        /// <param name="collection">The collection to use</param>
        /// <param name="page">The number of the page to store</param>
        /// <param name="pageSize">The size for each page</param>
        /// <param name="totalItems">The total number of items in the collection</param>
        public SCIPagedList(IEnumerable<T> collection, int page, int pageSize, int totalItems)
            : this(collection, page, pageSize, totalItems, null, null, false)
        {

        }

        /// <summary>
        /// Constructs a paged list by copying information from another paged list
        /// </summary>
        /// <param name="collection">The collection of the paged list</param>
        /// <param name="otherList">The paged list to copy information from</param>
        public SCIPagedList(IEnumerable<T> collection, ISCIPagedList otherList)
        {
            _collection = collection;

            this.PageSize = otherList.PageSize;
            this.TotalItems = otherList.TotalItems;
            this.TotalPages = otherList.TotalPages;
            this.CurrentPage = otherList.CurrentPage;
            this.FirstPageItem = otherList.FirstPageItem;
            this.LastPageItem = otherList.LastPageItem;
            this.SearchTerm = otherList.SearchTerm;
            this.SortBy = otherList.SortBy;
            this.IsAscendingOrder = otherList.IsAscendingOrder;
        }
    }
}
