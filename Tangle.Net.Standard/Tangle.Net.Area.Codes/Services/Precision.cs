namespace Tangle.Net.Area.Codes.Services
{
  using System.Linq;

  using Google.OpenLocationCode;

  using Tangle.Net.Area.Codes.Entity;

  internal static class Precision
  {
    public static string Calculate(IotaAreaCode areaCode, int newPrecision)
    {
      if (newPrecision == areaCode.CodePrecision)
      {
        return areaCode.Value;
      }

      if (newPrecision < areaCode.CodePrecision)
      {
        var reduced = areaCode.Value.Replace("9", string.Empty).Substring(0, newPrecision);
        if (newPrecision <= 8)
        {
          return $"{reduced}{string.Concat(Enumerable.Repeat('A', 8 - newPrecision))}9";
        }

        return $"{reduced.Substring(0, 8)}9{reduced.Substring(8)}";
      }

      return IotaAreaCode.FromOpenLocationCode(OpenLocationCode.Encode(areaCode.Area.Latitude, areaCode.Area.Longitude, newPrecision)).Value;
    }
  }
}