# Safe Browsing.NET

Safe Browsing.NET is an opinionated .NET implementation of the
[Google Safe Browsing API](https://developers.google.com/safe-browsing). It is written in C#, supports
[Google Safe Browsing API 4](https://developers.google.com/safe-browsing/v4), and has a flexible and modular API that
is ridiculously easy to learn, pick up, and extend.

## Features

* Safe Browsing.NET supports any
  [.NET Runtime](https://docs.microsoft.com/en-us/dotnet/standard/net-standard#net-implementation-support) that
  supports [.NET Standard 2.0](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md)
* Safe Browsing.NET supports [Google Safe Browsing API 4](https://developers.google.com/safe-browsing/v4)
* Safe Browsing.NET has a flexible and modular API that is ridiculously easy to learn, pick up, and extend
* Safe Browsing.NET is flexible. Want an HTTP client to simply communicate with the Google Safe Browsing API and handle
  the response yourself? Safe Browsing.NET has you covered. Want a service to setup and manage a threat database for
  you? That's covered too
* Safe Browsing.NET is modular. Unhappy with the default HTTP client implementation and want to implement one yourself?
  Safe Browsing.NET has you covered. Want to implement support for a database engine that is not natively supported?
  That's covered too

## Limitations

* Safe Browsing.NET **only implements** the
  [Google Safe Browsing Update API](https://developers.google.com/safe-browsing/v4/update-api). It
  **does not implement** the
  [Google Safe Browsing Lookup API](https://developers.google.com/safe-browsing/v4/lookup-api). Typically, this should
  not be a problem because Safe Browsing.NET has APIs that will setup and manage for you the threat database required
  by the Google Safe Browsing Update API. Additionally, the Google Safe Browsing Update API is generally considered
  more private since URLs that are looked up will not be shared with Google
* Safe Browsing.NET **does not implement**
  [Rice compression](https://developers.google.com/safe-browsing/v4/compression) when retrieving
  [threat lists](https://developers.google.com/safe-browsing/v4/lists) from the Google Safe Browsing API. However, it
  **does implement** standard HTTP compression and will automatically set the correct HTTP compression request headers
  when retrieving threat lists from the Google Safe Browsing API
* Safe Browsing.NET **does not implement**
  [back-off mode](https://developers.google.com/safe-browsing/v4/request-frequency#back-off-mode) if communication with
  the Google Safe Browsing API fails. You should consider implementing this feature yourself in your application until
  such time it is implemented natively by Safe Browsing.NET

## Disclaimer

Safe Browsing.NET is an independently developed project. It is not a Google developed nor is it a Google sponsored
project. It is not affiliated with Google in any way.

## Getting started

### Create a Google Account

You'll need to create a Google account, a Google Developer Console project, a Google Safe Browsing API Key, and
activate the Google Safe Browsing API within your Google Developer Console project. For more information, please see
the [Google Safe Browsing API documentation](https://developers.google.com/safe-browsing/v4/get-started).

You should review the Google Safe Browsing API Terms of Service to avoid having your account disabled by Google. For
more information, please see the
[Google Safe Browsing API documentation](https://developers.google.com/safe-browsing/v4/terms).

Google enforces usage limits on consumers of the Google Safe Browsing API. You should review these usage limits to
avoid having your account disabled by Google. For more information, please see the
[Google Safe Browsing API documentation](https://developers.google.com/safe-browsing/v4/usage-limits).

### Download Safe Browsing.NET

Safe Browsing.NET is distributed as a [NuGet package](https://www.nuget.org/packages/Gee.External.Browsing/). Either
open the NuGet Package Manager in Visual Studio and search for "Safe Browsing.NET" or head over to the
[NuGet website](https://www.nuget.org/packages/Gee.External.Browsing/) and download the NuGet package yourself.

The current version is a *pre-release version*. Please make sure you select *include prerelease* in the NuGet Package
Manager in Visual Studio in order to install it. For more information, please see the
[NuGet documentation](https://docs.microsoft.com/en-us/nuget/create-packages/prerelease-packages).
