#Code Coverage with Coverlet

## Getting Started

Open up Windows Powershell

Navigate into the PluggedIn_Tests folder

Run the following command to install Coverlet:

` dotnet add package coverlet.msbuild --version 2.1.0 `

Once installed, from within the PluggedIn_Tests folder, simply run the command
` dotnet test /p:CollectCoverage=true`

This will print a report to the console, and store the information in a file called `coverage.xml`