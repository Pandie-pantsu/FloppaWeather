# Floppa Weather

## Simple, yet kinda quirky.

---
Floppa Weather is a simple weather web application built in C# using the .NET 6.0 framework.
This was originally going to be a project using Node.js, but I scrapped it in favor of .NET.

## Dependencies
---
- jQuery 3.6.0
- Microsoft.EntityFrameworkCore.Tools 6.0.2

It is recommended to install through the dotnet CLI
```sh
$ dotnet add package jQuery -v 3.6.0
$ dotnet add package Microsoft.EntityFrameworkCore.Tools -v 6.0.2
```

## Secret Key
---
In GeolocationDTOController.cs, paste your OpenWeatherMap API Key in line 38:
```cs
string Secret = "{YOUR API KEY HERE}";
```
## To-Do List:
---
- Implement a database schema using MySQL.
- Use the One-Call API to get current weather and five day forecast.
- Store current weather and five day forecast to database to save from constantly making calls to the API.
- Deploy the darn thing (either via Azure or somewhere lol)
