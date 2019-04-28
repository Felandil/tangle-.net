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

<br>

# Class documentation
## IotaCodeArea
Representation of an IotaAreaCode in parsed form. Contains geolocation information about the area.
### Members

    // Precision of the area. See IotaAreaCodeDimension for mapping
    public int CodePrecision { get; set; } 

    // Latitude of the areas' center
    public double Latitude { get; set; }

    // Upper Latitude boundary
    public double LatitudeHigh { get; set; }

    // Lower Latitude boundary
    public double LatitudeLow { get; set; }

    // Longitude of the areas' center
    public double Longitude { get; set; }

    // Upper Longitude boundary
    public double LongitudeHigh { get; set; }

    // Lower Longitude boundary
    public double LongitudeLow { get; set; }



## IotaAreaCode
### Members
    // Referenced Area. See IotaCodeArea
    public IotaCodeArea Area

    // Precision of the area. See IotaAreaCodeDimension for mapping
    public int CodePrecision

### Methods
#### Encode
Encode a location into an IOTA Area Code.

    public static IotaAreaCode Encode(double latitude, double longitude, int precision = OpenLocationCode.CodePrecisionNormal)

| Param | Description |
| --- | --- |
| latitude | The latitude in signed decimal degrees. Values less than -90 will be clipped to -90, values over 90 will be clipped to 90. |
| longitude | The longitude in signed decimal degrees. This will be normalised to the range -180 to 180. |
| precision | The desired precision. Allowed values are 2, 4, 6, 8, 10, 11. If omitted, OpenLocationCode.CodePrecisionNormal will be used. For precision OpenLocationCode.CodePrecisionExtra is recommended. |

Example

    var areaCodeHighPrecision = IotaAreaCode.Encode(52.529562, 13.413047, OpenLocationCode.CodePrecisionExtra);

#### FromOpenLocationCode
Convert the Open Location Code to IOTA Area Code.

    public static IotaAreaCode FromOpenLocationCode(string openLocationCode)

| Param | Description |
| --- | --- |
| openLocationCode | The Open Location Code to convert. |

Example

    var areaCodeFromOpenLocationCode = IotaAreaCode.FromOpenLocationCode("X4HM+MM");

#### IsValid
Checks whether the given area code is valid

    public static bool IsValid(string iotaAreaCode)

| Param | Description |
| --- | --- |
| iotaAreaCode | The IOTA Area Code trytes to validate. |

Example

    var isValid = IotaAreaCode.IsValid("NPHTQORL9XKP");

#### IsValidPartial
Is the IOTA Area Code a valid partial code.

    public static bool IsValidPartial(string iotaAreaCode)

| Param | Description |
| --- | --- |
| iotaAreaCode | The IOTA Area Code trytes to validate. |

Example

    var isValidPartial = IotaAreaCode.IsValidPartial("NPAAAAAA9");

#### Extract
Extract an IOTA Area Code from trytes.

    public static IotaAreaCode Extract(string trytes)

| Param | Description |
| --- | --- |
| trytes | The trytes from which to try and extract the IOTA Area Code. |

Example

    var extracted = IotaAreaCode.Extract("NPHTQORL9XKP999999999");

#### ToOpenLocationCode
Convert the IOTA Area Code to Open Location Code. Has a static and class level implementation

    public static string ToOpenLocationCode(string iotaAreaCode)

| Param | Description |
| --- | --- |
| iotaAreaCode | The IOTA Area Code to convert. 

Example

    var areaCode = IotaAreaCode.ToOpenLocationCode("NPHTQORL9XKP")

Class level

    public string ToOpenLocationCode()

Example

    var openLocationCode = new IotaAreaCode("NPHTQORL9XKP").ToOpenLocationCode()

#### Decode
Decode an IOTA Area Code into a IotaCodeArea.

    public IotaCodeArea Decode()

Example

    var codeArea = new IotaAreaCode("NPHTQORL9XKP").Decode()

#### DecreasePrecision
Decrease the precision of an area code.

    public IotaAreaCode DecreasePrecision()

#### SetPrecision
Set the precision of an area code.

    public IotaAreaCode SetPrecision(int precision)

| Param | Description |
| --- | --- |
| precision | The new precision to set. |

#### IncreasePrecision
Increase the precision of an area code.

    public IotaAreaCode IncreasePrecision()

## IotaAreaCodeDimension
Display dimensions of an area. Class can not be initialized directly, needs to be retrieved by static method, see below.

### Members
    public double? BlocksSizeDegrees { get; private set; }

    public string BlocksSizeDegreesFormatted { get; private set; }

    public double SizeMetres { get; private set; }

    public string SizeMetresFormatted { get; private set; }

### Methods
Gets the dimensions for the corresponding precision.

    public static IotaAreaCodeDimension GetByPrecision(int precision)

Example

    var dimensions = IotaAreaCodeDimension.GetByPrecision(4);

## RepositoryExtension
### Methods
#### FindByAreaCodeAsync
Searches for transactions in the given area. The areaCode property is searched by tag internally.

    public static async Task<List<Bundle>> FindByAreaCodeAsync(IotaAreaCode areaCode)

Example

      // Create area code for a region by lat/lng and the precision
      // This example points to Hannover, Germany with a precision of 3.5m x 3.5m
      var areaCode = IotaAreaCode.Encode(52.37052, 9.73322, OpenLocationCode.CodePrecisionExtra);
      var bundles = await IotaRepository.FindByAreaCodeAsync(areaCode);

#### PublishWithAreaCodeAsync
Publishes a message with an area code attached. The area code is included as the tag of the bundles transactions

    public static async Task<Bundle> PublishWithAreaCodeAsync(TryteString message, IotaAreaCode areaCode, Address address = null)

Example

      var areaCode = IotaAreaCode.Encode(52.37052, 9.73322, OpenLocationCode.CodePrecisionExtra);
      var bundle = await IotaRepository.PublishWithAreaCodeAsync(
            TryteString.FromAsciiString("Hello from Hannover! \n Have fun with geo locations on the tangle!"),
            areaCode);
