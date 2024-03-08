﻿// <copyright file="InteractionsService.cs" company="Automate The Planet Ltd.">
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
using Bellatrix.Playwright.Services;
using System.Drawing;

namespace Bellatrix.Playwright;

/// <summary>
/// Provides a mechanism for building advanced interactions with the browser.
/// </summary>
public class InteractionsService : WebService
{
    public InteractionsService(WrappedBrowser wrappedBrowser)
        : base(wrappedBrowser)
    {
        Actions = new();
    }

    public Point Coordinates()
    {
        _ = CurrentPage.EvaluateAsync("document.addEventListener('mousemove', (event) => { window.playwrightMouseX = event.clientX; });").Result;
        _ = CurrentPage.EvaluateAsync("document.addEventListener('mousemove', (event) => { window.playwrightMouseY = event.clientY; });").Result;

        var x = CurrentPage.EvaluateAsync("window.playwrightMouseX").Result.GetValueOrDefault().GetInt32();
        var y = CurrentPage.EvaluateAsync("window.playwrightMouseY").Result.GetValueOrDefault().GetInt32();

        return new Point(x, y);
    }

    public List<Action> Actions { get; }

    /// <summary>
    /// Releases the mouse button at the last known mouse coordinates.
    /// </summary>
    /// <returns>A self-reference to this <see cref="Bellatrix.Playwright.InteractionsService" />.</returns>
    public InteractionsService Release()
    {
        Actions.Add(() => CurrentPage.Mouse.UpAsync().GetAwaiter().GetResult());
        return this;
    }

    /// <summary>Releases the mouse button on the specified element.</summary>
    /// <param name="element">The element on which to release the button.</param>
    /// <returns>A self-reference to this <see cref="Bellatrix.Playwright.InteractionsService" />.</returns>
    public InteractionsService Release(Component element)
    {
        var boundingBox = element.WrappedElement.BoundingBoxAsync().Result;
        Actions.Add(() =>
        {
            CurrentPage.Mouse.MoveAsync(boundingBox.X, boundingBox.Y).GetAwaiter().GetResult();
            CurrentPage.Mouse.UpAsync().GetAwaiter().GetResult();
        });
        return this;
    }

    /// <summary>Sends a modifier key down message to the browser.</summary>
    /// <param name="theKey">The key to be sent.</param>
    /// <returns>A self-reference to this <see cref="Bellatrix.Playwright.InteractionsService" />.</returns>
    public InteractionsService KeyDown(string theKey)
    {
        Actions.Add(() => CurrentPage.Keyboard.DownAsync(theKey).GetAwaiter().GetResult());
        return this;
    }

    /// <summary>
    /// Sends a modifier key down message to the specified element in the browser.
    /// </summary>
    /// <param name="element">The element to which to send the key command.</param>
    /// <param name="theKey">The key to be sent.</param>
    /// <returns>A self-reference to this <see cref="Bellatrix.Playwright.InteractionsService" />.</returns>
    public InteractionsService KeyDown(Component element, string theKey)
    {
        Actions.Add(() => element.WrappedElement.PressAsync(theKey).GetAwaiter().GetResult());
        return this;
    }

    /// <summary>Sends a modifier key up message to the browser.</summary>
    /// <param name="theKey">The key to be sent.</param>
    /// <returns>A self-reference to this <see cref="Bellatrix.Playwright.InteractionsService" />.</returns>
    public InteractionsService KeyUp(string theKey)
    {
        Actions.Add(() => CurrentPage.Keyboard.UpAsync(theKey).GetAwaiter().GetResult());
        return this;
    }

    /// <summary>
    /// Sends a modifier up down message to the specified element in the browser.
    /// </summary>
    /// <param name="element">The element to which to send the key command.</param>
    /// <param name="theKey">The key to be sent.</param>
    /// <returns>A self-reference to this <see cref="Bellatrix.Playwright.InteractionsService" />.</returns>
    public InteractionsService KeyUp(Component element, string theKey)
    {
        //WrappedActions.KeyUp(element.WrappedElement, theKey);
        return this;
    }

