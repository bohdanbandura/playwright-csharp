using Microsoft.Playwright;

namespace PlaywrighNunit.Pages
{
    public class BasePage
    {

        private readonly IPage _page; 

        public BasePage(IPage page) 
        {
            _page = page;
        }
    }
}
