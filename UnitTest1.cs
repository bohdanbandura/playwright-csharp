using Microsoft.Playwright;
using PlaywrighNunit.Locators;
using Microsoft.Playwright.NUnit;
using NUnit.Framework.Legacy;
using PlaywrighNunit.Pages;

namespace PlaywrighNunit
{
    public class PlaywrighNunitTests : PageTest
    {
        private LoginPage _loginPage;
        private CartPage _cartPage;
        private ProductsPage _productsPage;
        private readonly ProductsPageLocators _products_page_locators = new ProductsPageLocators();
        private readonly CartPageLocators _cart_page_locators = new CartPageLocators();
        private readonly string _locked_user_login = "locked_out_user";

        [SetUp]
        public void Setup()
        {
            _loginPage = new LoginPage(Page);
            _cartPage = new CartPage(Page);
            _productsPage = new ProductsPage(Page);
        }

        public override BrowserNewContextOptions ContextOptions()
        {
            return new BrowserNewContextOptions()
            {
                RecordVideoDir = "../../../videos/",
                RecordVideoSize = new RecordVideoSize
                {
                    Height = 720,
                    Width = 1280
                }
            };
        }

        [Test]
        public async Task AddProductToTheCart()
        {
            await _loginPage.Login();

            var product = Page.Locator(_products_page_locators.productBlock, new PageLocatorOptions { HasText = "Sauce Labs Backpack" });

            var addToCart = product.Locator(_products_page_locators.addToCart);

            await Expect(addToCart).ToBeEnabledAsync();

            await addToCart.ClickAsync();

            var cartBadge = Page.Locator(_products_page_locators.cartBadge);

            await Expect(cartBadge).ToBeVisibleAsync();

            await Expect(cartBadge).ToHaveTextAsync("1");
        }

        [Test]
        public async Task DeleteProductFromTheCart()
        {
            await this.AddProductToTheCart();

            await Page.Locator(_products_page_locators.cart).ClickAsync();

            await Page.Locator(_cart_page_locators.removeBtn).ClickAsync();

            var removedItem = Page.Locator(_cart_page_locators.removedItem);

            await Expect(removedItem).ToBeEmptyAsync();
        }

        [Test]
        public async Task SortingProducts()
        {
            await _loginPage.Login();

            string[] productNamesAZ = await _productsPage.GetProducts();

            var sortingDropdown = Page.Locator(_products_page_locators.sortingDropDown);

            await sortingDropdown.SelectOptionAsync(new[] { "Name (Z to A)" });

            string[] productNamesZA = await _productsPage.GetProducts();

            CollectionAssert.AreEqual(productNamesAZ.Reverse(), productNamesZA);
        }

        [Test]
        public async Task CheckLockedUser()
        {
            await _loginPage.Login(_locked_user_login);

            var errorMessage = Page.Locator("h3[data-test=\"error\"]");

            await Expect(errorMessage).ToHaveTextAsync("Epic sadface: Sorry, this user has been locked out.");
        }
    }
}