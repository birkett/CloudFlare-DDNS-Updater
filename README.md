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
Add the following key and string value to your registry:

```
Windows Registry Editor Version 5.00

[HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\Application\CloudFlare DDNS Updater]
"EventMessageFile"="C:\\windows\\microsoft.net\\framework\\v4.0.30319\\EventLogMessages.dll"
```

This is required so the program does not require elevation at any point.

On first launch, enter your details in Tools->Options. 
You will need your CloudFlare API key, available on your CloudFlare Account page. 

## External Resources
This application makes use of the [![FamFamFam Icons](FamFamFam Icon Pack)](http://www.famfamfam.com/lab/icons/silk/)
Icon pack is available under the Creative Commons Attributions 2.5 license. 