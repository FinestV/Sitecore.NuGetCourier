# Sitecore.NuGetCourier

Builds a NuGet Manifest (.nuspec) or NuGet Package (.nupkg) file by analyzing two Sitecore project directories and capturing the differences.

Sitecore.NuGetCourier.Runner creates a NuGet Manifest which captures only the differences between two project directories. This allows for an automated build to include only the differences between two branches or tags rather than the entire site. The build artifact produced allows the Sitecore folder to remain untouched and caters for multi site and multi vendor scenarios where we cannot safely purge the project directory.

This project extends Sitecore Courier https://github.com/adoprog/Sitecore-Courier a great tool for building Sitecore update packages during CI builds. Unfortunately update packages are not well suited for code file and config changes.

Sitecore.NuGetCourier fills the gap by creating a NuGet Manifest during an automated build which packs into a NuGet package build artifact.

The NuGet package produced can be consumed directly by Octopus Deploy https://octopusdeploy.com/