using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using PlaywrighNunit.Locators;

namespace PlaywrighNunit.Pages
{
    public class ProductsPage
    {

        private readonly IPage _page;
        private readonly ProductsPageLocators _products_page_locators = new ProductsPageLocators();

        public ProductsPage(IPage page) 
        {
            _page = page;
        }

        public async Task<string[]> GetProducts()
        {
            var products = await _page.Locator(_products_page_locators.productBlock).AllAsync();

            List<string> productNames = new List<string>();

            foreach (var product in products)
            {
                string productName = await product.Locator(_products_page_locators.productName).TextContentAsync();
                productNames.Add(productName);
            }

            return productNames.ToArray();
        }

    }
}
