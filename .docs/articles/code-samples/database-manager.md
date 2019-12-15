# Threat Database Manager Code Samples

A `Threat Database Manager` is a mechanism to retrieve threat lists from the Google Safe Browsing API on a
periodic schedule and manage a `Threat Database` for you automatically.

If you have a use case where you want to build a `Threat Database` that is shared by multiple different processes,
regardless of whether those processes are local or remote, a `Threat Database Manager` is the right solution for
you. It can simplify the implementation effort for you since you don't have to build your own database synchronization
mechanism.

> [!TIP]
> To follow along with these examples, please see the
> [Gee.External.Browsing.Services](xref:Gee.External.Browsing.Services) namespace and in particular
> [BrowsingDatabaseManagerBuilder](xref:Gee.External.Browsing.Services.BrowsingDatabaseManagerBuilder) and
> [BrowsingDatabaseManager](xref:Gee.External.Browsing.Services.BrowsingDatabaseManager) in the
> [Safe Browsing.NET API reference](xref:Gee.External.Browsing).

## Example: Create a Database Manager With All Available Threat Lists

[!code-csharp[Main](database-manager-code-sample-01.code)]

## Example: Create a Database Manager With a Restricted Number of Threat Lists

[!code-csharp[Main](database-manager-code-sample-02.code)]

## Example: Create a Database Manager With Threat List Update Constraints

[!code-csharp[Main](database-manager-code-sample-03.code)]
