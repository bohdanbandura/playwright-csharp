using Microsoft.Playwright;
using PlaywrighNunit.Locators;


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

            await _page.FillAsync(_login_page_locators.userName, login);

            await _page.FillAsync(_login_page_locators.password, _password);

            await _page.ClickAsync(_login_page_locators.loginButton);
        }
    }
}
