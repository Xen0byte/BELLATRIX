﻿// <copyright file="AndroidApp.cs" company="Automate The Planet Ltd.">
// Copyright 2021 Automate The Planet Ltd.
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
using Bellatrix.Mobile.EventHandlers.Android;
using Bellatrix.Mobile.Services;
using Bellatrix.Mobile.Services.Android;
using OpenQA.Selenium.Appium.Android;

namespace Bellatrix.Mobile
{
    public class AndroidApp : App<AndroidDriver<AndroidElement>, AndroidElement>
    {
        public AndroidAppService AppService => ServicesCollection.Current.Resolve<AndroidAppService>();

        [Obsolete("FileSystemService is deprecated use Files property instead.")]
        public AndroidFileSystemService FileSystemService => ServicesCollection.Current.Resolve<AndroidFileSystemService>();
        public AndroidFileSystemService Files => ServicesCollection.Current.Resolve<AndroidFileSystemService>();

        [Obsolete("DeviceService is deprecated use Device property instead.")]
        public AndroidDeviceService DeviceService => ServicesCollection.Current.Resolve<AndroidDeviceService>();
        public AndroidDeviceService Device => ServicesCollection.Current.Resolve<AndroidDeviceService>();

        [Obsolete("KeyboardService is deprecated use Keyboard property instead.")]
        public AndroidKeyboardService KeyboardService => ServicesCollection.Current.Resolve<AndroidKeyboardService>();
        public AndroidKeyboardService Keyboard => ServicesCollection.Current.Resolve<AndroidKeyboardService>();

        [Obsolete("TouchActionsService is deprecated use TouchActions property instead.")]
        public TouchActionsService<AndroidDriver<AndroidElement>, AndroidElement> TouchActionsService => ServicesCollection.Current.Resolve<TouchActionsService<AndroidDriver<AndroidElement>, AndroidElement>>();
        public TouchActionsService<AndroidDriver<AndroidElement>, AndroidElement> TouchActions => ServicesCollection.Current.Resolve<TouchActionsService<AndroidDriver<AndroidElement>, AndroidElement>>();

        public override void Dispose()
        {
            DisposeDriverService.DisposeAllAndroid();
            GC.SuppressFinalize(this);
        }

        public void AddElementEventHandler<TComponentsEventHandler>()
           where TComponentsEventHandler : ComponentEventHandlers
        {
            var elementEventHandler = (TComponentsEventHandler)Activator.CreateInstance(typeof(TComponentsEventHandler));
            elementEventHandler.SubscribeToAll();
        }

        public void RemoveElementEventHandler<TComponentsEventHandler>()
            where TComponentsEventHandler : ComponentEventHandlers
        {
            var elementEventHandler = (TComponentsEventHandler)Activator.CreateInstance(typeof(TComponentsEventHandler));
            elementEventHandler.UnsubscribeToAll();
        }
    }
}
