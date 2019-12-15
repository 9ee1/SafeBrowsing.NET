# Unmanaged Service Code Samples

An `Unmanaged Service` is a mechanism to allow you to lookup a URL to determine whether it is safe
or unsafe, using a `Threat Database` that is managed through another mechanism. In contrast to a `Managed Service`, it
does not manage a `Threat Database` automatically for you. It simply just consumes (i.e. read from) it. An
`Unmanaged Service` is, effectively, a *semi* implementation of the
[Google Safe Browsing Update API protocol](https://developers.google.com/safe-browsing/v4/update-api).

An `Unmanaged Service` is named so because it *does not manage* a `Threat Database` for you.

> [!TIP]
> To follow along with these examples, please see the
> [Gee.External.Browsing.Services](xref:Gee.External.Browsing.Services) namespace and in particular
> [UnmanagedBrowsingServiceBuilder](xref:Gee.External.Browsing.Services.UnmanagedBrowsingServiceBuilder),
> [UnmanagedBrowsingService](xref:Gee.External.Browsing.Services.UnmanagedBrowsingService), and
> [UrlLookupResult](xref:Gee.External.Browsing.Services.UrlLookupResult) in the
> [Safe Browsing.NET API reference](xref:Gee.External.Browsing).

## Example: Create an Unmanaged Service

[!code-csharp[Main](unmanaged-service-code-sample-01.code)]
