@ECHO OFF
CALL rmdir /S /Q %~dp0\nuget
CALL mkdir %~dp0\nuget
CALL mkdir %~dp0\nuget\lib\netstandard2.0
CALL mkdir %~dp0\nuget\ref\netstandard2.0
CALL mkdir %~dp0\nuget\runtimes\any\lib\netstandard2.0

CALL set MSBuildSDKsPath=
CALL dotnet msbuild %~dp0\..\Gee.External.Browsing\Gee.External.Browsing.csproj ^
     /property:Configuration="Release" ^
     /property:Platform="Any CPU" ^
     /property:AssemblyVersion=1.0.0.0 ^
     /property:Copyright="Copyright (c) Ahmed Garhy" ^
     /property:FileVersion=1.0.0.0 ^
     /property:InformationalVersion=1.0.0.0 ^
     /property:Product="Safe Browsing.NET"

CALL copy /V /Y ^
          %~dp0\..\LICENSE ^
          %~dp0\nuget\LICENSE

CALL copy /V /Y ^
          "%~dp0\..\Gee.External.Browsing\bin\Any CPU\Release\netstandard2.0\Gee.External.Browsing.dll" ^
          %~dp0\nuget\lib\netstandard2.0\Gee.External.Browsing.dll

CALL copy /V /Y ^
          %~dp0\..\Gee.External.Browsing\Gee.External.Browsing.xml ^
          %~dp0\nuget\lib\netstandard2.0\Gee.External.Browsing.xml

CALL copy /V /Y ^
          "%~dp0\..\Gee.External.Browsing\bin\Any CPU\Release\netstandard2.0\Gee.External.Browsing.dll" ^
          %~dp0\nuget\ref\netstandard2.0\Gee.External.Browsing.dll

CALL copy /V /Y ^
          %~dp0\..\Gee.External.Browsing\Gee.External.Browsing.xml ^
          %~dp0\nuget\ref\netstandard2.0\Gee.External.Browsing.xml

CALL copy /V /Y ^
          "%~dp0\..\Gee.External.Browsing\bin\Any CPU\Release\netstandard2.0\Gee.External.Browsing.dll" ^
          %~dp0\nuget\runtimes\any\lib\netstandard2.0\Gee.External.Browsing.dll

CALL copy /V /Y ^
          "%~dp0\..\Gee.External.Browsing\bin\Any CPU\Release\netstandard2.0\Gee.Common.dll" ^
          %~dp0\nuget\runtimes\any\lib\netstandard2.0\Gee.Common.dll

CALL copy /V /Y ^
          %~dp0\Gee.External.Browsing.nuspec ^
          %~dp0\nuget\Gee.External.Browsing.nuspec

CALL cd %~dp0\nuget
CALL nuget pack
CALL copy /V /Y %~dp0\nuget\Gee.External.Browsing.1.0.0-rc.nupkg %~dp0\Gee.External.Browsing.1.0.0-rc.nupkg
CALL cd %~dp0
CALL rmdir /S /Q %~dp0\nuget
