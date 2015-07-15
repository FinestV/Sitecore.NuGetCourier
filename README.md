# Sitecore.NuGetCourier

Builds a NuGet Manifest (.nuspec) or NuGet Package (.nupkg) file by analyzing two Sitecore project directories and capturing the differences.

The Sitecore.NuGetCourier.Runner can be used in an automated build to produce a NuGet Manifest which includes only the differences between two branches or tags. The packs into a NuGet Package artifact that allows the Sitecore folder to remain untouched and caters for multi site and multi vendor scenarios where we cannot safely purge the project directory.

When used outside of a build server Sitecore.NuGetCourier.Runner may also create the NuGet Package as well as the Manifest.

This project extends Sitecore Courier https://github.com/adoprog/Sitecore-Courier a great tool for building Sitecore update packages during CI builds. Unfortunately update packages are not well suited for code file and config changes.

Sitecore.NuGetCourier fills the gap by creating a NuGet Manifest during an automated build which packs into a NuGet package build artifact.

The NuGet package produced can be consumed directly by Octopus Deploy https://octopusdeploy.com/

<b>Sitecore.NuGetCourier.Runner Usage:<b>

-s source Source Directory

-t target Target Directory

-n nuspec NuGet Manifest Id (#Id#.nuspec)

-c config Include all config files (use if we wan't to transform at deploy not build time)

-p pack Pack the generated NuGet Manifest file to create a NuGet Package with this version number