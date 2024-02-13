﻿// <copyright file="IOSSauceLabsAttribute.cs" company="Automate The Planet Ltd.">
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
using System.Reflection;
using Bellatrix.Mobile.Plugins.Attributes;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;

namespace Bellatrix.Mobile;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class IOSSauceLabsAttribute : SauceLabsAttribute, IAppiumOptionsFactory
{
    public IOSSauceLabsAttribute(
        string appPath,
        string platformVersion,
        string deviceName,
        Lifecycle behavior = Lifecycle.NotSet,
        bool recordVideo = false,
        bool recordScreenshots = false)
        : base(appPath, platformVersion, deviceName, behavior, recordVideo, recordScreenshots)
    {
        AppConfiguration.MobileOSType = MobileOSType.IOS;
        AppConfiguration.PlatformName = "iOS";
    }

    public new AppiumOptions CreateAppiumOptions(MemberInfo memberInfo, Type testClassType)
    {
        var appiumOptions = base.CreateAppiumOptions(memberInfo, testClassType);
        appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, AppConfiguration.PlatformName);
        appiumOptions.AddAdditionalCapability("deviceOrientation", "portrait");
        appiumOptions.AddAdditionalCapability("browserName", string.Empty);

        return appiumOptions;
    }
}