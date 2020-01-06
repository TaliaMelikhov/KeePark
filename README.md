# KeePark

KeePark is a startup company which seeded by 3 students from the collage of manangement.
The purpose of the system is to provide a proper platform for people who want to rent their private parking spot
and for other who are looking to reserve their parking spots in advance.

## Key Features

  - Admin interface - see your entire system parking spots, customers orders and system users.
  - Recommendation system powered by machine learning.
  - Integrated with Facebook graph API, GoogleMaps GeoCoder and OpenWeather services.
  - Beautiful and mobile-friendly interface.
  - Order your parking spots in advance (except for the actual payment).

## Installation

KeePark requires [ASP.Net](https://www.asp.net/core/overview/aspnet-vnext) to run.

 - open .KeePark/KeePark/Views/ParkingSpots/Details.cshtml and make sure to replace the Key with the relevant API keys.
 - open .KeePark/KeePark/Views/ReserveSpots/Details.cshtml add API keys for Google API and OpenWeather.
 - open the sln file on the main folder with Visual Studio 2019, and type the following command in the NuGet console

```sh
$ Update-Database -context KeeParkContext
$ Update-Database -context IdentityContext
```
Another package installation is required for the ML algo
```sh
pm> Install-Package Accord -Version 3.8.2
pm> Install-Package Accord.MachineLearning
pm> Install-Package Accord.Math -Version 3.8.0
pm> Install-Package Accord.Statistics -Version 3.8.0
```
By Starting the project an initialization function is executing to generate fake data into the project.
```
Administrator credentials:
        user: keepark@keepark.com
    password: Ad7&Ad
Standard user credentials:
        user: peter@peter.com
    password: Ad7&Ad
```

## Project Owners
Talia Melikhov |
Raz Sardas |
Aviv Nizri