    /// <summary>Sends a sequence of keystrokes to the browser.</summary>
    /// <param name="keysToSend">The keystrokes to send to the browser.</param>
    /// <returns>A self-reference to this <see cref="Bellatrix.Playwright.InteractionsService" />.</returns>
    public InteractionsService SendKeys(string keysToSend)
    {
        Actions.Add(() => CurrentPage.Keyboard.TypeAsync(keysToSend).GetAwaiter().GetResult());
        return this;
    }

    /// <summary>
    /// Sends a sequence of keystrokes to the specified element in the browser.
    /// </summary>
    /// <param name="element">The element to which to send the keystrokes.</param>
    /// <param name="keysToSend">The keystrokes to send to the browser.</param>
    /// <returns>A self-reference to this <see cref="Bellatrix.Playwright.InteractionsService" />.</returns>
    public InteractionsService SendKeys(Component element, string keysToSend)
    {
        Actions.Add(() => element.WrappedElement.PressSequentiallyAsync(keysToSend).GetAwaiter().GetResult());
        return this;
    }

    /// <summary>Clicks the mouse at the last known mouse coordinates.</summary>
    /// <returns>A self-reference to this <see cref="Bellatrix.Playwright.InteractionsService" />.</returns>
    public InteractionsService Click()
    {
        var coordinates = Coordinates();
        Actions.Add(() => CurrentPage.Mouse.ClickAsync(coordinates.X, coordinates.Y).GetAwaiter().GetResult());
        return this;
    }

    /// <summary>Clicks the mouse on the specified element.</summary>
    /// <param name="element">The element on which to click.</param>
    /// <returns>A self-reference to this <see cref="Bellatrix.Playwright.InteractionsService" />.</returns>
    public InteractionsService Click(Component element)
    {
        Actions.Add(() => element.WrappedElement.ClickAsync().GetAwaiter().GetResult());
        return this;
    }

    /// <summary>
    /// Clicks and holds the mouse button at the last known mouse coordinates.
    /// </summary>
    /// <returns>A self-reference to this <see cref="Bellatrix.Playwright.InteractionsService" />.</returns>
    public InteractionsService ClickAndHold(int delay = 1000)
    {
        var coordinates = Coordinates();
        Actions.Add(() => CurrentPage.Mouse.ClickAsync(coordinates.X, coordinates.Y, new MouseClickOptions { Delay = delay}).GetAwaiter().GetResult());
        return this;
    }

    /// <summary>
    /// Clicks and holds the mouse button down on the specified element.
    /// </summary>
    /// <param name="element">The element on which to click and hold.</param>
    /// <returns>A self-reference to this <see cref="Bellatrix.Playwright.InteractionsService" />.</returns>
    public InteractionsService ClickAndHold(Component element, int delay = 1000)
    {
        Actions.Add(() => element.WrappedElement.ClickAsync(new LocatorClickOptions { Delay = delay}).GetAwaiter().GetResult());
        return this;
    }

    /// <summary>
    /// Double-clicks the mouse at the last known mouse coordinates.
    /// </summary>
    /// <returns>A self-reference to this <see cref="Bellatrix.Playwright.InteractionsService" />.</returns>
    public InteractionsService DoubleClick()
    {
        var coordinates = Coordinates();
        Actions.Add(() => CurrentPage.Mouse.ClickAsync(coordinates.X, coordinates.Y, new MouseClickOptions { ClickCount = 2 }).GetAwaiter().GetResult());
        return this;
    }

    /// <summary>Double-clicks the mouse on the specified element.</summary>
    /// <param name="element">The element on which to double-click.</param>
    /// <returns>A self-reference to this <see cref="Bellatrix.Playwright.InteractionsService" />.</returns>
    public InteractionsService DoubleClick(Component element)
    {
        Actions.Add(() => element.WrappedElement.DblClickAsync().GetAwaiter().GetResult());
        return this;
    }

    /// <summary>
    /// Right-clicks the mouse at the last known mouse coordinates.
    /// </summary>
    /// <returns>A self-reference to this <see cref="Bellatrix.Playwright.InteractionsService" />.</returns>
    public InteractionsService ContextClick()
    {
        var coordinates = Coordinates();
        Actions.Add(() => CurrentPage.Mouse.ClickAsync(coordinates.X, coordinates.Y, new MouseClickOptions { Button = MouseButton.Right }).GetAwaiter().GetResult());
        return this;
    }

