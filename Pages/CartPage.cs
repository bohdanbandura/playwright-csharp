using Microsoft.Playwright;
using PlaywrighNunit.Locators;

namespace PlaywrighNunit.Pages
{
    public class CartPage
    {
        private readonly IPage _page;
        private readonly CartPageLocators _cart_page_locators = new CartPageLocators();
        private readonly ProductsPageLocators _products_page_locators = new ProductsPageLocators();

        public CartPage(IPage page) 
        {
            _page = page;
        }

        public async Task DeleteProduct()
        {
            await _page.Locator(_products_page_locators.cart).ClickAsync();

            await _page.Locator(_cart_page_locators.removeBtn).ClickAsync();

            var removedItem = _page.Locator(_cart_page_locators.removedItem);

            //await Expect(removedItem).ToBeEmptyAsync();
        }
    }
}
