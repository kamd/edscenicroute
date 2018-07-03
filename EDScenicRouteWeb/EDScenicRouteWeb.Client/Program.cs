using Microsoft.AspNetCore.Blazor.Browser.Rendering;
using Microsoft.AspNetCore.Blazor.Browser.Services;
using System;
using Cloudcrate.AspNetCore.Blazor.Browser.Storage;
using EDScenicRouteWeb.Client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EDScenicRouteWeb.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new BrowserServiceProvider(configure =>
            {
                configure.AddSingleton<AppState>();
                configure.AddStorage();
            });

            new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        }
    }
}
