using Microsoft.Playwright;
using PlaywrighNunit.Locators;
using PlaywrighNunit.Helpers;

namespace PlaywrighNunit.Pages
{
    public class LoginPage
    {
        private readonly string _base_url = "https://www.saucedemo.com/";
        private readonly LoginPageLocators _login_page_locators = new LoginPageLocators();
        private readonly string _login = "standard_user";
        private readonly string _password = "secret_sauce";
        private readonly IPage _page;
        
        public LoginPage(IPage page)
        {
            _page = page;
        }

        public async Task Login()
        {
            await Login(_login);
        }

        public async Task Login(string login)
        {
            await _page.GotoAsync(_base_url);

            await _page.ScreenshotAsync(new() { Path = "../../../screenshots/screenshot1.png" });

            await _page.FillAsync(_login_page_locators.userName, login);

            await _page.ScreenshotAsync(new() { Path = "../../../screenshots/screenshot2.png" });

            await _page.FillAsync(_login_page_locators.password, _password);

            await _page.ScreenshotAsync(new() { Path = "../../../screenshots/screenshot3.png" });

            await _page.ClickAsync(_login_page_locators.loginButton);

            await _page.ScreenshotAsync(new() { Path = "../../../screenshots/screenshot4.png" });

            ImageComparer.CompareImages("../../../expectedScreenshots/expectedScreenshot1.png", "../../../screenshots/screenshot1.png", "../../../diffScreenshots/diffScreenshot1.png");
            ImageComparer.CompareImages("../../../expectedScreenshots/expectedScreenshot2.png", "../../../screenshots/screenshot2.png", "../../../diffScreenshots/diffScreenshot2.png");
            ImageComparer.CompareImages("../../../expectedScreenshots/expectedScreenshot3.png", "../../../screenshots/screenshot3.png", "../../../diffScreenshots/diffScreenshot3.png");
            ImageComparer.CompareImages("../../../expectedScreenshots/expectedScreenshot4.png", "../../../screenshots/screenshot4.png", "../../../diffScreenshots/diffScreenshot4.png");
        }
    }
}
