﻿using Bellatrix.Playwright.Enums;
using Bellatrix.Playwright.Settings;
using NUnit.Framework;

namespace Bellatrix.Playwright.GettingStarted;

[TestFixture]

// 1. To execute BELLATRIX tests in Docker containers using Selenoid (https://github.com/aerokube/selenoid), you should use the Selenoid attribute instead of Browser.
// Selenoid has the same parameters as Browser but adds to additional ones-
// browser version, recordVideo, saveSessionLogs and enableVnc.
// As with the Browser attribute you can override the class lifecycle on Test level.
[Selenoid(BrowserChoice.Chrome, "77", Lifecycle.RestartEveryTime, recordVideo: true, enableVnc: true, saveSessionLogs: true)]
public class SelenoidTests : NUnit.WebTest
{
    [Test]
    [Ignore("no need to run")]
    public void PromotionsPageOpened_When_PromotionsButtonClicked()
    {
        App.Navigation.Navigate("https://demos.bellatrix.solutions/");

        var promotionsLink = App.Components.CreateByLinkText<Anchor>("Promotions");

        promotionsLink.Click();
    }

    [Test]
    [Ignore("no need to run")]
    [Selenoid(BrowserChoice.Chrome, "76", DesktopWindowSize._1280_1024, Lifecycle.RestartEveryTime)]

    ////[Selenoid(BrowserChoice.Chrome, "76", 1000, 500, Lifecycle.RestartEveryTime)]
    ////[Selenoid(BrowserChoice.Chrome, "76", MobileWindowSize._320_568, Lifecycle.RestartEveryTime)]
    ////[Selenoid(BrowserChoice.Chrome, "76", TabletWindowSize._600_1024, Lifecycle.RestartEveryTime)]
    public void BlogPageOpened_When_PromotionsButtonClicked()
    {
        App.Navigation.Navigate("https://demos.bellatrix.solutions/");

        var blogLink = App.Components.CreateByLinkText<Anchor>("Blog");

        blogLink.Click();
    }
}