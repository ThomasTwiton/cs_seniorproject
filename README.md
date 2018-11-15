# PluggedIn
PluggedIn is a social media network that connects musicans, 
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

* Asp .NET Core v2.1
* React.js 
* Bootstrap 4
* JQuery *(For some Bootstrap formatting)*

It is also recommended that you have Visual Studio although 
this is not entirely necessary. In addition, you may want to 
have some way of minifying `.css` and `.js` code, however
if you choose not to, there is a simple fix in configuraiton
of the `_Layout.cshtml` file.

### Installation

Here is a step by step series of examples that tells you how
to get the development environment up and running.
```
[ WORK IN PROGRESS ]
```
<!--
Installing PostgreSQL

```
$ sudo apt-get update
$ sudo apt-get install postgresql postgresql-contrib
$ sudo -u postgres createuser --interactive
```
Follow the prompts to create a role for yourself. I suggest creating a role that is the same name as your username and making yourself a superuser.

Installing Python

```
$ sudo apt-get install python3.5
```

Installing VirtualEnv

```
$ sudo pip install virtualenv
$ virtualenv -p python3 /path/to/home/MyEnv
$ source /path/to/home/MyEnv/bin/activate
```
In order to develop in the correct Python environment, you will need to perform the last step each time you close your terminal. Similarly, if you would like to exit the virtual enviromnet, simply type `deactivate` in the terminal.

With all of the above prerequisites installed, you should be able to run
```
$ python createDB.py
$ python scheduleServer.py
```
to start the Flask server. You can then open the browser of your choice and go to `localhost:5000/`.
-->
## Running the tests

Automated unit testing is provided by [Travis CI](https://travis-ci.org/)
An explanation about how the unit tests are run can be 
found in the `PluggedIn_Tests/` directory. It is 
recommended that you run your unit tests locally *before*
pushing any changes to Github as the current implementation
of .NET is a bit sluggish.

## Deployment

PluggedIn, being a .NET application written in Visual
Studio, works really well with 
[Azure](https://azure.microsoft.com/en-us/). Other 
options might be available, but your mileage may vary.

Using Visual Studio you can easily deploy by:
```
[ WORK IN PROGRESS ]
```

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
