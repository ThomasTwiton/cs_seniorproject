# PluggedIn
PluggedIn is a social media network that connects musicians, 
ensembles, and venues making it easier to network and find 
out about people, auditions, and gigs in your area. PluggedIn
was created as a Senior Computer Science Project.


### Objective 
Our goal is to develop an application that makes it simple 
for bands and ensembles to search for gigs, venues to 
search for bands, and musicians to find bands offering 
auditions. 

## Getting Started

These instructions will get you a copy of the project up 
and running on your local machine for development and 
testing purposes. See deployment for notes on how to 
deploy the project on a live system.


### Prerequisites

PluggedIn utilizes the .NET framework for its server and
database and React.js and Bootstrap for the front-end design
and user interface. For this application, you will need:

* [Asp .NET Core v2.1](https://dotnet.microsoft.com/download/archives)
* React.js
* Bootstrap 4
* JQuery *(For some Bootstrap formatting)*

It is also recommended that you have Visual Studio. Use
of other IDEs should still work, however Visual Studio
has a fair amount of built-in functionality that makes
development and deployment much easier. In addition, you may want to 
have some way of minifying `.css` and `.js` code, however
if you choose not to, there is a simple fix in the configuration
of the `_Layout.cshtml` file.

### Installation

First download and install .NET Core 2.1 [here](https://dotnet.microsoft.com/download/archives).
Then, if you would like to use Visual Studio, you can download
and install it [here](https://visualstudio.microsoft.com/downloads/).

Next clone this repository or download the `.zip` file.

To clone our repository, execute the commands below in a PowerShell terminal.
```
cd \location\for\development\
git clone git@github.com:ThomasTwiton/cs_seniorproject.git
```

If you simply downloaded the `.zip` file, unzip it in the directory you
would like to use for development.

When you have the necessary files, run the `setup.ps1` file 
or execute the following from the `\server\server` directory
```
.\setup.ps1
```

This script will build your environment, test the application, and run
the application locally.

## Running the tests

Automated unit testing for this repository is provided by [Travis CI](https://travis-ci.org/)
An explanation about how the unit tests are run can be 
found in the `PluggedIn_Tests/` directory. It is 
recommended that you run your unit tests locally *before*
pushing any changes to Github as the current implementation
of .NET is a bit sluggish.

Executing the following commands in a PowerShell terminal will run
the tests for the application.
```
dotnet build
dotnet test PluggedIn_Tests.sln
```

Additionally, tests can be run in Visual Studio by going to Test > Run > All Tests

## Deployment

PluggedIn, being a .NET application written in Visual
Studio, works really well with 
[Azure](https://azure.microsoft.com/en-us/). Other 
options might be available, but your mileage may vary.

Using Visual Studio, you can easily deploy by:
* Make a free account with [Azure](https://azure.microsoft.com) 
* In Powershell, navigate into the server folder
* Run the script `.\deployscript.ps1`

This will create a blank website on Azure.
Eventually, this script will complete the following steps automatically, but for now:

* In .NET, in the Solution Explorer, right click the top level of the project (server), and go to Publish
* Within the wizard, connect to your Azure profile and choose to deploy to the web app created by the script
* Within the wizard, go to Configure > Settings > Databases > Entity Framework Migrations and select “Apply this migration on publish”


<!--
## Contributing

Please read [CONTRIBUTING.md](https://gist.github.com/PurpleBooth/b24679402957c63ec426) 
for details on our code of conduct, and the process 
for submitting pull requests to us.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/your/project/tags). 
-->
## Authors

* **Tyler Conzett** - [conzty01](https://github.com/conzty01)
* **Thomas Twiton** - [ThomasTwiton](https://github.com/ThomasTwiton)
* **Mason Donnohue** - [donnma01](https://github.com/donnma01)

See also the list of [contributors](https://github.com/ThomasTwiton/cs_seniorproject/graphs/contributors)
who participated in this project.
<!--
## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

* Hat tip to anyone who's code was used
* Inspiration
* etc
-->

