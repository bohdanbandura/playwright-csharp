using Microsoft.Playwright;
using PlaywrighNunit.Locators;
using Microsoft.Playwright.NUnit;
using NUnit.Framework.Legacy;
using PlaywrighNunit.Pages;
using PlaywrighNunit.Helpers;

namespace PlaywrighNunit
{
    public class PlaywrighNunitTests : PageTest
    {
        private LoginPage _loginPage;
        private CartPage _cartPage;
        private ProductsPage _productsPage;
        private readonly LoginPageLocators _login_page_locators = new();
        private readonly ProductsPageLocators _products_page_locators = new();
        private readonly CartPageLocators _cart_page_locators = new();
        private readonly string _locked_user_login = "locked_out_user";

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

        [SetUp]
        public async Task Setup()
        {
            await Context.Tracing.StartAsync(new()
            {
                Title = TestContext.CurrentContext.Test.ClassName + "." + TestContext.CurrentContext.Test.Name,
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });

            _loginPage = new LoginPage(Page);
            _cartPage = new CartPage(Page);
            _productsPage = new ProductsPage(Page);
        }

        [TearDown]
        public async Task TearDown()
        {
            await Context.Tracing.StopAsync(new() 
            { 
                Path = Path.Combine("../../../playwright-traces", $"{TestContext.CurrentContext.Test.ClassName}.{TestContext.CurrentContext.Test.Name}.zip")
            });
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

            Assert.That(productNamesZA, Is.EqualTo(productNamesAZ.Reverse()).AsCollection);
        }

        [Test]
        public async Task CheckLockedUser()
        {
            await _loginPage.Login(_locked_user_login);

            var errorMessage = Page.Locator(_login_page_locators.errorMessage);

            await Expect(errorMessage).ToHaveTextAsync("Epic sadface: Sorry, this user has been locked out.");
        }

        [Test]
        public async Task CompareScreenshots()
        {
            await Page.GotoAsync("https://www.google.com");

            await Page.ScreenshotAsync(new() { Path = "../../../screenshots/screenshot1.png" });

            await Page.Locator("textarea[name=\"q\"]").FillAsync("qqqq");

            await Page.ScreenshotAsync(new() { Path = "../../../screenshots/screenshot2.png" });

            await Page.PressAsync("textarea[name=\"q\"]", "Enter");

            await Page.ScreenshotAsync(new() { Path = "../../../screenshots/screenshot3.png" });

            await Page.Locator("h3", new PageLocatorOptions { HasText = "QQQQ: Definition, Composition, and Current Ticker" }).First.ClickAsync();

            await Page.ScreenshotAsync(new() { Path = "../../../screenshots/screenshot4.png" });

            await Page.Locator("li[class=\"header-nav__list-item\"]", new PageLocatorOptions { HasText = "News"}).ClickAsync();

            await Page.ScreenshotAsync(new() { Path = "../../../screenshots/screenshot5.png" });

            ImageComparer.CompareImages("../../../expectedScreenshots/expectedScreenshot1.png", "../../../screenshots/screenshot1.png", "../../../diffScreenshots/diffScreenshot1.png");
            ImageComparer.CompareImages("../../../expectedScreenshots/expectedScreenshot2.png", "../../../screenshots/screenshot2.png", "../../../diffScreenshots/diffScreenshot2.png");
            ImageComparer.CompareImages("../../../expectedScreenshots/expectedScreenshot3.png", "../../../screenshots/screenshot3.png", "../../../diffScreenshots/diffScreenshot3.png");
            ImageComparer.CompareImages("../../../expectedScreenshots/expectedScreenshot4.png", "../../../screenshots/screenshot4.png", "../../../diffScreenshots/diffScreenshot4.png");
            ImageComparer.CompareImages("../../../expectedScreenshots/expectedScreenshot5.png", "../../../screenshots/screenshot5.png", "../../../diffScreenshots/diffScreenshot5.png");
        }
    }
}