using System.Collections.Generic;

namespace ProductUI.Models
{
    public class ProductListModel
    {
        public IEnumerable<ProductViewModel> Products{ get; set; }
        public string SearchTerm { get; set; }
    }
}
