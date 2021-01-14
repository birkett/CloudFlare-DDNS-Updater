# CloudFlareDDNS Updater Client

Dynamic DNS client for Windows, for use with CloudFlare.
Written in C#, using Windows Forms.

## Features

> - Supports updating of the following MX records
>
>> **AAAA**    (IPv6)
>> **A**       (IPv4)
>
> - Supports Multiple Network Interface
> - Supports custom IPv4, IPv6 update URL
> - Supports custom Cloudfalre API URL
> - Supports Cloudflare API v4
> - Supports to use Internal IP
> - Supports Windows Event Log
> - Supports Multiple Domains

## Screenshots

### Overview

![Overview](/READMEPresentImage.png)

### Settings

![Settings](/READMESettingsImage.png)

## Requirements

### OS Support

* Client: Windows XP / Vista / 7 / 8 / 8.1
* Server: 2008 / 2008R2 / 2012 / 2012R2

### .NET Framework

.NET Framework 4.7 or above is required (already installed on Windows 8 / 8.1 / 2012 / 2012R2).
https://www.microsoft.com/en-us/download/details.aspx?id=55170
<br /><br />

## Building

Solution is known to build with Visual Studio 2013 (Community, Pro and Ultimate tested, Express should work), no additional dependencies are needed.<br />
The project targets the full .NET 4.7 profile (not the client profile).
<br /><br />

## Installing

The executable can be run from anywhere, no install required.
Settings will be saved in %userprofile%\AppData\Roaming\CloudFlareDDNS

The application makes use of the Windows Event Log to save messages.
Run the application as an Administrator at least once (Right Click->Run as Administrator), this will automatically create the required registry keys for Event Log access.
<br /><br />

## Configuration

On first launch, enter your details in Tools->Options.
###### Domain: The root domain name you wish to update (example.com)

###### Email: The email address associated with your CloudFlare account

###### API Key: The API key given by CloudFlare at the bottom of your "My Account" page


###### Auto Fetch Time: The time in minutes an update will automatically occur

###### Use Windows Event Log: Enable / Disable writing logs to the Windows Event Log

<br /><br />

## External Resources

This application makes use of the [FamFamFam Icon Pack](http://www.famfamfam.com/lab/icons/silk/)
,available under the Creative Commons Attributions 2.5 license.
