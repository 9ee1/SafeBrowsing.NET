# Threat Databases Code Samples

`Threat Databases` are a mechanism to store threat lists retrieved from the Google Safe Browsing API. Once you store
one or more threat lists in a `Threat Database`, you can then query it to determine if a threat exists or not.

If you're using a `Managed Service`, you'll typically never interact with a `Threat Database` directly because the
`Managed Service` will retrieve threat lists on a periodic schedule and manage a `Threat Database` for you
automatically. If you, however, wish to retrieve threat lists manually using a `Client`, you should consider using one
of the out-of-the-box `Threat Databases` provided by Safe Browsing.NET as opposed to building your own.

> [!TIP]
> To follow along with these examples, please see the
> [Gee.External.Browsing.Databases](xref:Gee.External.Browsing.Databases) and the
> [Gee.External.Browsing.Databases.Json](xref:Gee.External.Browsing.Databases.Json) namespaces in the
> [Safe Browsing.NET API reference](xref:Gee.External.Browsing).

## Example: Using an In-Memory Database to Store Retrieved Threat Lists

[!code-csharp[Main](databases-code-sample-01.code)]

## Example: Using a JSON Database to Store Retrieved Threat Lists

[!code-csharp[Main](databases-code-sample-02.code)]
