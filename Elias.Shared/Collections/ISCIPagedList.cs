using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elias.Shared.Collections
{
    /// <summary>
    /// The interface for paged list collections
    /// </summary>
    public interface ISCIPagedList
    {
        int CurrentPage { get; set; }
        int FirstPageItem { get; set; }
        int LastPageItem { get; set; }
        int TotalItems { get; set; }
        int TotalPages { get; set; }
        int PageSize { get; set; }

        string SearchTerm { get; set; }
        string SortBy { get; set; }
        bool IsAscendingOrder { get; set; }
    }
}
