﻿using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.DevTools;
using DevToolsSessionDomains = OpenQA.Selenium.DevTools.V104.DevToolsSessionDomains;
using OpenQA.Selenium.DevTools.V104.DOMSnapshot;
using OpenQA.Selenium.DevTools.V104.Network;
using EnableCommandSettings = OpenQA.Selenium.DevTools.V104.Network.EnableCommandSettings;
using OpenQA.Selenium.DevTools.V104.Security;
using OpenQA.Selenium.DevTools.V104.Emulation;
using SetUserAgentOverrideCommandSettings = OpenQA.Selenium.DevTools.V104.Network.SetUserAgentOverrideCommandSettings;
using OpenQA.Selenium.DevTools.V104.Console;
using OpenQA.Selenium.DevTools.V104.Performance;
using Bellatrix.Assertions;
using System.Collections.Concurrent;

namespace Bellatrix.Web;

public class DevToolsService : WebService, IDisposable
{
    public DevToolsService(IWebDriver wrappedDriver)
        : base(wrappedDriver)
    {
        DevToolsSession = ((IDevTools)wrappedDriver).GetDevToolsSession();
        DevToolsSessionDomains = DevToolsSession.GetVersionSpecificDomains<DevToolsSessionDomains>();
    }

    public DevToolsSessionDomains DevToolsSessionDomains { get; set; }
    public DevToolsSession DevToolsSession { get; set; }
    public List<NetworkRequestSentEventArgs> RequestsHistory { get; set; }
    public List<NetworkResponseReceivedEventArgs> ResponsesHistory { get; set; }

    public void StartMonitoringBiDiApi()
    {
        RequestsHistory = new();
        ResponsesHistory = new();

        INetwork networkInterceptor = WrappedDriver.Manage().Network;
        networkInterceptor.NetworkResponseReceived += (o, s) =>
        {
            ResponsesHistory.Add(s);
        };
        networkInterceptor.NetworkRequestSent += (o, s) =>
        {
            RequestsHistory.Add(s);
        };

        networkInterceptor.StartMonitoring();
    }

    public void ClearRequestsResponsesHistory()
    {
        RequestsHistory.Clear();
        ResponsesHistory.Clear();
    }

    public List<string> GetSpecificRequestUrls(string requestName)
    {
        HashSet<NetworkRequestSentEventArgs> requests = new HashSet<NetworkRequestSentEventArgs>(RequestsHistory);
        return requests.ToList()
            .FindAll(r => r.RequestUrl.ToString().Contains(requestName))
            .Select(fr => fr.RequestUrl).ToList();
    }

    public void OverrideScreenResolution(long screenHeight, long screenWidth, bool mobile, double deviceScaleFactor)
    {
        var settings = new SetDeviceMetricsOverrideCommandSettings();
        settings.ScreenHeight = screenHeight;
        settings.ScreenWidth = screenWidth;
        settings.Mobile = mobile;
        settings.DeviceScaleFactor = deviceScaleFactor;

        DevToolsSession.SendCommand(settings);
    }

    public long GetResponseContentLengthByPartialUrl(string responseName)
    {
        var exactResponse = ResponsesHistory.ToList().Find(r => r.ResponseUrl.Contains(responseName)).ResponseHeaders["content-length"].ToString();

        return exactResponse.ToLong();
    }

    public string GetResponseContentTypeByPartialUrl(string responseName)
    {
        var exactResponse = ResponsesHistory.ToList().Find(r => r.ResponseUrl.Contains(responseName)).ResponseHeaders["content-type"].ToString();

        return exactResponse;
    }

    public void AssertResponse404ErrorCodeRecievedByPartialUrl(string partialUrl)
    {
        var response = ResponsesHistory.ToList().Find(r => r.ResponseUrl.Contains(partialUrl)).ResponseStatusCode;

        Assert.AreEqual(response, 404, "404 Error code not detected on the page.");
    }

    public void AssertNoErrorCodes()
    {
        bool areThereErrorCodes = ResponsesHistory.Any(r => r.ResponseStatusCode > 400 && r.ResponseStatusCode < 599);
        Assert.IsFalse(areThereErrorCodes, "Error codes detected on the page.");
    }

    public void AssertRequestMade(string url)
    {
        bool areRequestsMade = ResponsesHistory.Any(r => r.ResponseUrl.Contains(url));
        Assert.IsTrue(areRequestsMade, $"Request {url} was not made.");
    }

    public void AssertRequestNotMade(string url)
    {
        bool areRequestsMade = ResponsesHistory.Any(r => r.ResponseUrl.Contains(url));
        Assert.IsFalse(areRequestsMade, $"Request {url} was made.");
    }

    public int CountRequestsMadeByFileFormat(string fileFormat)
    {
        var exactResponse = ResponsesHistory.ToList().FindAll(r => r.ResponseUrl.Contains(fileFormat)).ToList();

        var numberOfResponses = exactResponse.Count;

        return numberOfResponses;
    }

    public void DisableCache()
    {
        DevToolsSessionDomains.Network.Disable();
    }

