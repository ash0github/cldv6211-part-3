using Azure.Search.Documents.Models;

namespace KhumaloCraft.Models
{
    public class SearchData
    {
        public string searchText { get; set; }
        public SearchResults<ProductInformation> resultList;
    }
}
