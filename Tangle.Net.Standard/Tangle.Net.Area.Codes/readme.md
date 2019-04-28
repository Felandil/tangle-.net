# IOTA Area Codes (IAC) for .NET Standard

IACs are short, tryte encoded, location codes that can be used to tag and retrieve IOTA transactions related to specific locations. The IACs are typically 11 trytes long and will represent a 13.5m by 13.5m area, at the equator. However IACs can be 12 trytes long and represent a 2.8m by 3.5m grid.

More information can be read here: https://iota-poc-area-codes.dag.sh/

## Installation
The package can be installed via nuget (https://www.nuget.org/packages/Tangle.Net.Area.Codes)

```
Install-Package Tangle.Net.Area.Codes -Version 0.1.0
```

## Examples
The corresponding examples for the TS version can be found here:
https://github.com/Felandil/tangle-.net/blob/develop/Tangle.Net.Standard/Tangle.Net.Area.Codes.Example/Program.cs

In addition the package specifies two extensions for the IOTA repository of tangle.net. Examples for the usage can be found here:
https://github.com/Felandil/tangle-.net/blob/develop/Tangle.Net.Standard/Tangle.Net.Area.Codes.Example/Repository.cs

## Class documentation
### IotaCodeArea
Representation of an IotaAreaCode in parsed form. Contains geolocation information about the area.
#### Members

    public int CodePrecision { get; set; }

    public double Latitude { get; set; }

    public double LatitudeHigh { get; set; }

    public double LatitudeLow { get; set; }

    public double Longitude { get; set; }

    public double LongitudeHigh { get; set; }

    public double LongitudeLow { get; set; }


### IotaAreaCode
#### Members
Area - 