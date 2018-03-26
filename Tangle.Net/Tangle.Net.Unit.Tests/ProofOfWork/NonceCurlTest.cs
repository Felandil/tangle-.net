namespace Tangle.Net.Unit.Tests.ProofOfWork
{
  using System;
  using System.Collections.Generic;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Entity;
  using Tangle.Net.ProofOfWork;

  /// <summary>
  /// The nonce curl test.
  /// </summary>
  [TestClass]
  public class NonceCurlTest
  {
    /// <summary>
    /// The expected after increment.
    /// </summary>
    private readonly List<Tuple<ulong, ulong>> expectedBeforeIncrement = new List<Tuple<ulong, ulong>>
                                                                          {
                                                                            new Tuple<ulong, ulong>(
                                                                              15811494920322472813,
                                                                              13176245766935394011),
                                                                            new Tuple<ulong, ulong>(
                                                                              17941353825114769379,
                                                                              14403622084951293727),
                                                                            new Tuple<ulong, ulong>(
                                                                              576458557575118879,
                                                                              18445620372817592319),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446741876833779711,
                                                                              2199023255551),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              0),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              18446744073709551615,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                            new Tuple<ulong, ulong>(
                                                                              0,
                                                                              18446744073709551615),
                                                                          };

    /// <summary>
    /// The test increment.
    /// </summary>
    [TestMethod]
    public void TestIncrement()
    {
      var seed = new Seed("L9DRGFPYDMGVLH9ZCEWHXNEPC9TQQSA9W9FZVYXLBMJTHJC9HZDONEJMMVJVEMHWCIBLAUYBAUFQOMYSN");
      var helperCurl = new Curl(CurlMode.CurlP27);
      helperCurl.Absorb(seed.ToTrits());

      var helperState = helperCurl.Rate(Curl.StateLength);

      var nonceCurl = new NonceCurl(Curl.StateLength, (int)CurlMode.CurlP27);
      nonceCurl.Absorb(helperState, helperState.Length, 0);

      nonceCurl.High[0] = 13176245766935394011;
      nonceCurl.High[1] = 14403622084951293727;
      nonceCurl.High[2] = 18445620372817592319;
      nonceCurl.High[3] = 2199023255551;

      nonceCurl.Low[0] = 15811494920322472813;
      nonceCurl.Low[1] = 17941353825114769379;
      nonceCurl.Low[2] = 576458557575118879;
      nonceCurl.Low[3] = 18446741876833779711;

      for (var i = 0; i < this.expectedBeforeIncrement.Count; i++)
      {
        Assert.AreEqual(nonceCurl.Low[i], this.expectedBeforeIncrement[i].Item1);
        Assert.AreEqual(nonceCurl.High[i], this.expectedBeforeIncrement[i].Item2);
      }

      var incrementOffset = nonceCurl.Increment(0, nonceCurl.High.Length);
      Assert.AreEqual(729, incrementOffset);
    }
  }
}