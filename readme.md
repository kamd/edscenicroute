
# Elite Dangerous: Scenic Route Finder

This is a web app for players of the space-faring game Elite Dangerous, in which
players may travel across our galaxy, seeing nebulae, black holes and other
galactic wonders.

Using this app, players can enter their starting point and destination and 
receive a list of suggestions of galactic points of interest to visit near to their
intended route.

## Frameworks and Build Requirements

The server side runs on [ASP.NET Core](https://github.com/aspnet/Home), on
top of .NET Core 2.1.

The client side uses the new and experimental [Blazor](https://blazor.net/) framework,
 C# running on WebAssembly. Currently this project uses Blazor 0.6,
requirements to build are listed on the [Blazor website](https://blazor.net/docs/get-started.html).

## Live Version

You can see the project in action at https://elite.kamd.me.uk.

## Overview

The backend uses ASP.NET's Kestrel web server to serve the client pages and two
API calls the page uses, one that returns a list of POI suggestions given two
star systems and a maximum deviation from the straight line route, and one that
returns possible POI names for auto-completing textboxes on the page. Both API
endpoints return JSON data.

The client pages use [Bootstrap 4](https://getbootstrap.com/) components,
[Bootswatch](https://bootswatch.com/)'s Darkly theme, [Font Awesome](https://fontawesome.com/)
icon font, and [Google Fonts](https://fonts.google.com/) CDN.

## Testing

There are a handful of unit tests for the core calculations, written in
the [NUnit](http://nunit.org/) framework.

## External APIs

Elite Dangerous galaxy data is drawn from the [Elite Dangerous Star Map](https://www.edsm.net/)
project, without which this project would not be possible.
