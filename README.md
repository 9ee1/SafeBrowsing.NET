# Safe Browsing.NET

Safe Browsing.NET is an opinionated .NET implementation of the
[Google Safe Browsing API](https://developers.google.com/safe-browsing). It is written in C#, supports
[Google Safe Browsing API 4](https://developers.google.com/safe-browsing/v4), and has a flexible and modular API that
is ridiculously easy to learn, pick up, and extend.

[![NuGet](https://img.shields.io/nuget/dt/Gee.External.Browsing?label=Safe%20Browsing.NET%20V1.0.0&style=for-the-badge)](https://www.nuget.org/packages/Gee.External.Browsing/1.0.0)

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

## Quick Example

Please see the [Safe Browsing.NET documentation](https://9ee1.github.io/SafeBrowsing.NET/articles/index.html) for a
collection of code examples and samples.

```C#
using Gee.External.Browsing.Cache;
using Gee.External.Browsing.Clients;
using Gee.External.Browsing.Databases;
using Gee.External.Browsing.Services;
using System.Threading.Tasks;

namespace Examples {
    /// <summary>
    ///     Main Program.
    /// </summary>
    public static class Program {
        /// <summary>
        ///     Execute Main Program.
        /// </summary>
        /// <param name="args">
        ///     A collection of arguments passed from the command line.
        /// </param>
        /// <returns>
        ///     A task representing the asynchronous operation
        /// </returns>
        public static async Task Main(string[] args) {
            // ...
            //
            // Create a managed service that uses an in-memory cache, a JSON database that is persisted to disk, and
            // an HTTP client to communicate with the Google Safe Browsing API.
            //
            // The managed service will automatically setup a database and manage it (i.e. it will automatically
            // synchronize it with the Google Safe Browsing API in accordance with the Google Safe Browsing Update API
            // protocol).
            var managedService = ManagedBrowsingService.Build()
                .UseMemoryCache()
                .UseJsonDatabase("/Google.json")
                .UseHttpClient("YOUR_GOOGLE_API_KEY")
                .Build();

            using (managedService) {
                try {
                    // ...
                    //
                    // Throws an exception if the operation fails.
                    const string testUrl = "https://testsafebrowsing.appspot.com/s/phishing.html";
                    var lookupTask = managedService.LookupAsync(testUrl);
                    var testUrlLookupResult = await lookupTask.ConfigureAwait(false);
                    if (testUrlLookupResult.IsDatabaseStale) {
                        // ...
                        //
                        // If the database is stale, you can either wait for a few minutes and try again or assume the
                        // URL is safe.
                        //
                        // Internally, if the database is stale, the service will block, up to a limit so your program
                        // doesn't freeze, until it is up-to-date before determining whether the URL is safe or unsafe.
                        // So this URL lookup result code should be rare.
                    }
                    else if (testUrlLookupResult.IsSafe) {
                        // ...
                        //
                        // Yay, the URL is safe!
                    }
                    else if (testUrlLookupResult.IsUnsafe) {
                        // ...
                        //
                        // If the URL is unsafe, retrieve the computed URL expression that indicated it is unsafe.
                        var unsafeUrlExpression = testUrlLookupResult.UnsafeUrlExpression;

                        // ...
                        //
                        // If the URL is unsafe, retrieve the collection of unsafe threat list descriptors the URL is
                        // associated with.
                        var unsafeThreatListDescriptors = testUrlLookupResult.UnsafeThreatListDescriptors;

                        // ...
                        //
                        // If the URL is unsafe, you can use the collection of unsafe threat list descriptors the URL
                        // is associated with to determine the target platforms and threat types. But you can also use
                        // the following convenient properties to retrieve them.
                        var targetPlatforms = testUrlLookupResult.TargetPlatforms;
                        var threatTypes = testUrlLookupResult.ThreatTypes;
                    }
                }
                catch (BrowsingCacheException) {
                    // ...
                    //
                    // Catch this exception if you want to handle a caching error.
                }
                catch (BrowsingClientException) {
                    // ...
                    //
                    // Catch this exception if you want to handle a communication error with the Google Safe Browsing
                    // API.
                }
                catch (BrowsingDatabaseException) {
                    // ...
                    //
                    // Catch this exception if you want to handle a database error.
                }
            }
        }
    }
}
```
