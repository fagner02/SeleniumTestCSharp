using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
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
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(sec));
        wait.Until(x => { try { x.FindElement(by); return true; } catch { return false; } });
        return driver.FindElement(by);

    }

    public List<IWebElement> GetWebElements(By by, int sec = 10)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(sec));
        wait.Until(x => { try { x.FindElement(by); return true; } catch { return false; } });
        return driver.FindElements(by).ToList();

    }

    [Fact]
    public void AccessArtistBySearch()
    {
        Start();
        var searchBar = GetWebElement(By.Id("topbar-search"));
        searchBar.SendKeys("Sia");
        searchBar.Submit();

        Thread.Sleep(2000);
        var container = GetWebElement(By.CssSelector(".search-top-result"));

        Assert.NotNull(container);
        Assert.True(container.Displayed);
        Thread.Sleep(2000);
        var profile = container.FindElement(By.ClassName("heading-3"));
        var name = profile.Text;

        profile.FindElement(By.TagName("a")).Click();

        container = GetWebElements(By.CssSelector(".container"))[0];
        var title = container.FindElement(By.ClassName("chakra-heading"));
        Assert.Equal(title.Text, name);
        Assert.True(title.Displayed);
        driver.Quit();
    }

    [Fact]
    public void FollowProfile()
    {
        Start();
        var searchBar = GetWebElement(By.Id("topbar-search"));
        searchBar.SendKeys("Beatriz");
        searchBar.Submit();

        Thread.Sleep(1000);
        var container = GetWebElements(By.CssSelector(".thumbnail-grid.thumbnail-grid-responsive.thumbnail-grid-one-line"))[4];

        Assert.NotNull(container);
        Assert.True(container.Displayed);
        var profile = container.FindElement(By.ClassName("heading-4"));
        var name = profile.Text;

        Actions actions = new(driver);
        actions.MoveToElement(profile);
        actions.KeyDown(Keys.PageDown);
        actions.Perform();
        profile.Click();

        container = GetWebElements(By.CssSelector(".container"))[0];
        var title = container.FindElement(By.ClassName("chakra-heading"));
        Assert.Equal(title.Text, name);

        var btn = container.FindElement(By.CssSelector(".states-button-action"));
        actions.Reset();
        actions.KeyDown(Keys.Down);
        actions.KeyDown(Keys.Down);
        actions.Perform();

        btn.Click();
        Thread.Sleep(1000);
        GetWebElement(By.CssSelector("[data-testid=\"user-follow-button\"]")).Click();
        Thread.Sleep(1000);
        driver.Quit();
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

    [Fact]
    public void OpenLyrics()
    {
        Start();

        var searchBar = GetWebElement(By.Id("topbar-search"));
        searchBar.SendKeys("garçon manqué");
        searchBar.Submit();

        var song = GetWebElement(By.CssSelector(".NkGZL"));
        Assert.True(song.Displayed);
        song.Click();
        Thread.Sleep(1000);

        var btn = GetWebElement(By.CssSelector("[data-testid=\"lyrics_button\"]"));

        btn.Click();
        var lyrics = GetWebElement(By.ClassName("player-lyrics-full"));
        Assert.True(lyrics.Displayed);
        driver.Quit();
    }

    [Fact]
    public void AccessPlaylistFromHome()
    {
        Start();

        var container = GetWebElements(By.CssSelector(".carousel"));
        var playlist = container[2].FindElement(By.ClassName("thumbnail"));
        Actions actions = new(driver);
        actions.MoveToElement(playlist);
        actions.KeyDown(Keys.Down);
        actions.Perform();
        playlist.Click();

        driver.Quit();
    }

    [Fact]
    public void AddSongToPlaylist()
    {
        Start();
        GetWebElement(By.CssSelector("[data-testid=\"add_to_playlist_button\"]")).Click();
        var container = GetWebElement(By.CssSelector(".popper"));
        Assert.True(container.Displayed);
        var playlist = container.FindElements(By.TagName("button")).FirstOrDefault(x => x.Text.Contains("Test"));
        Assert.NotNull(playlist);
        Assert.True(playlist.Displayed);
        playlist.Click();

        driver.Quit();
    }

    [Fact]
    public void ShareSongFromPlaylist()
    {
        Start();

        var container = GetWebElement(By.CssSelector(".css-11zfhyy"));
        Thread.Sleep(1000);
        container.FindElement(By.CssSelector(".css-1oaqkcs:nth-child(2)")).Click();
        Thread.Sleep(1000);
        Actions actions = new(driver);
        actions.KeyDown(Keys.PageDown);
        actions.Perform();
        Thread.Sleep(2000);

        GetWebElement(By.ClassName("Ledev")).FindElement(By.ClassName("popper-wrapper")).Click();
        GetWebElements(By.CssSelector(".KUMiD"))[8].Click();
        Thread.Sleep(5000);
        GetWebElement(By.ClassName("notice")).Click();
        driver.Quit();
    }
}