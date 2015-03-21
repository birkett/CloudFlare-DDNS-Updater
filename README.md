#CloudFlareDDNS Updater Client
Dynamic DNS client for Windows, for use with CloudFlare.
Written in C#, using Windows Forms.


## Support Me
[![Donate Via Paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=CALMNQUWLZNYL)
<br /><br />
## Requirements
####OS Support
* Client: Windows XP / Vista / 7 / 8 / 8.1
* Server: 2008 / 2008R2 / 2012 / 2012R2

####.NET Framework
.NET Framework 4.0 or above is required (already installed on Windows 8 / 8.1 / 2012 / 2012R2).
https://www.microsoft.com/en-gb/download/details.aspx?id=17851
<br /><br />
## Building
Solution is known to build with Visual Studio 2013 (Community, Pro and Ultimate tested, Express should work), no additional dependencies are needed.<br />
The project targets the full .NET 4.0 profile (not the client profile).
<br /><br />
## Installing
The executable can be run from anywhere, no install required.
Settings will be saved in %userprofile%\AppData\Roaming\CloudFlareDDNS

The application makes use of the Windows Event Log to save messages.
Run the application as an Administrator at least once (Right Click->Run as Administrator), this will automatically create the required registry keys for Event Log access.
<br /><br />
##Configuration
On first launch, enter your details in Tools->Options.
######Domain: The root domain name you wish to update (example.com)
######Email: The email address associated with your CloudFlare account
######API Key: The API key given by CloudFlare at the bottom of your "My Account" page
######Auto Fetch Time: The time in minutes an update will automatically occur
######Use Windows Event Log: Enable / Disable writing logs to the Windows Event Log
<br /><br />
##Running as a service
Once you have verified the application is updating your records as intended, you may run it as a service.

To install as a service, run from an evevated (Administrator) command prompt:<br />
```shell
CloudFlareDDNS.exe /install
```

The service will be named "CloudFlareDDNS", and will run automatically on boot. The service shares the configuration with the GUI, and both can be run at the same time.

To remove the service:
```shell
CloudFlareDDNS.exe /uninstall
```
<br /><br />
## External Resources
This application makes use of the [FamFamFam Icon Pack](http://www.famfamfam.com/lab/icons/silk/)
,available under the Creative Commons Attributions 2.5 license.
