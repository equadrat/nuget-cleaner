# Introduction
This project is an example of how to use the [equadrat Framework](https://www.nuget.org/profiles/equadrat).

 The project is a service to maintain the packages of a NuGet feed.
 Primary usecase is to cleanup a feed from deprecated package versions.

 Internally it uses the [NuGet.Protocol](https://www.nuget.org/packages/NuGet.Protocol) to interact with NuGet.

# Getting Started
Configure the appsettings.json file.
Use **/Source/Worker/appsettings.Development.json** as example
- Sources: the url of your feed(s)
- ApiKeys: the PATs to authenticate against your feed(s).
- PackageCleanups: parameters to define how to cleanup the feed.
- PackageGroups: hookup source, api-key, package-cleanup and the package parameters. You can use patterns to aggregate multiple packages to a group.

# Build and Test
Open **/equadrat.NuGet.Cleaner.sln** in Visual Studio 2022 or later.
- *Source/equadrat.NuGet.Cleaner.Worker* is the startup project.
- *Tests/equadrat.NuGet.Cleaner.UnitTests* is the test project.

# Deployment
There is an installation script for Linux (Ubuntu) you can use to install the application as systemd service: **/Source/Worker/Deployment/Linux/equadrat.NuGet.Cleaner.Worker.Install.sh**.

# Contribute
You can contact me on my website: [www.equadrat.net](https://www.equadrat.net/en/contact/)

# License
Please respect the license and check **equadrat.NuGet.Cleaner.License.md** before using the source code.