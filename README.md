SimpleMVC4PaginationHtmlHelper
==============================

This is a simple pagination tool for use with ASP.NET MVC4. Rather than build in every option imaginable, it's set up to allow you to use this as a staring point and modify it to your needs.

There are two parts:

* **Controller**:

PaginationService takes a generic list of items from the controller and creates a PagedList object with trimmed list. PagedList has all the information needed for the HtmlHelper in the view.
(itemList is the full list of results to be paged, and model is the view model)

```C#
PaginationService paginationService = new PaginationService();

//Parse the routing string for the page number
int pageNumber = paginationService.ParseUrlRoutingString(Page);

//Create pagedList and add it the viewmodel
model.pagedList = paginationService.CreatePagedList(itemList, pageNumber);
```
 
* **View**:
 
PaginationHtmlHelper creates a with the pagining component.

```razor
 @Html.PaginationHelper(model.pagedList.currentPage, model.pagedList.pageCount, model.pagedList.totalResults)    
```

Result:

![](https://raw2.github.com/Mason240/SimpleMVC4PaginationHtmlHelper/master/PagingScreenshot.PNG)

A working example can be found at [LotrLcg.com](http://www.lotrlcg.com/Card/Search/?Type=Player&searchRadio=All&DisplayList=false&Page=7).



