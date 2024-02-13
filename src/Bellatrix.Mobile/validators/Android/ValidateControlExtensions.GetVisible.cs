﻿// <copyright file="ValidateControlExtensions.GetVisible.cs" company="Automate The Planet Ltd.">
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
using System;
using Bellatrix.Mobile.Contracts;
using Bellatrix.Mobile.Events;
using OpenQA.Selenium.Appium.Android;

namespace Bellatrix.Mobile.Android;

public static partial class ValidateControlExtensions
{
    public static void ValidateIsVisible<T>(this T control, int? timeout = null, int? sleepInterval = null)
        where T : IComponentVisible, IComponent<AndroidElement>
    {
        ValidateControlWaitService.WaitUntil<AndroidDriver<AndroidElement>, AndroidElement>(() => control.IsVisible.Equals(true), "The control should be visible but was NOT.", timeout, sleepInterval);
        ValidatedIsVisibleEvent?.Invoke(control, new ComponentActionEventArgs<AndroidElement>(control));
    }

    public static void ValidateIsNotVisible<T>(this T control, int? timeout = null, int? sleepInterval = null)
        where T : IComponentVisible, IComponent<AndroidElement>
    {
        ValidateControlWaitService.WaitUntil<AndroidDriver<AndroidElement>, AndroidElement>(() => !control.IsVisible.Equals(true), "The control should be NOT visible but was NOT.", timeout, sleepInterval);
        ValidatedIsNotVisibleEvent?.Invoke(control, new ComponentActionEventArgs<AndroidElement>(control));
    }

    public static event EventHandler<ComponentActionEventArgs<AndroidElement>> ValidatedIsVisibleEvent;
    public static event EventHandler<ComponentActionEventArgs<AndroidElement>> ValidatedIsNotVisibleEvent;
}