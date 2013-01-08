# Outil de vérification de consommation internet des rédences de l'ÉTS
My ISP's Internet usage monitoring tool isn't very good so I made another one.
It's available on Windows Azure: http://reznetusage.cloudapp.net/

## Components
### DataFetcher
Used to gather monthly information about the global internet usage.
The data is used for monthly statistics and history. See [Monthly.aspx](https://github.com/scarpentier/ETS-Rez-Net-Usage/blob/master/RezNetUsage.Web/Monthly.aspx)

### RezNetUsage.Core
Contains the storage objects used by the UI and the logic to fetch the usage information from the Cooptel website.
The magic happens in the [UsageFactory](https://github.com/scarpentier/ETS-Rez-Net-Usage/blob/master/RezNetUsage.Core/UsageFactory.cs) class.

### RezNetUsage.Web
Actual web site files

