# Virtue CI

Virtue is a bot for continuous integration that lets you keep your repositories under control and automate common actions. Virtue has
the benefit of being entirely under your control and very customizable. You run it on your own server and it will perform many tasks,
such as:

* Compiling and checking for errors
* Automatically running unit tests
* Running scripts on new commits

Virtue is still in development, and these features are not complete.

## Setting Up

**Linux, Mac**

Install Mono, then clone the source of Virtue. From the root of the repository, run `xbuild` to compile Virtue, the API,
and all of the common plugins. You'll get Virtue binaries in `Virtue/bin/Debug/`. Copy these wherever you wish, and then
create a `plugins` folder there. Copy any plugins you want in here (you need the .DLL files and the .JSON file). Run
`mono Virtue.exe` from that folder and it'll walk you through the setup.

**Windows**

Clone Virtue and run `msbuild` from the root of the repository with the Microsoft.NET tools in your %PATH%. You'll get
binaries in `Virtue/bin/Debug/`. Copy these wherever, then create a `plugins` folder there. Copy any plugins you want
into this folder (you need the .DLL files and the .JSON file). Run `Virtue.exe` from a command line and it'll walk you
through the rest of the steps.
