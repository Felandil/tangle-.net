// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Felandil IT">
//    Copyright (c) 2008 -2018 Felandil IT. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Tangle.Net.Source.Cryptography
{
  /// <summary>
  /// The extensions.
  /// </summary>
  public static class Extensions
  {
    #region Public Methods and Operators

    /// <summary>
    /// The slice.
    /// </summary>
    /// <param name="source">
    /// The source.
    /// </param>
    /// <param name="start">
    /// The start.
    /// </param>
    /// <param name="end">
    /// The end.
    /// </param>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    public static int[] Slice(this int[] source, int start, int end)
    {
      if (end < 0)
      {
        end = source.Length + end;
      }

      var len = end - start;

      var res = new int[len];
      for (var i = 0; i < len; i++)
      {
        res[i] = source[i + start];
      }

      return res;
    }

    #endregion
  }
}