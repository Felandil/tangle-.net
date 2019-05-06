namespace Tangle.Net.Area.Codes.Entity
{
  using System;
  using System.Linq;

  public class IotaAreaCodeDimension
  {
    internal static readonly int[] Precision = { 2, 4, 6, 8, 10, 11 };

    private IotaAreaCodeDimension()
    {
    }

    public double? BlocksSizeDegrees { get; private set; }

    public string BlocksSizeDegreesFormatted { get; private set; }

    public double SizeMetres { get; private set; }

    public string SizeMetresFormatted { get; private set; }

    public static IotaAreaCodeDimension GetByPrecision(int precision)
    {
      if (!Precision.Contains(precision))
      {
        throw new ArgumentException($"Invalid precision. Allowed values are {string.Join(", ", Precision)}");
      }

      switch (precision)
      {
        case 2:
          return new IotaAreaCodeDimension
                   {
                     BlocksSizeDegrees = 20, BlocksSizeDegreesFormatted = "20°", SizeMetres = 2200000, SizeMetresFormatted = "2200km"
                   };
        case 4:
          return new IotaAreaCodeDimension
                   {
                     BlocksSizeDegrees = 1, BlocksSizeDegreesFormatted = "1°", SizeMetres = 110000, SizeMetresFormatted = "110km"
                   };
        case 6:
          return new IotaAreaCodeDimension
                   {
                     BlocksSizeDegrees = 0.05, BlocksSizeDegreesFormatted = "0.05°", SizeMetres = 5500, SizeMetresFormatted = "5.5km"
                   };
        case 8:
          return new IotaAreaCodeDimension
                   {
                     BlocksSizeDegrees = 0.0025, BlocksSizeDegreesFormatted = "0.0025°", SizeMetres = 275, SizeMetresFormatted = "275m"
                   };
        case 10:
          return new IotaAreaCodeDimension
                   {
                     BlocksSizeDegrees = 0.000125, BlocksSizeDegreesFormatted = "0.000125°", SizeMetres = 14, SizeMetresFormatted = "14m"
                   };
        default:
          return new IotaAreaCodeDimension
                   {
                     BlocksSizeDegrees = null, BlocksSizeDegreesFormatted = string.Empty, SizeMetres = 3.5, SizeMetresFormatted = "3.5m"
                   };
      }
    }
  }
}