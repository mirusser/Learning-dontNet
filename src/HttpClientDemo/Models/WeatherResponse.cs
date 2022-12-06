
using Newtonsoft.Json;

namespace HttpClientDemo.Models;

public record WeatherResponse(
Coord Coord,
IReadOnlyList<Weather> Weather,
string Base,
Main Main,
int? Visibility,
Wind Wind,
Rain Rain,
Clouds Clouds,
int? Dt,
Sys Sys,
int? Timezone,
int? Id,
string Name,
int? Cod
);

public record Clouds(
int? All
);

public record Coord(
double? Lon,
double? Lat
);

public record Main(
double? Temp,
double? FeelsLike,
double? TempMin,
double? TempMax,
int? Pressure,
int? Humidity,
int? SeaLevel,
int? GrndLevel
);

public record Rain(
    [property: JsonProperty("1h")] double? _1h
);

public record Sys(
int? Type,
int? Id,
string Country,
int? Sunrise,
int? Sunset
);

public record Weather(
int? Id,
string Main,
string Description,
string Icon
);

public record Wind(
double? Speed,
int? Deg,
double? Gust
);