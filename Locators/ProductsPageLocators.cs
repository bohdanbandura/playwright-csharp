using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrighNunit.Locators
{
    public class ProductsPageLocators
    {
        public string title = "span[data-test=\"title\"]";
        public string productBlock = "div[data-test=\"inventory-item\"]";
        public string productName = "div[data-test=\"inventory-item-name\"]";
        public string addToCart = "#add-to-cart-sauce-labs-backpack";
        public string cart = "#shopping_cart_container";
        public string cartBadge = "span[data-test=\"shopping-cart-badge\"]";
        public string sortingDropDown = "select[data-test=\"product-sort-container\"]";
    }
}