    /// <summary>Right-clicks the mouse on the specified element.</summary>
    /// <param name="element">The element on which to right-click.</param>
    /// <returns>A self-reference to this <see cref="Bellatrix.Playwright.InteractionsService" />.</returns>
    public InteractionsService ContextClick(Component element)
    {
        Actions.Add(() => element.WrappedElement.ClickAsync(new LocatorClickOptions { Button = MouseButton.Right }).GetAwaiter().GetResult());
        return this;
    }

    /// <summary>
    /// Performs a drag-and-drop operation from one element to another.
    /// </summary>
    /// <param name="sourceElement">The element on which the drag operation is started.</param>
    /// <param name="destinationElement">The element on which the drop is performed.</param>
    /// <returns>A self-reference to this <see cref="Bellatrix.Playwright.InteractionsService" />.</returns>
    public InteractionsService DragAndDrop(Component sourceElement, Component destinationElement)
    {
        Actions.Add(() => sourceElement.WrappedElement.DragToAsync(destinationElement.WrappedElement).GetAwaiter().GetResult());
        return this;
    }

    /// <summary>
    /// Performs a drag-and-drop operation on one element to a specified offset.
    /// </summary>
    /// <param name="sourceElement">The element on which the drag operation is started.</param>
    /// <param name="offsetX">The horizontal offset to which to move the mouse.</param>
    /// <param name="offsetY">The vertical offset to which to move the mouse.</param>
    /// <returns>A self-reference to this <see cref="Bellatrix.Playwright.InteractionsService" />.</returns>
    public InteractionsService DragAndDrop(Component sourceElement, int offsetX, int offsetY)
    {
        var boundingBox = sourceElement.WrappedElement.BoundingBoxAsync().Result;
        Actions.Add(() =>
        {
            sourceElement.WrappedElement.ClickAsync(new LocatorClickOptions { Delay = 1000 }).GetAwaiter().GetResult();
            CurrentPage.Mouse.MoveAsync(boundingBox.X + offsetX, boundingBox.Y + offsetY).GetAwaiter().GetResult();
        });
        return this;
    }

    /// <summary>Moves the mouse to the specified element.</summary>
    /// <param name="element">The element to which to move the mouse.</param>
    /// <returns>A self-reference to this <see cref="Bellatrix.Playwright.InteractionsService" />.</returns>
    public InteractionsService MoveToElement(Component element)
    {
        var boundingBox = element.WrappedElement.BoundingBoxAsync().Result;
        Actions.Add(() => CurrentPage.Mouse.MoveAsync(boundingBox.X, boundingBox.Y).GetAwaiter().GetResult());
        return this;
    }

    /// <summary>
    /// Moves the mouse to the specified offset of the top-left corner of the specified element.
    /// </summary>
    /// <param name="sourceElement">The element to which to move the mouse.</param>
    /// <param name="offsetX">The horizontal offset to which to move the mouse.</param>
    /// <param name="offsetY">The vertical offset to which to move the mouse.</param>
    /// <returns>A self-reference to this <see cref="Bellatrix.Playwright.InteractionsService" />.</returns>
    public InteractionsService MoveToElement(Component sourceElement, int offsetX, int offsetY)
    {
        var boundingBox = sourceElement.WrappedElement.BoundingBoxAsync().Result;
        Actions.Add(() => CurrentPage.Mouse.MoveAsync(boundingBox.X + offsetX, boundingBox.Y + offsetY).GetAwaiter().GetResult());
        return this;
    }

    /// <summary>
    /// Moves the mouse to the specified offset of the last known mouse coordinates.
    /// </summary>
    /// <param name="offsetX">The horizontal offset to which to move the mouse.</param>
    /// <param name="offsetY">The vertical offset to which to move the mouse.</param>
    /// <returns>A self-reference to this <see cref="Bellatrix.Playwright.InteractionsService" />.</returns>
    public InteractionsService MoveByOffset(int offsetX, int offsetY)
    {
        var currentCoordinates = Coordinates();
        Actions.Add(() => CurrentPage.Mouse.MoveAsync(currentCoordinates.X + offsetX, currentCoordinates.Y + offsetY).GetAwaiter().GetResult());
        return this;
    }

    public void Perform()
    {
        foreach(var action in Actions) action();
    }
}