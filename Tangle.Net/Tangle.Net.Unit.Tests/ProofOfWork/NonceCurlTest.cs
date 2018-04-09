﻿namespace Tangle.Net.Unit.Tests.ProofOfWork
{
  using System;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.ProofOfWork.HammingNonce;

  /// <summary>
  /// The nonce curl test.
  /// </summary>
  [TestClass]
  public class NonceCurlTest
  {
    /// <summary>
    /// The expected after increment.
    /// </summary>
    private readonly Tuple<ulong, ulong>[] expectedAfterIncrement =
      {
        new Tuple<ulong, ulong>(7905747460161236406, 15811494920322472813), new Tuple<ulong, ulong>(4548512237353040124, 17941353825114769379),
        new Tuple<ulong, ulong>(17871409217026392032, 576458557575118879), new Tuple<ulong, ulong>(18446744071562067968, 18446741876833779711),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 0),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615), new Tuple<ulong, ulong>(0, 18446744073709551615),
        new Tuple<ulong, ulong>(18446744073709551615, 0), new Tuple<ulong, ulong>(18446744073709551615, 18446744073709551615),
        new Tuple<ulong, ulong>(0, 18446744073709551615)
      };

    /// <summary>
    /// The expected after transform.
    /// </summary>
    private readonly Tuple<ulong, ulong>[] expectedAfterTransform =
      {
        new Tuple<ulong, ulong>(17211912508998645487, 15794103894430906773), new Tuple<ulong, ulong>(17199244191104461819, 4421794518878582365),
        new Tuple<ulong, ulong>(18338639505939341052, 16711619995798332867), new Tuple<ulong, ulong>(18094648297263726503, 568576704109093727),
        new Tuple<ulong, ulong>(4563368716304382910, 16140829995364525405), new Tuple<ulong, ulong>(4467406999039531441, 17506544744278783742),
        new Tuple<ulong, ulong>(6537453934283569628, 13245050610517835515), new Tuple<ulong, ulong>(4278339273536504666, 16060820469342797295),
        new Tuple<ulong, ulong>(8853688306848223718, 10338007987422572473), new Tuple<ulong, ulong>(10865490155368429503, 7583647632548945856),
        new Tuple<ulong, ulong>(12532879731518888887, 6211305003921964767), new Tuple<ulong, ulong>(15851894958175858679, 4504439507923238237),
        new Tuple<ulong, ulong>(6214397521795537885, 18297931141611482791), new Tuple<ulong, ulong>(17385792309038126583, 4538293764505955848),
        new Tuple<ulong, ulong>(8832394170853151705, 18262036549282905398), new Tuple<ulong, ulong>(14834776751040061372, 17808758451186886635),
        new Tuple<ulong, ulong>(17996351054717841007, 14323908598954926039), new Tuple<ulong, ulong>(8662175524734887419, 18432606335804496812),
        new Tuple<ulong, ulong>(17286418354661224175, 15418880902572603166), new Tuple<ulong, ulong>(8643427893981929101, 11272335798552977790),
        new Tuple<ulong, ulong>(3328648269117677055, 18014335397075090079), new Tuple<ulong, ulong>(3637731861455911927, 18416835630318024027),
        new Tuple<ulong, ulong>(17383155441930679784, 1143616877706340951), new Tuple<ulong, ulong>(1005426208892702123, 18050378077315120765),
        new Tuple<ulong, ulong>(17257470966329308381, 10800667232988285799), new Tuple<ulong, ulong>(13221387993392573423, 16853802137480451093),
        new Tuple<ulong, ulong>(2200571070737906009, 17545962438005866222), new Tuple<ulong, ulong>(17484906546981154769, 12671281344935853502),
        new Tuple<ulong, ulong>(13591755572133035895, 18440550523663013544), new Tuple<ulong, ulong>(18070564572019785727, 9042380825682887609),
        new Tuple<ulong, ulong>(17226264166957463287, 8790592502613146895), new Tuple<ulong, ulong>(18423799078494076527, 12534535751308901788),
        new Tuple<ulong, ulong>(11358367413914160458, 7674128473260980157), new Tuple<ulong, ulong>(14807483180636698570, 13733670977816820671),
        new Tuple<ulong, ulong>(13666660530116124570, 17753150881720433765), new Tuple<ulong, ulong>(18112474691644686307, 13835044016977482078),
        new Tuple<ulong, ulong>(15939786639032056219, 7989027870435374951), new Tuple<ulong, ulong>(11113770985566361903, 9209850239952840666),
        new Tuple<ulong, ulong>(6123135432733809551, 12341786009512179197), new Tuple<ulong, ulong>(11987367152139869527, 18279787215823107770),
        new Tuple<ulong, ulong>(4385585115002486515, 15523881333906328559), new Tuple<ulong, ulong>(17126470222743969199, 10545053731843172215),
        new Tuple<ulong, ulong>(13208907884508472150, 17147066832912137727), new Tuple<ulong, ulong>(15555432833581821918, 9133087045051076711),
        new Tuple<ulong, ulong>(17651936959330735087, 1143914302529388058), new Tuple<ulong, ulong>(17813737461310717759, 5462000137081556193),
        new Tuple<ulong, ulong>(14050524597662938039, 4571873642881322575), new Tuple<ulong, ulong>(13420416676508129884, 15994427145266374115),
        new Tuple<ulong, ulong>(17255881338033655271, 13463224499205814201), new Tuple<ulong, ulong>(15846980292120075388, 8646769940358987671),
        new Tuple<ulong, ulong>(3230205115562314918, 15793989684049966079), new Tuple<ulong, ulong>(6608697082708491455, 18442167656937774957),
        new Tuple<ulong, ulong>(16703212400235887543, 11202527996327937613), new Tuple<ulong, ulong>(2990388936993234303, 16037517421688368892),
        new Tuple<ulong, ulong>(18446571449287044599, 3196886627938408312), new Tuple<ulong, ulong>(17267070874259226597, 11087860729384748027),
        new Tuple<ulong, ulong>(11033674483466049339, 7486561053077597671), new Tuple<ulong, ulong>(17401036890671401727, 11501942483477004086),
        new Tuple<ulong, ulong>(18359903773466678015, 861753074600570224), new Tuple<ulong, ulong>(16387182584634634747, 4375049054895468254),
        new Tuple<ulong, ulong>(7189429453026401873, 11455749523079259055), new Tuple<ulong, ulong>(2017163685410242007, 16474057287920441149),
        new Tuple<ulong, ulong>(8643897725104943088, 9856961012450646031), new Tuple<ulong, ulong>(8299271394002235712, 12456768241399733951),
        new Tuple<ulong, ulong>(11164949037200373414, 7921662479338266619), new Tuple<ulong, ulong>(18317590781212753880, 3011499662304392999),
        new Tuple<ulong, ulong>(3093390132777910165, 15392106234861056107), new Tuple<ulong, ulong>(18122202978728139502, 2159367161576564723),
        new Tuple<ulong, ulong>(18296957443704481776, 13525479172635286911), new Tuple<ulong, ulong>(12608934845917687374, 15658411798153762225),
        new Tuple<ulong, ulong>(17143895085498812671, 13435828533854713681), new Tuple<ulong, ulong>(8613099092433099217, 11384461945657442030),
        new Tuple<ulong, ulong>(18413951551101959995, 13078434560374799317), new Tuple<ulong, ulong>(8946292081798668199, 13258442200967704031),
        new Tuple<ulong, ulong>(2105968242692969406, 16349825019302227915), new Tuple<ulong, ulong>(16134971525273866162, 9013700613030523869),
        new Tuple<ulong, ulong>(4012194866577401191, 18013267965879450361), new Tuple<ulong, ulong>(12310369121817493095, 15546142797599848380),
        new Tuple<ulong, ulong>(12053378796391982011, 8917109532265979613), new Tuple<ulong, ulong>(13492677270388038495, 5113397445623785392),
        new Tuple<ulong, ulong>(16950941975433320405, 8419389031818328254), new Tuple<ulong, ulong>(18410004917461506419, 4313234399643793343),
        new Tuple<ulong, ulong>(17581911026024958267, 15878836418970384124), new Tuple<ulong, ulong>(18152404216011554783, 3951834574320287905),
        new Tuple<ulong, ulong>(13686436475696283630, 7725418146238944511), new Tuple<ulong, ulong>(6525706678353939286, 12103947210540056057),
        new Tuple<ulong, ulong>(12960830312109678587, 7954478177508024335), new Tuple<ulong, ulong>(17788645947447572021, 7926175790229798862),
        new Tuple<ulong, ulong>(6771105891373413229, 12258085675105402326), new Tuple<ulong, ulong>(15805093705196452572, 13761777460122010987),
        new Tuple<ulong, ulong>(1976516667144834665, 18302548338739508662), new Tuple<ulong, ulong>(6617283169763648644, 16585550840284790783),
        new Tuple<ulong, ulong>(18407960275930041948, 6169309092297504171), new Tuple<ulong, ulong>(14860282165534825263, 13690869887109431039),
        new Tuple<ulong, ulong>(4341311436169925823, 15550043830384831355), new Tuple<ulong, ulong>(16712346626028135420, 18121356020460403907),
        new Tuple<ulong, ulong>(18292354758133930424, 15559890395559882319), new Tuple<ulong, ulong>(15130898366797409753, 17171652663319378486),
        new Tuple<ulong, ulong>(17566701337170541078, 15904179462252474351), new Tuple<ulong, ulong>(8570313204431303910, 11365728584244919131),
        new Tuple<ulong, ulong>(17957771795410959798, 6907868694275480171), new Tuple<ulong, ulong>(12970025694926209528, 17090375946881203951),
        new Tuple<ulong, ulong>(3170478610097735263, 15300333220439489466), new Tuple<ulong, ulong>(12577143253321577439, 15419621287168015462),
        new Tuple<ulong, ulong>(5391625267644540140, 17671312925327810483), new Tuple<ulong, ulong>(15725789480297409237, 7338307565778665387),
        new Tuple<ulong, ulong>(15982915398642662647, 12301582372866736026), new Tuple<ulong, ulong>(17451413189656230755, 11519149221059581439),
        new Tuple<ulong, ulong>(4610268674630614781, 14293646305626021279), new Tuple<ulong, ulong>(17603703665493229492, 18427462896058299215),
        new Tuple<ulong, ulong>(18301466670368881381, 13118751965029539742), new Tuple<ulong, ulong>(13048610269196465880, 18156947140409023295),
        new Tuple<ulong, ulong>(9151244020024859453, 17828001797580633794), new Tuple<ulong, ulong>(18146600421399926551, 15888272874476793581),
        new Tuple<ulong, ulong>(13229604621086555990, 8898841224173410991), new Tuple<ulong, ulong>(18220044442510671326, 14066983117127933485),
        new Tuple<ulong, ulong>(14622505368213759830, 4600286281869818623), new Tuple<ulong, ulong>(6586478681436926329, 13184355715802582943),
        new Tuple<ulong, ulong>(17559953435569129181, 4566556989873355575), new Tuple<ulong, ulong>(8614541229489913831, 18409433683439066490),
        new Tuple<ulong, ulong>(18428270592561618911, 5396930467613633584), new Tuple<ulong, ulong>(17707898200085463029, 7662289732658788890),
        new Tuple<ulong, ulong>(4607118628703167910, 13950460864875084669), new Tuple<ulong, ulong>(15830985793043556459, 7845163137999536063),
        new Tuple<ulong, ulong>(18428013454202762073, 8091714537351737071), new Tuple<ulong, ulong>(5730223595409293247, 18337228300146269566),
        new Tuple<ulong, ulong>(12926364466288670313, 18374079437583810015), new Tuple<ulong, ulong>(5678251369885289160, 17524312052370110455),
        new Tuple<ulong, ulong>(4534771342242623165, 15265900914863625198), new Tuple<ulong, ulong>(17755149907225433842, 10070047920687462687),
        new Tuple<ulong, ulong>(17974990875473426294, 2854295957099511499), new Tuple<ulong, ulong>(3476444501198734922, 17288605349804243391),
        new Tuple<ulong, ulong>(17291103240078192379, 1629722521722666326), new Tuple<ulong, ulong>(12317199192385639807, 15932529041947933675),
        new Tuple<ulong, ulong>(18153231620062945215, 3312103430508936018), new Tuple<ulong, ulong>(18403414759814852558, 4312742000960893503),
        new Tuple<ulong, ulong>(13400443058776149267, 15492063859679654639), new Tuple<ulong, ulong>(17783209825809684463, 1855337823539083956),
        new Tuple<ulong, ulong>(4308903329762038710, 14914721140230159743), new Tuple<ulong, ulong>(10694527387883664281, 18156116478601124974),
        new Tuple<ulong, ulong>(8189736372081244155, 16094869939117907902), new Tuple<ulong, ulong>(17236885278975128930, 11383545629266893565),
        new Tuple<ulong, ulong>(18083280617225427967, 15393300502558764990), new Tuple<ulong, ulong>(11222968617356229212, 16714456595144056315),
        new Tuple<ulong, ulong>(2253548567601149695, 16338458898539166677), new Tuple<ulong, ulong>(10297467884269337823, 8887871377620236066),
        new Tuple<ulong, ulong>(12208491009469814248, 6818374648131907423), new Tuple<ulong, ulong>(11708829778514999368, 15991692919196960703),
        new Tuple<ulong, ulong>(13752141537539878905, 7325095356702637799), new Tuple<ulong, ulong>(4219859067745337339, 16680874033450288765),
        new Tuple<ulong, ulong>(10349153194147027966, 17576262658988830269), new Tuple<ulong, ulong>(4025408824633196259, 17237231268251287839),
        new Tuple<ulong, ulong>(1010961900232753150, 17724997888950268407), new Tuple<ulong, ulong>(18374641261651786187, 14638859556489766716),
        new Tuple<ulong, ulong>(3384650727230332907, 15800685224375824158), new Tuple<ulong, ulong>(14842456687338845549, 12837283671978031763),
        new Tuple<ulong, ulong>(9068968426924261091, 16138596247135514044), new Tuple<ulong, ulong>(17243035964760815479, 6466446848296987082),
        new Tuple<ulong, ulong>(15175545922750808319, 12679851729070587776), new Tuple<ulong, ulong>(16139632051757141950, 17410043340952301763),
        new Tuple<ulong, ulong>(18300218355884486681, 2290842784181936110), new Tuple<ulong, ulong>(14698359125825220189, 13219739747594244539),
        new Tuple<ulong, ulong>(3383986152086419932, 17725533577615955583), new Tuple<ulong, ulong>(17288957097223519921, 15760286835018746206),
        new Tuple<ulong, ulong>(16927816020347178837, 4447116885407513338), new Tuple<ulong, ulong>(17221188259265534108, 4467130809299238755),
        new Tuple<ulong, ulong>(12384616389673858109, 15993139036705783790), new Tuple<ulong, ulong>(10227373669032638311, 8860715683885477565),
        new Tuple<ulong, ulong>(17680910308766362962, 18443062822871102463), new Tuple<ulong, ulong>(8601836234094884847, 13471810129325246610),
        new Tuple<ulong, ulong>(17914701103307348631, 17249831623851360108), new Tuple<ulong, ulong>(13521880786186267995, 6916109635036312300),
        new Tuple<ulong, ulong>(17149139207721806650, 17497485937905424071), new Tuple<ulong, ulong>(15841338125036014591, 3944124040682011777),
        new Tuple<ulong, ulong>(11313027693400244208, 8641244130984324799), new Tuple<ulong, ulong>(13796389154078053482, 7831734925637566453),
        new Tuple<ulong, ulong>(6741313994825358319, 11708070282470668121), new Tuple<ulong, ulong>(18001375553832213403, 7997915131385161581),
        new Tuple<ulong, ulong>(5748066731291442633, 12879133830608460479), new Tuple<ulong, ulong>(8839192112161093211, 13212976225816714663),
        new Tuple<ulong, ulong>(16752698398519322450, 8621998120589819375), new Tuple<ulong, ulong>(13950725201042845695, 13796669674178573168),
        new Tuple<ulong, ulong>(18085442592629059235, 2872135445334497628), new Tuple<ulong, ulong>(18009894718594127870, 1680657668532595287),
        new Tuple<ulong, ulong>(6773382344423354782, 16287794372341026685), new Tuple<ulong, ulong>(4549664009768926137, 14626845746261466855),
        new Tuple<ulong, ulong>(6300531166994487087, 13742739141586561756), new Tuple<ulong, ulong>(18441955372875121126, 10717309062699023897),
        new Tuple<ulong, ulong>(14948851054813330719, 4466435855278731232), new Tuple<ulong, ulong>(16390392774609104638, 4521033202945226531),
        new Tuple<ulong, ulong>(8177340470439667699, 12584428088943155454), new Tuple<ulong, ulong>(12818886590759373851, 18419422108447014903),
        new Tuple<ulong, ulong>(8194789661497673151, 16055137228248578777), new Tuple<ulong, ulong>(15696128613071409131, 9164330734015083902),
        new Tuple<ulong, ulong>(15482348432352345256, 13397064761444928479), new Tuple<ulong, ulong>(18441924482864920993, 16003857922289555423),
        new Tuple<ulong, ulong>(16118692008621932999, 3197246828075515452), new Tuple<ulong, ulong>(15846117041946492345, 2816962936739630790),
        new Tuple<ulong, ulong>(18441905466360332285, 17634829474807578875), new Tuple<ulong, ulong>(6883404546090974847, 13617125616107354077),
        new Tuple<ulong, ulong>(17831296760348780348, 10158615386192870895), new Tuple<ulong, ulong>(6807181453046503285, 11811674755156619003),
        new Tuple<ulong, ulong>(14389000723197050797, 8959049849413754366), new Tuple<ulong, ulong>(11942102239471353567, 6843047531596600746),
        new Tuple<ulong, ulong>(5473558877926566803, 13069137943811422078), new Tuple<ulong, ulong>(7452431813805291743, 13447667971788431136),
        new Tuple<ulong, ulong>(18146676284186876893, 14712080158720636666), new Tuple<ulong, ulong>(11913119959082934239, 16050786905163295736),
        new Tuple<ulong, ulong>(15948713028170431097, 11812882873091745703), new Tuple<ulong, ulong>(15528264378064043855, 16840874176799974392),
        new Tuple<ulong, ulong>(17034495938688466935, 6313905900871739851), new Tuple<ulong, ulong>(17149707364315271923, 4259109730734997407),
        new Tuple<ulong, ulong>(15888389310462654952, 4309341616275379991), new Tuple<ulong, ulong>(2100224644227200839, 16923277226725982205),
        new Tuple<ulong, ulong>(18356551062136735697, 11948030148933533615), new Tuple<ulong, ulong>(13829828707616485603, 17852257513110970238),
        new Tuple<ulong, ulong>(13778646404194146974, 4957895677180637173), new Tuple<ulong, ulong>(2982155323565931246, 17869700561150426417),
        new Tuple<ulong, ulong>(17098758587325284338, 12877312979288028303), new Tuple<ulong, ulong>(18041257213999447294, 13827904321515145073),
        new Tuple<ulong, ulong>(18446629446328706167, 13652092134912524271), new Tuple<ulong, ulong>(13801703149519154682, 16284981469771584095),
        new Tuple<ulong, ulong>(9208155619989519787, 10031556832455546868), new Tuple<ulong, ulong>(12501780909588870943, 8620141402765638646),
        new Tuple<ulong, ulong>(11136228637974437860, 17725745782882467707), new Tuple<ulong, ulong>(18239000694670480858, 3380828823191629631),
        new Tuple<ulong, ulong>(14977634962796034009, 18205229607869213550), new Tuple<ulong, ulong>(12805808507311874024, 5640947357167186175),
        new Tuple<ulong, ulong>(18397201667804041703, 3238570538138992285), new Tuple<ulong, ulong>(12329709498724644836, 17867072730270719807),
        new Tuple<ulong, ulong>(15842643037489380707, 2714535329133330108), new Tuple<ulong, ulong>(13192923791182466031, 17292129007260073916),
        new Tuple<ulong, ulong>(6031711597070314671, 18446514737578490736), new Tuple<ulong, ulong>(6243104320606929371, 12501145661238935542),
        new Tuple<ulong, ulong>(8754992587410350937, 12670188037326503854), new Tuple<ulong, ulong>(8459420070055509915, 17193369465564117111),
        new Tuple<ulong, ulong>(18134130439879915645, 10159828272022423503), new Tuple<ulong, ulong>(17221506173735959267, 6712874724832442206),
        new Tuple<ulong, ulong>(15373024026976313299, 12681448930386360493), new Tuple<ulong, ulong>(8621498818428403396, 16743238538953288123),
        new Tuple<ulong, ulong>(4510071277572894703, 17291878615362295743), new Tuple<ulong, ulong>(7617737474872940750, 15441154168212907831),
        new Tuple<ulong, ulong>(8841641313037047803, 13829779830210752494)
      };

    /// <summary>
    /// The test increment.
    /// </summary>
    [TestMethod]
    public void TestIncrement()
    {
      var trits = Converter.TrytesToTrits("ADHMOICIPKGYHYL9VMLSSXHGKTUTEQUTIWUQWSVYHZWTAHNIYQICEJWFTCYBGRGRM9DWBCGDELIGEIIIH");
      var ulongTrits = UlongTritConverter.TritsToUlong(trits, this.expectedAfterIncrement.Length, Mode._64bit);

      ulongTrits.Low[0] = 15811494920322472813;
      ulongTrits.Low[1] = 17941353825114769379;
      ulongTrits.Low[2] = 576458557575118879;
      ulongTrits.Low[3] = 18446741876833779711;

      ulongTrits.High[0] = 13176245766935394011;
      ulongTrits.High[1] = 14403622084951293727;
      ulongTrits.High[2] = 18445620372817592319;
      ulongTrits.High[3] = 2199023255551;

      var curl = new NonceCurl(ulongTrits.Low, ulongTrits.High, (int)CurlMode.CurlP27);
      var incrementResult = curl.Increment(0, curl.Low.Length);

      Assert.AreEqual(729, incrementResult);

      for (var i = 0; i < this.expectedAfterIncrement.Length; i++)
      {
        Assert.AreEqual(this.expectedAfterIncrement[i].Item1, curl.Low[i]);
        Assert.AreEqual(this.expectedAfterIncrement[i].Item2, curl.High[i]);
      }
    }

    /// <summary>
    /// The test transform.
    /// </summary>
    [TestMethod]
    public void TestTransform()
    {
      var trits = Converter.TrytesToTrits("ADHMOICIPKGYHYL9VMLSSXHGKTUTEQUTIWUQWSVYHZWTAHNIYQICEJWFTCYBGRGRM9DWBCGDELIGEIIIH");
      var ulongTrits = UlongTritConverter.TritsToUlong(trits, Curl.StateLength, Mode._64bit);

      ulongTrits.Low[0] = 15811494920322472813;
      ulongTrits.Low[1] = 17941353825114769379;
      ulongTrits.Low[2] = 576458557575118879;
      ulongTrits.Low[3] = 18446741876833779711;

      ulongTrits.High[0] = 13176245766935394011;
      ulongTrits.High[1] = 14403622084951293727;
      ulongTrits.High[2] = 18445620372817592319;
      ulongTrits.High[3] = 2199023255551;

      var curl = new NonceCurl(ulongTrits.Low, ulongTrits.High, (int)CurlMode.CurlP27);
      curl.Transform();

      for (var i = 0; i < this.expectedAfterTransform.Length; i++)
      {
        Assert.AreEqual(this.expectedAfterTransform[i].Item1, curl.Low[i]);
        Assert.AreEqual(this.expectedAfterTransform[i].Item2, curl.High[i]);
      }
    }
  }
}