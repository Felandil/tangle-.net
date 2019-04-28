namespace Tangle.Net.Area.Codes.Example
{
  using System;

  using Google.OpenLocationCode;

  using Newtonsoft.Json;

  using Tangle.Net.Area.Codes.Entity;

  class Program
  {
    static void Main(string[] args)
    {
      var areaCode = IotaAreaCode.Encode(52.529562, 13.413047);
      Console.WriteLine($"IOTA Area Code {areaCode}");

      var areaCodeHighPrecision = IotaAreaCode.Encode(52.529562, 13.413047, OpenLocationCode.CodePrecisionExtra);
      Console.WriteLine($"IOTA Area Code High Precision {areaCodeHighPrecision}");

      // Using the Decode() method instead of the Area property is possible as well
      var codeArea = new IotaAreaCode("NPHTQORL9XKP").Area;
      Console.WriteLine($"IOTA Code Area {JsonConvert.SerializeObject(codeArea, Formatting.Indented)}");

      // IotaAreaCode.ToOpenLocationCode("NPHTQORL9XKP") is possible as well
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

      var extracted = IotaAreaCode.Extract("NPHTQORL9XKP999999999");
      Console.WriteLine($"Extracted {extracted}");

      var dimensions = IotaAreaCodeDimension.GetByPrecision(4);
      Console.WriteLine($"Dimensions {JsonConvert.SerializeObject(dimensions, Formatting.Indented)}");

      var increasedPrecision = new IotaAreaCode("NPHTQORL9").IncreasePrecision();
      Console.WriteLine($"Increase Precision {increasedPrecision}");

      var decreasedPrecision = new IotaAreaCode("NPHTQORL9").DecreasePrecision();
      Console.WriteLine($"Decrease Precision {decreasedPrecision}");

      var setPrecision = new IotaAreaCode("NPHTQORL9").SetPrecision(4);
      Console.WriteLine($"Set Precision {setPrecision}");

      Console.WriteLine("----------------");

      Repository.FindAsync().Wait();

      Console.ReadKey();
    }
  }
}