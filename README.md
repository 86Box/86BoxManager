# 86Box Manager
**86Box Manager** is the official (though optional) configuration manager for the [86Box emulator](https://github.com/86Box/86Box). It's released under the MIT license, so it can be freely distributed with 86Box. See the `LICENSE` file for license information and `AUTHORS` for a complete list of contributors and authors.

It's written in C# with Windows Forms using Visual Studio 2017. Please see the [wiki](https://github.com/86Box/86BoxManager/wiki) for additional information.

## Features
* Powerful, lightweight and completely optional
* Create multiple isolated virtual machines
* Give each virtual machine a unique name and an optional description
* Run multiple virtual machines at the same time
* Control virtual machines from the Manager (pause, reset, etc.)
* A tray icon so the Manager window doesn't get in your way

## System requirements
Same as for 86Box. Additionally, the following is required:  

* 86Box 2.0 build 1799 or later (earlier builds don't support all the features the Manager expects)
* .NET Framework 4.6

## Support
If you have any issues, questions, suggestions, etc., please follow the [troubleshooting steps](https://github.com/86Box/86BoxManager/wiki/Troubleshooting-steps) or visit the official 86Box support channels on IRC and Discord (see the main 86Box repo for links). Lead developer, daviunic, is often idling there under the name Overdoze.

## How to use
1. Download the desired build [here](https://github.com/86Box/86BoxManager/releases)
2. Run `86Manager.exe`
3. Go to Settings, choose the folder where `86Box.exe` is located (along with the roms folder) and a folder where your virtual machines will be located (for configs, nvr folders, etc.)
4. Start creating new virtual machines and enjoy

## How to build
1. Clone the repo
2. Open `86BoxManager.sln` solution file in Visual Studio 2017
3. Make your changes
4. Choose the `Release` configuration and `x86` platform/CPU
5. Build the solution
6. `86Manager.exe` is now in `Bin\x86\Release\`
