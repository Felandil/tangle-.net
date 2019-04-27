using System;

namespace Tangle.Net.Area.Codes.Example
{
  using Google.OpenLocationCode;

  using Newtonsoft.Json;

  using RestSharp.Serializers;

  using Tangle.Net.Area.Codes.Entity;

  class Program
  {
    static void Main(string[] args)
    {
      var areaCode = IotaAreaCode.Encode(52.529562, 13.413047);
      Console.WriteLine($"IOTA Area Code {areaCode}");

      var areaCodeHighPrecision = IotaAreaCode.Encode(52.529562, 13.413047, OpenLocationCode.CodePrecisionExtra);
      Console.WriteLine($"IOTA Area Code High Precision {areaCodeHighPrecision}");

      // Using the Decode() method instead of the Area property would be possible as well
      var codeArea = new IotaAreaCode("NPHTQORL9XKP").Area;
      Console.WriteLine($"IOTA Code Area {JsonConvert.SerializeObject(codeArea, Formatting.Indented)}");

      // IotaAreaCode.ToOpenLocationCode("NPHTQORL9XKP") would be possible aswell
      var openLocationCode = new IotaAreaCode("NPHTQORL9XKP").ToOpenLocationCode();
      Console.WriteLine($"Open Location Code {openLocationCode}");

      var areaCodeFromOpenLocationCode = IotaAreaCode.FromOpenLocationCode("X4HM+MM");
      Console.WriteLine($"IOTA Area Code {areaCodeFromOpenLocationCode}");

      var isValid1 = IotaAreaCode.IsValid("JAHAS0");
      Console.WriteLine($"isValid1 {isValid1}");

      var isValid2 = IotaAreaCode.IsValid("NPHTQORL9XKP");
      Console.WriteLine($"isValid2 {isValid2}");

      var isValidPartial1 = IotaAreaCode.IsValidPartial("JAHAS");
      Console.WriteLine($"isValidPartial1 {isValidPartial1}");

      var isValidPartial2 = IotaAreaCode.IsValidPartial("NPAAAAAA9");
      Console.WriteLine($"isValidPartial2 {isValidPartial2}");

      Console.ReadKey();
    }

    //const extracted = iotaAreaCodes.extract('NPHTQORL9XKP999999999');
    //console.log("extracted", extracted);

    //const dimensions = iotaAreaCodes.getPrecisionDimensions(4);
    //console.log("dimensions", dimensions);

    //const increasePrecision1 = iotaAreaCodes.increasePrecision('NPHTQORL9');
    //console.log("increasePrecision1", increasePrecision1);

    //const decreasePrecision1 = iotaAreaCodes.decreasePrecision('NPHTQORL9');
    //console.log("decreasePrecision1", decreasePrecision1);

    //const setPrecision1 = iotaAreaCodes.setPrecision('NPHTQORL9', 4);
    //console.log("setPrecision", setPrecision1);
  }
}
