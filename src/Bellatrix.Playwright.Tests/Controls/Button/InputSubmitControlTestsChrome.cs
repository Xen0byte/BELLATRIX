﻿// <copyright file="InputSubmitControlTestsChrome.cs" company="Automate The Planet Ltd.">
// Copyright 2024 Automate The Planet Ltd.
// Licensed under the Apache License, Version 2.0 (the "License");
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// <author>Miriam Kyoseva</author>
// <site>https://bellatrix.solutions/</site>
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bellatrix.Playwright.Tests.Controls;

[TestClass]
[Browser(BrowserTypes.Chrome, Lifecycle.ReuseIfStarted)]
[AllureSuite("Input Button Control")]
public class InputSubmitControlTestsChrome : MSTest.WebTest
{
    public override void TestInit() => App.Navigation.NavigateToLocalPage(ConfigurationService.GetSection<TestPagesSettings>().ButtonPage);

    [TestMethod]
    [TestCategory(Categories.Chrome), TestCategory(Categories.Windows), TestCategory(Categories.OSX)]
    public void SetTextToStop_When_UseClickMethod_Chrome()
    {
        var buttonElement = App.Components.CreateById<Button>("myButton4");

        buttonElement.Click();

        Assert.AreEqual("Stop", buttonElement.Value);
    }

    [TestMethod]
    [TestCategory(Categories.Chrome), TestCategory(Categories.Windows), TestCategory(Categories.OSX)]
    public void ReturnRed_When_Hover_Chrome()
    {
        var buttonElement = App.Components.CreateById<Button>("myButton5");

        buttonElement.Hover();

        Assert.AreEqual("color: red;", buttonElement.GetStyle());
    }

    [TestMethod]
    [TestCategory(Categories.Chrome), TestCategory(Categories.Windows), TestCategory(Categories.OSX)]
    public void ReturnBlue_When_Focus_Chrome()
    {
        var buttonElement = App.Components.CreateById<Button>("myButton6");

        buttonElement.Focus();

        Assert.AreEqual("color: blue;", buttonElement.GetStyle());
    }

    [TestMethod]
    [TestCategory(Categories.Chrome), TestCategory(Categories.Windows), TestCategory(Categories.OSX)]
    public void ReturnFalse_When_DisabledAttributeNotPresent_Chrome()
    {
        var buttonElement = App.Components.CreateById<Button>("myButton4");

        bool isDisabled = buttonElement.IsDisabled;

        Assert.IsFalse(isDisabled);
    }

    [TestMethod]
    [TestCategory(Categories.Chrome), TestCategory(Categories.Windows), TestCategory(Categories.OSX)]
    public void ReturnTrue_When_DisabledAttributePresent_Chrome()
    {
        var buttonElement = App.Components.CreateById<Button>("myButton7");

        bool isDisabled = buttonElement.IsDisabled;

        Assert.IsTrue(isDisabled);
    }

    [TestMethod]
    [TestCategory(Categories.Chrome), TestCategory(Categories.Windows), TestCategory(Categories.OSX)]
    public void ReturnStart_When_ValueAttributePresent_Chrome()
    {
        var buttonElement = App.Components.CreateById<Button>("myButton4");

        var actualValue = buttonElement.Value;

        Assert.AreEqual("Start", actualValue);
    }

    [TestMethod]
    [TestCategory(Categories.Chrome), TestCategory(Categories.Windows), TestCategory(Categories.OSX)]
    public void ReturnEmpty_When_UseInnerText_Chrome()
    {
        var buttonElement = App.Components.CreateById<Button>("myButton4");

        Assert.AreEqual(string.Empty, buttonElement.InnerText);
    }
}