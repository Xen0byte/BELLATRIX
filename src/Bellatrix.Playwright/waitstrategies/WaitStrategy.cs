﻿// <copyright file="BaseUntil.cs" company="Automate The Planet Ltd.">
// Copyright 2022 Automate The Planet Ltd.
// Licensed under the Apache License, Version 2.0 (the "License");
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// <author>Anton Angelov</author>
// <site>https://bellatrix.solutions/</site>
using Bellatrix.Playwright.Services.Browser;
using Bellatrix.Playwright.Settings.Extensions;
using Bellatrix.Playwright.Settings;

namespace Bellatrix.Playwright.Untils;

public abstract class WaitStrategy
{
    protected WaitStrategy(int? timeoutInterval = null, int? sleepInterval = null)
    {
        WrappedBrowser = ServicesCollection.Current.Resolve<WrappedBrowser>();
        TimeoutInterval = timeoutInterval;
        SleepInterval = sleepInterval ?? ConfigurationService.GetSection<WebSettings>().TimeoutSettings.InMilliseconds().SleepInterval;
    }

    protected WrappedBrowser WrappedBrowser { get; }

    protected int? TimeoutInterval { get; set; }

    protected int? SleepInterval { get; }

    public abstract void WaitUntil<TBy>(TBy by)
        where TBy : FindStrategy;

    public abstract void WaitUntil<TBy>(TBy by, Component parent)
        where TBy : FindStrategy;

    protected ILocator FindElement<TBy>(ILocator searchContext, TBy by)
        where TBy : FindStrategy
    {
        var nativeElementFinder = new NativeElementFinderService(searchContext);
        var element = nativeElementFinder.Find(by);
        return element;
    }

    protected ILocator FindElement<TBy>(IPage searchContext, TBy by)
        where TBy : FindStrategy
    {
        var nativeElementFinder = new NativeElementFinderService(searchContext);
        var element = nativeElementFinder.Find(by);
        return element;
    }

    protected ILocator FindElement<TBy>(WrappedBrowser searchContext, TBy by)
        where TBy : FindStrategy
    {
        var nativeElementFinder = new NativeElementFinderService(searchContext);
        var element = nativeElementFinder.Find(by);
        return element;
    }
}