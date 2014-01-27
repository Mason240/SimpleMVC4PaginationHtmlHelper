using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Pagination
{
    public static class PaginationHtmlHelper
    {
        private const int PAGES_DISPLAYED = 9;
        private const string PAGE_URL_PARAMATER = "Page";

        private const string CSS_CONTAINER = "pagination-container";
        private const string CSS_CURRENT = "pagination-current";
        private const string CSS_ELLIPSES = "pagination-ellipses";
        private const string CSS_FIRST = "pagination-first";
        private const string CSS_LAST = "pagination-last";

        /// <summary>
        /// Creates a div with links for pagination.</summary>
        /// <param name="helper"></param>
        /// <param name="currentPage">The current page number.</param>
        /// <param name="totalPageCount">The total number of pages.</param>
        /// <returns> div filled with links </returns>
        public static MvcHtmlString PaginationHelper(this HtmlHelper helper, int currentPageNumber, int totalPageCount)
        {
            return PaginationHelper(helper, currentPageNumber, totalPageCount, PAGES_DISPLAYED, PAGE_URL_PARAMATER);
        }

        /// <summary>
        /// Creates a div with links for pagination.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="currentPage">The current page number.</param>
        /// <param name="totalPageCount">The total number of pages.</param>
        /// <param name="pagesDisplayed">The number of pages to be displayed in the middle section. Should be odd.</param>
        /// <param name="pageUrlParamater">The name of the URL paramater representing the page number. Typically 'Page'</param>
        /// <returns></returns>
        public static MvcHtmlString PaginationHelper(this HtmlHelper helper, int currentPage, int totalPageCount, int pagesDisplayed, string pageUrlParamater)
        {

            if (totalPageCount == 1) return new MvcHtmlString("");
            if (pagesDisplayed < 3) pagesDisplayed = 3; //Validate to make sure pagesDisplayed is greater than 3
            string url = createURL(helper.ViewContext.HttpContext.Request.Url, pageUrlParamater);

            StringBuilder builder = new StringBuilder();

            //Open tag for containing div
            builder.Append("<div class='" + CSS_CONTAINER + "'>");

            //Create first link
            builder.Append("<a href='" + url + "1' class='" + (currentPage == 1 ? CSS_CURRENT : "") + " " + CSS_FIRST + "' >First</a>");

            //Starting and stopping values for the middle section of links
            int startMiddle = (currentPage > (pagesDisplayed + 1) ? (currentPage - pagesDisplayed) : 2);
            int stopMiddle = ((currentPage + pagesDisplayed) < totalPageCount ? (currentPage + pagesDisplayed) : (totalPageCount - 1));

            //Ellipeses
            if ((startMiddle > 2) && (totalPageCount > 2)) builder.Append("<div class='" + CSS_ELLIPSES + "'> ... </div>");

            //Build middle links
            for (int i = startMiddle; i < stopMiddle + 1; i++)
            {
                builder.Append("<a class='" + (currentPage == i ? CSS_CURRENT : "") + "' href=" + url + i + ">" + i + "</a>");
            }

            //Ellipeses
            if ((stopMiddle != (totalPageCount - 1)) && (totalPageCount > 2)) builder.Append("<div class='" + CSS_ELLIPSES + "'> ... </div>");

            //Create last link
            builder.Append("<a href='" + url + totalPageCount + "' class='" + (currentPage == totalPageCount ? CSS_CURRENT : "") + " " + CSS_LAST + "' >Last</a>");

            //Closing tag for containing div
            builder.Append("</div>");

            return new MvcHtmlString(builder.ToString());
        }


        //Uses the URI from the current page to create the URL used for the paging links;
        //The Page paramter is removed and then added back in empty, at the end of the string
        //ie: http://www.lotrlcg.com/Card/Search?Terms=&Type=Ally&Page=
        //The Page value is then added at each step in PaginationHelper method 
        private static string createURL(Uri urlObj, string pageUrlParamater)
        {
            //Get the query paramaters,removing the page paramater 
            NameValueCollection nameValues = HttpUtility.ParseQueryString(urlObj.Query.ToString());
            nameValues.Remove(pageUrlParamater);

            //Get routing part of URI
            string urlBase = urlObj.ToString().Split('?')[0];

            //Combine and return
            return urlBase + "?" + nameValues.ToString() + "&" + pageUrlParamater + "=";
        }

    }
}
