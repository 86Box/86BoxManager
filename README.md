# 86BoxManager
**86Box Manager** is the official (though optional) configuration manager for the [86Box emulator](https://github.com/86Box/86Box). It's released under the MIT license, so it can be freely distributed with 86Box. See the `LICENSE` file for more information.

It's written in C# with Windows Forms using Visual Studio 2017.

## Features
* Create multiple isolated virtual machines
* Give each VM a unique name and an optional description
* Run multiple virtual machines at the same time
* Control the VM from the Manager window using commands like Pause, Reset, etc.
* Minimize the Manager window when a VM is started

## System requirements
* Same as 86Box  

Additionally, the following is required:  

* 86Box build 1748 or later (earlier builds don't support all the features the Manager expects)
* .NET Framework 4.0

## How to use

1. Download the desired build
2. Run 86boxmanager.exe
3. Choose the folder where 86Box is located (exe and roms folder) and a folder where your virtual machines will be located (configs, nvr folders, etc.)
4. Start creating new virtual machines and enjoy

## How to build

1. Clone the repo
2. Open 86boxmanager.sln solution file in Visual Studio 2017
3. Make your changes
4. Choose the "Release" configuration for x86 platform
5. Build solution
6. 86boxmanager.exe is now in Bin\x86\Release\

## Support
If you have any issues, questions, suggestions, etc., you should visit the official 86Box support channels on IRC and Discord. The project maintainer, daviunic, is often idling there under the name Overdoze.