    public async Task<DocumentSnapshot[]> CaptureSnapshot()
    {
        var result = await DevToolsSessionDomains.DOMSnapshot.CaptureSnapshot(new CaptureSnapshotCommandSettings()
        {
            ComputedStyles = new string[] { "background-color", "color", "font-weight", "font-family", "display" },
            IncludeBlendedBackgroundColors = true,
            IncludePaintOrder = true,
            IncludeDOMRects = true,
            IncludeTextColorOpacities = true,
        });
        return result.Documents;
    }

    public void AddExtraHttpHeader(string header)
    {
        var settings = new SetExtraHTTPHeadersCommandSettings();
        settings.Headers.Add("Accept-Encoding", "gzip, deflate");
        DevToolsSession.SendCommand(settings);
    }

    public void OverrideUserAgent(string userAgent)
    {
        var settings = new SetUserAgentOverrideCommandSettings();
        settings.UserAgent = userAgent;
        DevToolsSession.SendCommand(settings);
    }

    public void OverrideGeolocationSettings(double latitude, double longitude, int accuracy)
    {
        var settings = new SetGeolocationOverrideCommandSettings();
        settings.Latitude = latitude;
        settings.Longitude = longitude;
        settings.Accuracy = accuracy;

        DevToolsSession.SendCommand(settings);
    }

    public void OverrideDeviceMetrics(long width, long height, bool mobile, double deviceScaleFactor)
    {
        var settings = new SetDeviceMetricsOverrideCommandSettings();
        settings.Width = width;
        settings.Height = height;
        settings.Mobile = mobile;
        settings.DeviceScaleFactor = deviceScaleFactor;

        DevToolsSession.SendCommand(settings);
    }

    public void IgnoreCertificateError()
    {
        var settings = new SetIgnoreCertificateErrorsCommandSettings();
        settings.Ignore = true;
        DevToolsSession.SendCommand(settings);
    }

    public async Task BlockUrls(string pattern)
    {
        await DevToolsSessionDomains.Network.Enable(new EnableCommandSettings());
        await DevToolsSessionDomains.Network.SetBlockedURLs(new SetBlockedURLsCommandSettings()
        {
            Urls = new string[] { "*://*/*.css" }
        });
    }

    public async Task EmulateNetworkConditionOffline()
    {
        await DevToolsSessionDomains.Network.Enable(new EnableCommandSettings()
        {
            MaxTotalBufferSize = 100000000
        });

        await DevToolsSessionDomains.Network.EmulateNetworkConditions(new EmulateNetworkConditionsCommandSettings()
        {
            Offline = true,
        });
    }

    public async Task EmulateNetworkConditions(ConnectionType connectionType, int downloadThroughput, double latency, int uploadThroughput)
    {
        await DevToolsSessionDomains.Network.Enable(new EnableCommandSettings()
        {
            MaxTotalBufferSize = 100000000
        });

        await DevToolsSessionDomains.Network.EmulateNetworkConditions(new EmulateNetworkConditionsCommandSettings()
        {
            ConnectionType = connectionType,
            DownloadThroughput = downloadThroughput,
            Latency = latency,
            UploadThroughput = uploadThroughput,
        });
    }

    public async Task ListenConsoleLogs(EventHandler<MessageAddedEventArgs> messageAddedHandler)
    {
        DevToolsSessionDomains.Console.MessageAdded += messageAddedHandler;
        await DevToolsSessionDomains.Console.Enable();
    }

    public async Task ListenJavaScriptConsoleLogs(EventHandler<JavaScriptConsoleApiCalledEventArgs> javaScriptConsoleApiCalled)
    {
        IJavaScriptEngine monitor = new JavaScriptEngine(WrappedDriver);
        monitor.JavaScriptConsoleApiCalled += javaScriptConsoleApiCalled;
        await monitor.StartEventMonitoring();
    }

    public async Task ListenJavaScriptExceptionsThrown(EventHandler<JavaScriptExceptionThrownEventArgs> javaScriptExceptionThrown)
    {
        IJavaScriptEngine monitor = new JavaScriptEngine(WrappedDriver);
        monitor.JavaScriptExceptionThrown += javaScriptExceptionThrown;
        await monitor.StartEventMonitoring();
    }

    public async Task AddInitializationScript(string name, string script)
    {
        IJavaScriptEngine monitor = new JavaScriptEngine(WrappedDriver);
        await monitor.AddInitializationScript(name, script);
    }

    public async Task TurnOnPerformanceMetrics()
    {
        var enableCommand = new EnableCommandSettings();
        await DevToolsSession.SendCommand(enableCommand);           
    }

    public async Task<Metric[]> GetPerformanceMetrics()
    {
        var metricsResponse = await DevToolsSession.SendCommand<GetMetricsCommandSettings, GetMetricsCommandResponse>(new GetMetricsCommandSettings());
        return metricsResponse.Metrics;
    }

    public void Dispose()
    {
        DevToolsSession.Dispose();
        GC.SuppressFinalize(this);
    }
}
