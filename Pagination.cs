using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaginationService
{

    public class PagedList<T>
    {
        public int itemsPerPage { get; set; }
        public int pageCount { get; set; }
        public int currentPage { get; set; }
        public List<T> pagedList { get; set; }

        public PagedList(int itemsPerPage, int pageCount, int currentPage, List<T> pagedList)
        {
            this.itemsPerPage = itemsPerPage;
            this.pageCount = pageCount;
            this.currentPage = currentPage;
            this.pagedList = pagedList;
        }
    }

    public class PaginationService
    {
        private const int ITEMS_PER_PAGE = 15;

        public int itemsPerPage { get; set; }

        public PaginationService()
        {
            initialize(ITEMS_PER_PAGE);
        }
        public PaginationService(int itemsPerPage)
        {
            initialize(itemsPerPage);
        }

        private void initialize(int itemsPerPage)
        {
            this.itemsPerPage = itemsPerPage;
        }

        /// <summary>Parses the UrlRoutingString value from the URL for pagination. Default return is 1.</summary>
        public int ParseUrlRoutingString(string UrlRoutingString)
        {
            if (string.IsNullOrWhiteSpace(UrlRoutingString)) return 1;

            int pageNumber = 1;
            bool parse = int.TryParse(UrlRoutingString, out pageNumber);
            if (!parse || pageNumber < 1) return 1;

            return pageNumber;
        }

        public PagedList<T> CreatePagedList<T>(List<T> itemList, int pageNumber)
        {
            if (pageNumber < 1) pageNumber = 1;
            int itemListCount = itemList.Count;
            int pageCount = (itemListCount / itemsPerPage) + 1;

            //List of items will fit onto one page, return itemList
            if (itemListCount <= itemsPerPage) return new PagedList<T>(itemsPerPage, 1, 1, itemList);

            //Page number is higher than the number of possible pages, set pageNumber to the last page.
            if (pageNumber > pageCount) pageNumber = pageCount;

            int firstResult = (pageNumber - 1) * itemsPerPage;
            itemList = itemList.Skip(firstResult).Take(itemsPerPage).ToList();

            return new PagedList<T>(itemsPerPage, pageCount, pageNumber, itemList);
        }
    }
}
