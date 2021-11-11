# RunAsAdmin
Visual Studio extension to run Visual Studio as administrator

[![Build Status](https://github.com/sboulema/RunAsAdmin/actions/workflows/workflow.yml/badge.svg)](https://github.com/sboulema/RunAsAdmin/actions/workflows/workflow.yml)
[![Sponsor](https://img.shields.io/badge/-Sponsor-fafbfc?logo=GitHub%20Sponsors)](https://github.com/sponsors/sboulema)

## Installing
[Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=SamirBoulema.RunAsAdmin) ![Visual Studio Marketplace](http://vsmarketplacebadge.apphb.com/version-short/SamirBoulema.RunAsAdmin.svg)

[Github Releases](https://github.com/sboulema/RunAsAdmin/releases)

[Open VSIX Gallery](http://vsixgallery.com/extension/dda2a29d-8fd9-4131-adc5-afcf6ef36b25/)

## Run Visual Studio as administrator
1. Install the [RunAsAdmin](https://marketplace.visualstudio.com/items?itemName=SamirBoulema.RunAsAdmin) extension
2. Go to Tools -> Options -> RunAsAdmin
3. Set Enabled to true
4. Restart Visual Studio

## Run Visual Studio as normal user
1. Go to Tools -> Options -> RunAsAdmin
2. Set Enabled to false
3. Restart Visual Studio

## Uninstalling the extension
When uninstalling the extension the current elevation mode will be preserved.
If you have the extension Enabled in the options and uninstall it,
Visual Studio will remain running as administrator.
You have to use the RunAsAdmin options setting to enable or disable.

## Disabling the extension
When disabling the extension from within the Visual Studio extensions dialog nothing will happen like with uninstalling the extension.
You have to use the RunAsAdmin options setting to enable or disable.

### Under the hood
The extension adds two registry entries at the following location: `HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers\`

One for the path to Visual Studio and one for the path to the Visual Studio Launcher. The value of the registry entries will be "RUNASADMIN" or "RUNASNORMAL"

## Thanks
Cazzulino - [How to get Visual Studio 2017 installation path](https://www.cazzulino.com/how-to-get-vsinstallroot.html)

Rick van den Bosch - [Running Visual Studio as an administrator causes ‘Save changes to devenv.sln’ when double clicking solutions](https://www.rickvandenbosch.net/blog/running-visual-studio-as-an-administrator-causes-save-changes-to-devenv-sln-when-double-clicking-solutions/)