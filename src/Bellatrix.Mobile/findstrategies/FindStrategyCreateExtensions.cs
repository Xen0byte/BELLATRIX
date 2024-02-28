﻿// <copyright file="ByCreateExtensions.cs" company="Automate The Planet Ltd.">
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
// <author>Anton Angelov</author>
// <site>https://bellatrix.solutions/</site>
using Bellatrix.Mobile.Controls.Core;
using Bellatrix.Mobile.Core;
using Bellatrix.Mobile.Locators;
using OpenQA.Selenium.Appium;

namespace Bellatrix.Mobile.SytaxSugar;

public static class FindStrategyCreateExtensions
{
    public static TComponent Create<TComponent, TBy, TDriver, TDriverElement>(this TBy by)
        where TComponent : Component<TDriver, TDriverElement>
        where TBy : FindStrategy<TDriver, TDriverElement>
        where TDriver : AppiumDriver
        where TDriverElement : AppiumElement
    {
        var elementRepository = ServicesCollection.Current.Resolve<ComponentCreateService>();
        return elementRepository.Create<TComponent, TBy, TDriver, TDriverElement>(by);
    }

    public static ComponentsList<TComponent, TBy, TDriver, TDriverElement> CreateAll<TComponent, TBy, TDriver, TDriverElement>(this TBy by)
        where TComponent : Component<TDriver, TDriverElement>
        where TBy : FindStrategy<TDriver, TDriverElement>
        where TDriver : AppiumDriver
        where TDriverElement : AppiumElement
    {
        var elementRepository = ServicesCollection.Current.Resolve<ComponentCreateService>();
        return elementRepository.CreateAll<TComponent, TBy, TDriver, TDriverElement>(by);
    }
}
