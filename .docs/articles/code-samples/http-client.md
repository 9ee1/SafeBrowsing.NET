# HTTP Client Code Samples

An `HTTP Client` is the default implementation of a `Client` in Safe Browsing.NET that allows you to communicate with
the Google Safe Browsing API.

If you're using a `Service`, whether a `Managed Service` or an `Unmanaged Service`, you'll typically never use an
`HTTP Client` directly because the `Managed Service` will communicate with the Google Safe Browsing API for you
internally. If you wish, however, to communicate with the Google Safe Browsing API manually, you should consider using
an `HTTP Client` as opposed to building your own.

> [!TIP]
> To follow along with these examples, please see the
> [Gee.External.Browsing.Clients](xref:Gee.External.Browsing.Clients) and the
> [Gee.External.Browsing.Clients.Http](xref:Gee.External.Browsing.Clients.Http) namespaces and in particular
> [HttpBrowsingClient](xref:Gee.External.Browsing.Clients.Http.HttpBrowsingClient) in the
> [Safe Browsing.NET API reference](xref:Gee.External.Browsing).

## Example: Retrieve All Available Threat List Descriptors

[!code-csharp[Main](http-client-code-sample-01.code)]

## Example: Retrieve Threat List Full Updates

[!code-csharp[Main](http-client-code-sample-02.code)]

## Example: Retrieve Threat List Full Updates With Update Constraints

[!code-csharp[Main](http-client-code-sample-03.code)]

## Example: Retrieve Threat List Partial Updates

[!code-csharp[Main](http-client-code-sample-04.code)]
