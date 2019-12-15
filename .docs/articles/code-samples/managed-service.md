# Managed Service Code Samples

A `Managed Service` is a mechanism to 1) retrieve threat lists from the Google Safe Browsing API on a periodic schedule
and manage a `Threat Database` for you automatically and 2) allow you to lookup a URL to determine whether it is safe
or unsafe. It is, effectively, a *complete* implementation of the
[Google Safe Browsing Update API protocol](https://developers.google.com/safe-browsing/v4/update-api).

A `Managed Service` is the easiest and quickest mechanism to get started using Safe Browsing.NET. However, it might not
be applicable for all use cases. If you have a use case where you want to manage a `Threat Database` and lookup URLs
from the same process, a `Managed Service` is for you.

If, however, you have a use case where you want to build a `Threat Database` that is shared by multiple different
processes, regardless of whether those processes are local or remote, a `Managed Service` might not be the right
solution for you.

A `Managed Service` is named so because it *manages* a `Threat Database` for you.

> [!TIP]
> To follow along with these examples, please see the
> [Gee.External.Browsing.Services](xref:Gee.External.Browsing.Services) namespace and in particular
> [ManagedBrowsingServiceBuilder](xref:Gee.External.Browsing.Services.ManagedBrowsingServiceBuilder),
> [ManagedBrowsingService](xref:Gee.External.Browsing.Services.ManagedBrowsingService), and
> [UrlLookupResult](xref:Gee.External.Browsing.Services.UrlLookupResult) in the
> [Safe Browsing.NET API reference](xref:Gee.External.Browsing).

## Example: Create a Managed Service With All Available Threat Lists

[!code-csharp[Main](managed-service-code-sample-01.code)]

## Example: Create a Managed Service With a Restricted Number of Threat Lists

[!code-csharp[Main](managed-service-code-sample-02.code)]


## Example: Create a Managed Service With Threat List Update Constraints

[!code-csharp[Main](managed-service-code-sample-03.code)]
