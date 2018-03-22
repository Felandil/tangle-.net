namespace Tangle.Net.Unit.Tests.Repository
{
  using System;
  using System.Collections.Generic;
  using System.Net;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Moq;

  using RestSharp;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.Repository;
  using Tangle.Net.Repository.Client;
  using Tangle.Net.Repository.DataTransfer;
  using Tangle.Net.Repository.Responses;
  using Tangle.Net.Unit.Tests.Cryptography;
  using Tangle.Net.Utils;

  /// <summary>
  /// The rest iota repository test.
  /// </summary>
  [TestClass]
  public class RestIotaRepositoryTest
  {
    #region Constants

    /// <summary>
    /// The transaction trytes with pow done.
    /// </summary>
    private const string TransactionTrytesWithPoWDone =
      "RBTC9D9DCDEAKDCDFD9DSCFA999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999YTXCUUWTXIXVRQIDSECVFRTKAFOEZITGDPLWYVUVFURMNVDPIRXEIQN9JHNFNVKVJMQVMA9GDZJROTSFZ999999999999999999999999999PYHARP9999999999999999999999ERESYD99999999999999999999VWBJK9VWFRBHNCRKYAYXZODZIYCXHK9GBRUTIHCGEAKTQLXEBTYNZYARTAEYJNTFCJWFJO9TKKDQFUIGDCXTEVCPNIEMITBQAVKWAQNZMVCHOZPTNQHGHQPSQDXEGWKLHDXLZUDEVOBONAEJJTFDGDRAXDVBUZ9999QINJZUOQGRRIKZJIKWJRBHVLQPKKGKGRODPMJXZZVZKNNVIGTYELWSYWRESO9JNZYSJBVYANLFVIZ9999CSHARP999999999999999999999ZPSCCE9JE999999999L999999999YVUAXJNPWDKHPOVOQOEBY9YINF";

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The test attach to tangle.
    /// </summary>
    [TestMethod]
    public void TestAttachToTangle()
    {
      var seed = new Seed("QLWOWK9NBXOT9IJ9FXEAPGAHEQDDVMTQXL9DD9VOSTXKSFUABIO9AETEAJPZSNEEPQMJDLWTTMOFDVVBE");
      var bundle = new Bundle();
      bundle.AddTransfer(
        new Transfer
          {
            Address =
              new Address("YTXCUUWTXIXVRQIDSECVFRTKAFOEZITGDPLWYVUVFURMNVDPIRXEIQN9JHNFNVKVJMQVMA9GDZJROTSFZHIVJOVAEC") { Balance = 0, PrivateKey = new PrivateKeyStub() }, 
            Message = TryteString.FromAsciiString("Hello world!"), 
            Tag = new Tag("CSHARP"), 
            Timestamp = Timestamp.UnixSecondsTimestamp
          });

      bundle.Finalize();
      bundle.Sign();

      var branchTransaction = new Hash("QINJZUOQGRRIKZJIKWJRBHVLQPKKGKGRODPMJXZZVZKNNVIGTYELWSYWRESO9JNZYSJBVYANLFVIZ9999");
      var trunkTransaction = new Hash("CXTEVCPNIEMITBQAVKWAQNZMVCHOZPTNQHGHQPSQDXEGWKLHDXLZUDEVOBONAEJJTFDGDRAXDVBUZ9999");

      var restClientMock = new Mock<IRestClient>();
      restClientMock.Setup(r => r.Execute<AttachToTangleResponse>(It.IsAny<IRestRequest>())).Returns(
        () =>
          {
            var restResponse = new RestResponse<AttachToTangleResponse>
                                 {
                                   StatusCode = HttpStatusCode.OK, 
                                   Data =
                                     new AttachToTangleResponse
                                       {
                                         Trytes =
                                           new List<string>
                                             {
                                               TransactionTrytesWithPoWDone
                                             }
                                       }
                                 };
            return restResponse;
          });

      var repository = new RestIotaRepository(restClientMock.Object);
      var result = repository.AttachToTangle(branchTransaction, trunkTransaction, bundle.Transactions);

      Assert.AreEqual(TransactionTrytesWithPoWDone, result[0].Value);
    }

    /// <summary>
    /// The test find transactions does not contain parameters should throw exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestFindTransactionsDoesNotContainParametersShouldThrowException()
    {
      var restClientMock = new Mock<IRestClient>();
      var repository = new RestIotaRepository(restClientMock.Object);
      repository.FindTransactions(new Dictionary<string, IEnumerable<TryteString>>());
    }

    /// <summary>
    /// The test find transactions parameters include invalid parameter names should throw exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestFindTransactionsParametersIncludeInvalidParameterNamesShouldThrowException()
    {
      var restClientMock = new Mock<IRestClient>();
      var repository = new RestIotaRepository(restClientMock.Object);
      repository.FindTransactions(
        new Dictionary<string, IEnumerable<TryteString>> { { "invalid", new List<TryteString>() } });
    }

    /// <summary>
    /// The test find transactions with given parameters but some are empty should throw exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestFindTransactionsWithGivenParametersButSomeAreEmptyShouldThrowException()
    {
      var restClientMock = new Mock<IRestClient>();

      var repository = new RestIotaRepository(restClientMock.Object);
      repository.FindTransactions(
        new Dictionary<string, IEnumerable<TryteString>>
          {
            { "bundles", new List<TryteString>() },
            { "addresses", new List<TryteString>() },
            { "tags", new List<TryteString>() },
            { "approvees", new List<TryteString>() }
          });
    }

    /// <summary>
    /// The test find transactions parameters include invalid parameter names should throw exception.
    /// </summary>
    [TestMethod]
    public void TestFindTransactionsWithValidParametersShouldReturnHashes()
    {
      var restClientMock = new Mock<IRestClient>();
      restClientMock.Setup(r => r.Execute<GetTransactionsResponse>(It.IsAny<IRestRequest>())).Returns(
        () =>
          {
            var restResponse = new RestResponse<GetTransactionsResponse>
                                 {
                                   StatusCode = HttpStatusCode.OK, 
                                   Data =
                                     new GetTransactionsResponse
                                       {
                                         Hashes =
                                           new List<string>
                                             {
                                               "EJEAOOZYSAWFPZQESYDHZCGYNSTWXUMVJOVDWUNZJXDGWCLUFGIMZRMGCAZGKNPLBRLGUNYWKLJTYEAQX"
                                             }
                                       }
                                 };
            return restResponse;
          });

      var repository = new RestIotaRepository(restClientMock.Object);
      var transactions =
        repository.FindTransactions(
          new Dictionary<string, IEnumerable<TryteString>>
            {
              {
                "bundles", 
                new List<TryteString>
                  {
                    new TryteString(
                      "RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB")
                  }
              }, 
              {
                "addresses", 
                new List<TryteString>
                  {
                    new TryteString(
                      "RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB")
                  }
              }, 
              {
                "tags", 
                new List<TryteString>
                  {
                    new TryteString(
                      "RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB")
                  }
              }, 
              {
                "approvees", 
                new List<TryteString>
                  {
                    new TryteString(
                      "RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB")
                  }
              }
            });

      Assert.AreEqual("EJEAOOZYSAWFPZQESYDHZCGYNSTWXUMVJOVDWUNZJXDGWCLUFGIMZRMGCAZGKNPLBRLGUNYWKLJTYEAQX", transactions.Hashes[0].Value);
    }

    /// <summary>
    /// The test get balances should include all parameters.
    /// </summary>
    [TestMethod]
    public void TestGetBalancesShouldIncludeAllParameters()
    {
      var restClientMock = new Mock<IRestClient>();
      restClientMock.Setup(r => r.Execute<GetBalanceResponse>(It.IsAny<IRestRequest>())).Returns(
        () =>
          {
            var restResponse = new RestResponse<GetBalanceResponse>
                                 {
                                   StatusCode = HttpStatusCode.OK, 
                                   Data =
                                     new GetBalanceResponse
                                       {
                                         Balances = new List<long> { 10000 }, 
                                         Duration = 100, 
                                         MilestoneIndex = 38274234, 
                                         References =
                                           new List<string>
                                             {
                                               "RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB"
                                             }
                                       }
                                 };
            return restResponse;
          });

      var repository = new RestIotaRepository(restClientMock.Object);
      var balances =
        repository.GetBalances(
          new List<Address> { new Address("RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB") }, 
          100);

      Assert.AreEqual(10000, balances.Addresses[0].Balance);
      Assert.AreEqual(100, balances.Duration);
      Assert.AreEqual(38274234, balances.MilestoneIndex);
      Assert.AreEqual("RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB", balances.References[0].Value);
    }

    /// <summary>
    /// The test get inputs has invalid bound should throw exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestGetInputsHasInvalidBoundShouldThrowException()
    {
      var restClientMock = new Mock<IRestClient>();
      var repository = new RestIotaRepository(restClientMock.Object);
      repository.GetInputs(Seed.Random(), 100, SecurityLevel.Medium, 10, 5);
    }

    /// <summary>
    /// The test get neighbors should include all parameters.
    /// </summary>
    [TestMethod]
    public void TestGetNeighborsShouldIncludeAllParameters()
    {
      var restClientMock = new Mock<IRestClient>();
      restClientMock.Setup(r => r.Execute<NeighborList>(It.IsAny<IRestRequest>())).Returns(
        () =>
          {
            var restResponse = new RestResponse<NeighborList>
                                 {
                                   StatusCode = HttpStatusCode.OK, 
                                   Data =
                                     new NeighborList
                                       {
                                         Duration = 1, 
                                         Neighbors =
                                           new List<Neighbor>
                                             {
                                               new Neighbor
                                                 {
                                                   Address = "/8.8.8.8:14265", 
                                                   ConnectionType = "Remote", 
                                                   NumberOfAllTransactions =
                                                     45, 
                                                   NumberOfInvalidTransactions
                                                     = 0, 
                                                   NumberOfNewTransactions = 5, 
                                                   NumberOfRandomTransactionRequests
                                                     = 3, 
                                                   NumberOfSentTransactions =
                                                     40
                                                 }
                                             }
                                       }
                                 };
            return restResponse;
          });

      var repository = new RestIotaRepository(restClientMock.Object);
      var neighbors = repository.GetNeighbors();

      Assert.AreEqual(1, neighbors.Duration);
      Assert.AreEqual("/8.8.8.8:14265", neighbors.Neighbors[0].Address);
      Assert.AreEqual("Remote", neighbors.Neighbors[0].ConnectionType);
      Assert.AreEqual(45, neighbors.Neighbors[0].NumberOfAllTransactions);
      Assert.AreEqual(0, neighbors.Neighbors[0].NumberOfInvalidTransactions);
      Assert.AreEqual(5, neighbors.Neighbors[0].NumberOfNewTransactions);
      Assert.AreEqual(3, neighbors.Neighbors[0].NumberOfRandomTransactionRequests);
      Assert.AreEqual(40, neighbors.Neighbors[0].NumberOfSentTransactions);
    }

    /// <summary>
    /// The test get node info should include all parameters.
    /// </summary>
    [TestMethod]
    public void TestGetNodeInfoShouldIncludeAllParameters()
    {
      var restClientMock = new Mock<IRestClient>();
      restClientMock.Setup(r => r.Execute<NodeInfo>(It.IsAny<IRestRequest>())).Returns(
        () =>
          {
            var restResponse = new RestResponse<NodeInfo>
                                 {
                                   StatusCode = HttpStatusCode.OK, 
                                   Data =
                                     new NodeInfo
                                       {
                                         AppName = "IRI", 
                                         AppVersion = "1.4.1.1", 
                                         Duration = 0, 
                                         JreAvailableProcessors = 4, 
                                         JreFreeMemory = 1433752216, 
                                         JreMaxMemory = 4294967296, 
                                         JreTotalMemory = 4294967296, 
                                         LatestMilestone =
                                           "MWBOADDZVATGJWCZSEQSPJSEEMGROZOGEERYBTIBZIKTJTXGNKMKWDFKIJOAKEPEJBGEMZLE9UVCA9999", 
                                         LatestMilestoneIndex = 325116, 
                                         LatestSolidSubtangleMilestone =
                                           "MWBOADDZVATGJWCZSEQSPJSEEMGROZOGEERYBTIBZIKTJTXGNKMKWDFKIJOAKEPEJBGEMZLE9UVCA9999", 
                                         LatestSolidSubtangleMilestoneIndex = 325116, 
                                         Neighbors = 6, 
                                         PacketsQueueSize = 0, 
                                         Time = 1515707837727, 
                                         Tips = 5946, 
                                         TransactionsToRequest = 550
                                       }
                                 };
            return restResponse;
          });

      var repository = new RestIotaRepository(restClientMock.Object);
      var nodeInfo = repository.GetNodeInfo();

      Assert.AreEqual("IRI", nodeInfo.AppName);
      Assert.AreEqual("1.4.1.1", nodeInfo.AppVersion);
      Assert.AreEqual(0, nodeInfo.Duration);
      Assert.AreEqual(4, nodeInfo.JreAvailableProcessors);
      Assert.AreEqual(1433752216, nodeInfo.JreFreeMemory);
      Assert.AreEqual(4294967296, nodeInfo.JreMaxMemory);
      Assert.AreEqual(4294967296, nodeInfo.JreTotalMemory);
      Assert.AreEqual("MWBOADDZVATGJWCZSEQSPJSEEMGROZOGEERYBTIBZIKTJTXGNKMKWDFKIJOAKEPEJBGEMZLE9UVCA9999", nodeInfo.LatestMilestone);
      Assert.AreEqual(325116, nodeInfo.LatestMilestoneIndex);
      Assert.AreEqual("MWBOADDZVATGJWCZSEQSPJSEEMGROZOGEERYBTIBZIKTJTXGNKMKWDFKIJOAKEPEJBGEMZLE9UVCA9999", nodeInfo.LatestSolidSubtangleMilestone);
      Assert.AreEqual(325116, nodeInfo.LatestSolidSubtangleMilestoneIndex);
      Assert.AreEqual(6, nodeInfo.Neighbors);
      Assert.AreEqual(0, nodeInfo.PacketsQueueSize);
      Assert.AreEqual(1515707837727, nodeInfo.Time);
      Assert.AreEqual(5946, nodeInfo.Tips);
      Assert.AreEqual(550, nodeInfo.TransactionsToRequest);
    }

    /// <summary>
    /// The test get transactions by addresses should include all parameters.
    /// </summary>
    [TestMethod]
    public void TestGetTransactionsByAddressesShouldIncludeAllParameters()
    {
      var restClientMock = new Mock<IRestClient>();
      restClientMock.Setup(r => r.Execute<GetTransactionsResponse>(It.IsAny<IRestRequest>())).Returns(
        () =>
          {
            var restResponse = new RestResponse<GetTransactionsResponse>
                                 {
                                   StatusCode = HttpStatusCode.OK, 
                                   Data =
                                     new GetTransactionsResponse
                                       {
                                         Hashes =
                                           new List<string>
                                             {
                                               "EJEAOOZYSAWFPZQESYDHZCGYNSTWXUMVJOVDWUNZJXDGWCLUFGIMZRMGCAZGKNPLBRLGUNYWKLJTYEAQX"
                                             }
                                       }
                                 };
            return restResponse;
          });

      var repository = new RestIotaRepository(restClientMock.Object);
      var transactions =
        repository.FindTransactionsByAddresses(
          new List<Address> { new Address("RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB") });

      Assert.AreEqual("EJEAOOZYSAWFPZQESYDHZCGYNSTWXUMVJOVDWUNZJXDGWCLUFGIMZRMGCAZGKNPLBRLGUNYWKLJTYEAQX", transactions.Hashes[0].Value);
    }

    /// <summary>
    /// The test get transactions to approve throws exception should wrap exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(IotaApiException))]
    public void TestGetTransactionsToApproveDoesNotReturnHttpStatusOkShouldWrapInException()
    {
      var restClientMock = new Mock<IRestClient>();
      restClientMock.Setup(r => r.Execute<GetTransactionsToApproveResponse>(It.IsAny<IRestRequest>()))
        .Returns(new RestResponse<GetTransactionsToApproveResponse> { StatusCode = HttpStatusCode.InternalServerError, Data = null });

      var repository = new RestIotaRepository(restClientMock.Object);
      repository.GetTransactionsToApprove();
    }

    /// <summary>
    /// The test get transactions to approve returns data should map to hashes.
    /// </summary>
    [TestMethod]
    public void TestGetTransactionsToApproveReturnsDataShouldMapToHashes()
    {
      var restClientMock = new Mock<IRestClient>();
      restClientMock.Setup(r => r.Execute<GetTransactionsToApproveResponse>(It.IsAny<IRestRequest>())).Returns(
        () =>
          {
            var restResponse = new RestResponse<GetTransactionsToApproveResponse>
                                 {
                                   StatusCode = HttpStatusCode.OK, 
                                   Data =
                                     new GetTransactionsToApproveResponse
                                       {
                                         BranchTransaction =
                                           "TKGDZ9GEI9CPNQGHEATIISAKYPPPSXVCXBSR9EIWCTHHSSEQCD9YLDPEXYERCNJVASRGWMAVKFQTC9999", 
                                         TrunkTransaction =
                                           "ASDFZ9GEI9CPNQGHEATIISAKYPPPSXVCXBSR9EIWCTHHSSEQCD9YLDPEXYERCNJVASRGWMAVKFQTC9999", 
                                         Duration = 1000
                                       }
                                 };
            return restResponse;
          });

      var repository = new RestIotaRepository(restClientMock.Object);
      var transactionsToApprove = repository.GetTransactionsToApprove();

      Assert.AreEqual(
        "TKGDZ9GEI9CPNQGHEATIISAKYPPPSXVCXBSR9EIWCTHHSSEQCD9YLDPEXYERCNJVASRGWMAVKFQTC9999", 
        transactionsToApprove.BranchTransaction.Value);
      Assert.AreEqual(
        "ASDFZ9GEI9CPNQGHEATIISAKYPPPSXVCXBSR9EIWCTHHSSEQCD9YLDPEXYERCNJVASRGWMAVKFQTC9999", 
        transactionsToApprove.TrunkTransaction.Value);
      Assert.AreEqual(1000, transactionsToApprove.Duration);
    }

    /// <summary>
    /// The test username and password is given should set authenticator.
    /// </summary>
    [TestMethod]
    public void TestUsernameAndPasswordIsGivenShouldSetAuthenticator()
    {
      var client = new RestClient();
      var repo = new RestIotaRepository(client, null, "test", "password");

      Assert.IsNotNull(client.Authenticator);
    }

    #endregion
  }
}