using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace Test;

public class AccessSong
{

    public required IWebDriver driver;
    private void Start()
    {
        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArgument("--user-data-dir=C:/Users/Fagner/AppData/Local/Google/Chrome/User Data/Default");
        chromeOptions.AddArgument("--disable-extensions");
        driver = new ChromeDriver(chromeOptions);
        driver.Navigate().GoToUrl("https://deezer.com");
    }

    public IWebElement GetWebElement(By by, int sec = 10)
    {
        Console.WriteLine("before");
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(sec));
        wait.Until(x => { try { x.FindElement(by); return true; } catch { return false; } });
        return driver.FindElement(by);

    }

    [Fact]
    public void AccessSongBySearch()
    {
        Start();
        var searchBar = GetWebElement(By.Id("topbar-search"));
        searchBar.SendKeys("garçon manqué");
        searchBar.Submit();

        var song = GetWebElement(By.CssSelector(".NkGZL"));
        Assert.True(song.Displayed);
        song.Click();
        Thread.Sleep(1000);
        var name = GetWebElement(By.CssSelector(".css-tadcwa.e1riuwxf1"));
        Assert.Equal(song.Text, name.Text);

        driver.Quit();
    }

    [Fact]
    public void AccessSongBySearchNotFound()
    {
        Start();
        var searchBar = GetWebElement(By.Id("topbar-search"));
        searchBar.SendKeys("mclihbkvgy");
        searchBar.Submit();

        Assert.Throws<WebDriverTimeoutException>(() => GetWebElement(By.CssSelector(".NkGZL"), 5));
        driver.Quit();
    }
}