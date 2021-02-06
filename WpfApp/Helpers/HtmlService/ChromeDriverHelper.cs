using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WpfApp.Helpers.HtmlService
{
    public class ChromeDriverHelper
    {
        public ChromeDriverHelper()
        {
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            Driver = new ChromeDriver(driverService, new ChromeOptions());
        }

        public IWebDriver Driver { get; }

        public void OpenHtmlFile(string filePath)
        {
            Driver.Navigate().GoToUrl(filePath);
        }

        public void StopWebDriver()
        {
            Driver.Quit();
        }
    }
}
