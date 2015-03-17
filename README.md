CloudFlare DDNS Updater
=============

Dynamic DNS client for use with CloudFlare.
Written in C#, using Windows Forms.


## Support Me
[![Donate Via Paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=CALMNQUWLZNYL)

## Requirements
Windows XP/Vista/7/8/8.1 or Windows Server 2008R2/2012/2012R2
.NET 4.0 or above (4.5 already installed with Windows 8 / 8.1)

## Building
Solution is known to build with VS2013.

## Installing
The executable can be run from anywhere, no install required.
Settings will be saved in %userprofile%\AppData\Local\CloudFlare_DDNS

The application makes use of the Windows Event Log to save messages.
Run the application as an Administrator at least once (Right Click->Run as Administrator), this will automatically create the required registry keys for Event Log access.

On first launch, enter your details in Tools->Options.
You will need your CloudFlare API key, available on your CloudFlare Account page.

## External Resources
This application makes use of the [FamFamFam Icon Pack](http://www.famfamfam.com/lab/icons/silk/)
,available under the Creative Commons Attributions 2.5 license.
