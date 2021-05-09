using Newtonsoft.Json;
using Tangle.Net.Entity.Ed25519;

namespace Tangle.Net.Tests.Entity.Message
{
  using System;
  using System.Collections.Generic;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Entity.Message;
  using Tangle.Net.Entity.Message.Payload;
  using Tangle.Net.Entity.Message.Payload.Transaction;
  using Tangle.Net.Utils;

  [TestClass]
  public class MessageTest
  {
    [TestMethod]
    public void TestMessageSerializationWithIndexationPayload()
    {
      var parentMessageIds = new List<string>
      {
        "7d52ed02a67ddfe3617345c8f2639363250f880d6b1380615bf5df411d4e59a4",
        "80ccbb4d519b28e855e2682b393439fafab437d6f2f3c9747f3225caeef95bac"
      };

      var message = new Message<IndexationPayload>
      {
        NetworkId = "6530425480034647824",
        Nonce = "0",
        Payload = new IndexationPayload { Index = "Tangle .Net".ToHex(), Data = "Hello world!".ToHex() },
        ParentMessageIds = parentMessageIds
      };

      var expected = new byte[]
                       {
                         16, 255, 159, 217, 95, 187, 160, 90, 2, 125, 82, 237, 2, 166, 125, 223, 227, 97, 115, 69, 200, 242, 99, 147, 99, 37, 15, 136,
                         13, 107, 19, 128, 97, 91, 245, 223, 65, 29, 78, 89, 164, 128, 204, 187, 77, 81, 155, 40, 232, 85, 226, 104, 43, 57, 52, 57,
                         250, 250, 180, 55, 214, 242, 243, 201, 116, 127, 50, 37, 202, 238, 249, 91, 172, 33, 0, 0, 0, 2, 0, 0, 0, 11, 0, 84, 97, 110,
                         103, 108, 101, 32, 46, 78, 101, 116, 12, 0, 0, 0, 72, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100, 33, 0, 0, 0, 0, 0, 0,
                         0, 0
                       };

      var actual = message.Serialize();

      CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestMessageDeserializationWithIndexationPayload()
    {
      var messageBytes = new byte[]
                           {
                             16, 255, 159, 217, 95, 187, 160, 90, 2, 125, 82, 237, 2, 166, 125, 223, 227, 97, 115, 69, 200, 242, 99, 147, 99, 37, 15, 136,
                             13, 107, 19, 128, 97, 91, 245, 223, 65, 29, 78, 89, 164, 128, 204, 187, 77, 81, 155, 40, 232, 85, 226, 104, 43, 57, 52, 57,
                             250, 250, 180, 55, 214, 242, 243, 201, 116, 127, 50, 37, 202, 238, 249, 91, 172, 33, 0, 0, 0, 2, 0, 0, 0, 11, 0, 84, 97, 110,
                             103, 108, 101, 32, 46, 78, 101, 116, 12, 0, 0, 0, 72, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100, 33, 0, 0, 0, 0, 0, 0,
                             0, 0
                           };

      var message = Message<IndexationPayload>.Deserialize(messageBytes);

      Assert.AreEqual("6530425480034647824", message.NetworkId);
      Assert.AreEqual("0", message.Nonce);
      Assert.AreEqual("7d52ed02a67ddfe3617345c8f2639363250f880d6b1380615bf5df411d4e59a4", message.ParentMessageIds[0]);
      Assert.AreEqual("80ccbb4d519b28e855e2682b393439fafab437d6f2f3c9747f3225caeef95bac", message.ParentMessageIds[1]);
      Assert.AreEqual(2, message.Payload.Type);
      Assert.AreEqual("Tangle .Net".ToHex(), message.Payload.Index);
      Assert.AreEqual("Hello world!".ToHex(), message.Payload.Data);
    }

    [TestMethod]
    public void TestMessageSerializationWithMilestonePayload()
    {
      var message = new Message<MilestonePayload>
      {
        NetworkId = "1454675179895816119",
        Nonce = "1581149492032252383",
        ParentMessageIds = new List<string>
        {
          "5bff3769dbff937d8872f0b7a11002cfd9805b1561b6154f119761edae08af0f",
          "77a558da1b41a4841621d7e5965d5511cda3d8b81646e2c1708059bf19170edc",
          "bdde8d1ec1cbdc9aa6e8568a61fc27dfbf3a5474c02c0b1f372aed8b2b2c8bd1",
          "d7e755158db3b28f933830ba1b780d4c2a40438b091f1bd0d0cc6cc69b5e0b66",
          "e439d513e8f21b1e1130847f380deeb509ebf5e856d696cd029ca13edd8d50fe",
          "e6b1f204be4c0029739ffb37a91f28111033005f607075131a4ba053c05bf8f3"
        },
        Payload = new MilestonePayload
        {
          Type = 1,
          Index = 89162,
          Timestamp = 1620511989,
          ParentMessageIds = new List<string>
          {
            "5bff3769dbff937d8872f0b7a11002cfd9805b1561b6154f119761edae08af0f",
            "77a558da1b41a4841621d7e5965d5511cda3d8b81646e2c1708059bf19170edc",
            "bdde8d1ec1cbdc9aa6e8568a61fc27dfbf3a5474c02c0b1f372aed8b2b2c8bd1",
            "d7e755158db3b28f933830ba1b780d4c2a40438b091f1bd0d0cc6cc69b5e0b66",
            "e439d513e8f21b1e1130847f380deeb509ebf5e856d696cd029ca13edd8d50fe",
            "e6b1f204be4c0029739ffb37a91f28111033005f607075131a4ba053c05bf8f3"
          },
          InclusionMerkleProof = "0e5751c026e543b2e8ab2eb06099daa1d1e5df47778f7787faab45cdf12fe3a8",
          NextPoWScore = 0,
          NextPoWScoreMilestoneIndex = 0,
          PublicKeys =
            new List<string>
            {
              "365fb85e7568b9b32f7359d6cbafa9814472ad0ecbad32d77beaf5dd9e84c6ba",
              "a9b46fe743df783dedd00c954612428b34241f5913cf249d75bed3aafd65e4cd"
            },
          Signatures = new List<string>
          {
            "59ae85d697902115fc83a12abab66e88d3211e78e3f956eac5642724570ac8787d997a7db22bf730e4d4303403c5e63f94987f0ba74480883ce6551e4696b20b",
            "3bfbce92cd6e99f4e7eb61c4190e80e351b0be6835a28f48f222e9ef521c0b56fde971a2a46714ab5df3214daa2ba68bfa32c030357cffccf111e026ac93a50e"
          }
        },
      };

      var messageAsHex =
        "b77f44715e0b3014065bff3769dbff937d8872f0b7a11002cfd9805b1561b6154f119761edae08af0f77a558da1b41a4841621d7e5965d5511cda3d8b81646e2c1708059bf19170edcbdde8d1ec1cbdc9aa6e8568a61fc27dfbf3a5474c02c0b1f372aed8b2b2c8bd1d7e755158db3b28f933830ba1b780d4c2a40438b091f1bd0d0cc6cc69b5e0b66e439d513e8f21b1e1130847f380deeb509ebf5e856d696cd029ca13edd8d50fee6b1f204be4c0029739ffb37a91f28111033005f607075131a4ba053c05bf8f3bf010000010000004a5c0100f50c976000000000065bff3769dbff937d8872f0b7a11002cfd9805b1561b6154f119761edae08af0f77a558da1b41a4841621d7e5965d5511cda3d8b81646e2c1708059bf19170edcbdde8d1ec1cbdc9aa6e8568a61fc27dfbf3a5474c02c0b1f372aed8b2b2c8bd1d7e755158db3b28f933830ba1b780d4c2a40438b091f1bd0d0cc6cc69b5e0b66e439d513e8f21b1e1130847f380deeb509ebf5e856d696cd029ca13edd8d50fee6b1f204be4c0029739ffb37a91f28111033005f607075131a4ba053c05bf8f30e5751c026e543b2e8ab2eb06099daa1d1e5df47778f7787faab45cdf12fe3a8000000000000000002365fb85e7568b9b32f7359d6cbafa9814472ad0ecbad32d77beaf5dd9e84c6baa9b46fe743df783dedd00c954612428b34241f5913cf249d75bed3aafd65e4cd000000000259ae85d697902115fc83a12abab66e88d3211e78e3f956eac5642724570ac8787d997a7db22bf730e4d4303403c5e63f94987f0ba74480883ce6551e4696b20b3bfbce92cd6e99f4e7eb61c4190e80e351b0be6835a28f48f222e9ef521c0b56fde971a2a46714ab5df3214daa2ba68bfa32c030357cffccf111e026ac93a50edf295ff1155ff115";
      var expected = messageAsHex.HexToBytes();

      var actual = message.Serialize();

      CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestMessageDeserializationWithMilestonePayload()
    {
      var messageAsHex =
        "b77f44715e0b3014060481396ba54c93fa9a84aa87c787d7c7a9665d98a2ab945756966801229b889129da994d7f4a66b50af58b1271f50c91dc217762ac1d6857afb321c83765e08eaad1f763e2634eb681661f180f2c3cc3948ee4426c67332e111dee028deadde7b24d2ae393c60b69f0cb13065bb67eaec88a64f3f6e4ff29085ef47b83dae29ed530f398f61a527b3494fc5bfb0325a52003fcee089a86b64b4bcf6c3283402ad7fbc7a180d5f72318f0566229362c67b7497d6a79ebd944d6d7ad40ff46ff62bf01000001000000972b0100f725956000000000060481396ba54c93fa9a84aa87c787d7c7a9665d98a2ab945756966801229b889129da994d7f4a66b50af58b1271f50c91dc217762ac1d6857afb321c83765e08eaad1f763e2634eb681661f180f2c3cc3948ee4426c67332e111dee028deadde7b24d2ae393c60b69f0cb13065bb67eaec88a64f3f6e4ff29085ef47b83dae29ed530f398f61a527b3494fc5bfb0325a52003fcee089a86b64b4bcf6c3283402ad7fbc7a180d5f72318f0566229362c67b7497d6a79ebd944d6d7ad40ff46ff620e5751c026e543b2e8ab2eb06099daa1d1e5df47778f7787faab45cdf12fe3a8000000000000000002365fb85e7568b9b32f7359d6cbafa9814472ad0ecbad32d77beaf5dd9e84c6baa9b46fe743df783dedd00c954612428b34241f5913cf249d75bed3aafd65e4cd0000000002e5046adc0a5a7eea3efc7de097f230835541162eb1b94a57dbe558f4cdf0701748aa2331b903fadb538f3993da385f9cb5d69bbdc8fc747eda4d6cf5ca3c320e64ce45d94f01195844fd5278526ee4111477698b2eb3c9a56a691b1c4fcdea174acad1c9ee11fd914be45e907bf18ef3b570f7ee2d5a8844fcbeeac132c524050d9e3ba8833aa883";
      var messageBytes = messageAsHex.HexToBytes();

      var actualMessage = Message<MilestonePayload>.Deserialize(messageBytes);
      var expectedMessage = new Message<MilestonePayload>
      {
        NetworkId = "1454675179895816119",
        Nonce = "9486896952193555981",
        ParentMessageIds = new List<string>
        {
          "0481396ba54c93fa9a84aa87c787d7c7a9665d98a2ab945756966801229b8891",
          "29da994d7f4a66b50af58b1271f50c91dc217762ac1d6857afb321c83765e08e",
          "aad1f763e2634eb681661f180f2c3cc3948ee4426c67332e111dee028deadde7",
          "b24d2ae393c60b69f0cb13065bb67eaec88a64f3f6e4ff29085ef47b83dae29e",
          "d530f398f61a527b3494fc5bfb0325a52003fcee089a86b64b4bcf6c3283402a",
          "d7fbc7a180d5f72318f0566229362c67b7497d6a79ebd944d6d7ad40ff46ff62"
        },
        Payload = new MilestonePayload
        {
          Type = 1,
          Index = 76695,
          Timestamp = 1620387319,
          ParentMessageIds = new List<string>
          {
            "0481396ba54c93fa9a84aa87c787d7c7a9665d98a2ab945756966801229b8891",
            "29da994d7f4a66b50af58b1271f50c91dc217762ac1d6857afb321c83765e08e",
            "aad1f763e2634eb681661f180f2c3cc3948ee4426c67332e111dee028deadde7",
            "b24d2ae393c60b69f0cb13065bb67eaec88a64f3f6e4ff29085ef47b83dae29e",
            "d530f398f61a527b3494fc5bfb0325a52003fcee089a86b64b4bcf6c3283402a",
            "d7fbc7a180d5f72318f0566229362c67b7497d6a79ebd944d6d7ad40ff46ff62"
          },
          InclusionMerkleProof = "0e5751c026e543b2e8ab2eb06099daa1d1e5df47778f7787faab45cdf12fe3a8",
          NextPoWScore = 0,
          NextPoWScoreMilestoneIndex = 0,
          PublicKeys =
            new List<string>
            {
              "365fb85e7568b9b32f7359d6cbafa9814472ad0ecbad32d77beaf5dd9e84c6ba",
              "a9b46fe743df783dedd00c954612428b34241f5913cf249d75bed3aafd65e4cd"
            },
          Signatures = new List<string>
          {
            "e5046adc0a5a7eea3efc7de097f230835541162eb1b94a57dbe558f4cdf0701748aa2331b903fadb538f3993da385f9cb5d69bbdc8fc747eda4d6cf5ca3c320e",
            "64ce45d94f01195844fd5278526ee4111477698b2eb3c9a56a691b1c4fcdea174acad1c9ee11fd914be45e907bf18ef3b570f7ee2d5a8844fcbeeac132c52405"
          }
        },
      };

      Assert.AreEqual(expectedMessage.NetworkId, actualMessage.NetworkId);
      Assert.AreEqual(expectedMessage.ParentMessageIds[0], actualMessage.ParentMessageIds[0]);
      Assert.AreEqual(expectedMessage.ParentMessageIds[1], actualMessage.ParentMessageIds[1]);

      Assert.AreEqual(expectedMessage.Payload.Index, actualMessage.Payload.Index);
      Assert.AreEqual(expectedMessage.Payload.Timestamp, actualMessage.Payload.Timestamp);
      Assert.AreEqual(expectedMessage.Payload.ParentMessageIds[0], actualMessage.Payload.ParentMessageIds[0]);
      Assert.AreEqual(expectedMessage.Payload.ParentMessageIds[1], actualMessage.Payload.ParentMessageIds[1]);
      Assert.AreEqual(expectedMessage.Payload.InclusionMerkleProof, actualMessage.Payload.InclusionMerkleProof);

      Assert.AreEqual(expectedMessage.Payload.PublicKeys[0], actualMessage.Payload.PublicKeys[0]);
      Assert.AreEqual(expectedMessage.Payload.PublicKeys[1], actualMessage.Payload.PublicKeys[1]);

      Assert.AreEqual(expectedMessage.Payload.Signatures[0], actualMessage.Payload.Signatures[0]);
      Assert.AreEqual(expectedMessage.Payload.Signatures[1], actualMessage.Payload.Signatures[1]);
    }

    [TestMethod]
    public void TestMessageSerializationWithTransactionPayload()
    {
      var parentMessageIds = new List<string>
      {
        "67b959e3b2ed8a8adfdeb6e5a1efe573e7caee5cac91e64220407d2043147219",
        "beba1162ef983a6570b1119d635661c121783a9b9d973a17b46bd26ae997a6a4",
      };

      var message = new Message<TransactionPayload<SignatureUnlockBlock>>
      {
        NetworkId = "6530425480034647824",
        Nonce = "724240",
        ParentMessageIds = parentMessageIds,
        Payload = new TransactionPayload<SignatureUnlockBlock>
        {
          Essence = new TransactionEssence
          {
            Inputs = new List<UTXOInput>
                                                               {
                                                                 new UTXOInput
                                                                   {
                                                                     TransactionId =
                                                                       "c08141e6f09d8da0c7d5604e3e3bab20731948c120af42d58826817eaec9d186",
                                                                     TransactionOutputIndex = 0
                                                                   }
                                                               },
            Outputs = new List<SigLockedSingleOutput>
                                                                {
                                                                  new SigLockedSingleOutput
                                                                    {
                                                                      Amount = 1000,
                                                                      Address = new Ed25519Address
                                                                                  {
                                                                                    Address =
                                                                                      "3f8530482fd6d0e3a198c327f5ce0423f2eb9e22be8c3ab0554d081467b20a5b"
                                                                                  }
                                                                    }
                                                                },
            Payload = new IndexationPayload
            {
              Index = "test\n".ToHex(),
              Data = "746573740a"
            }
          },
          UnlockBlocks = new List<SignatureUnlockBlock>
                                                       {
                                                         new SignatureUnlockBlock
                                                           {
                                                             Signature = new Ed25519Signature
                                                                           {
                                                                             PublicKey = "e6a908d6ccbfd4b797a2858c0b4067e4800a987d3f5d54096c6f384628e5a7eb",
                                                                             Signature = "45f7e6fff1c35f47dbcb739919507c26cbf5c5bbe1ec0d2a2dd3bd097b320214c3fcf9a426b1efce4438e7efdc387c4fefda7fff9526b76b813942f96f756306"
                                                                           }
                                                           }
                                                       }
        }
      };

      var expected = new byte[]
                      {
                        16, 255, 159, 217, 95, 187, 160, 90, 2, 103, 185, 89, 227, 178, 237, 138, 138, 223, 222, 182, 229, 161, 239, 229, 115, 231, 202,
                        238, 92, 172, 145, 230, 66, 32, 64, 125, 32, 67, 20, 114, 25, 190, 186, 17, 98, 239, 152, 58, 101, 112, 177, 17, 157, 99, 86,
                        97, 193, 33, 120, 58, 155, 157, 151, 58, 23, 180, 107, 210, 106, 233, 151, 166, 164, 210, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
                        192, 129, 65, 230, 240, 157, 141, 160, 199, 213, 96, 78, 62, 59, 171, 32, 115, 25, 72, 193, 32, 175, 66, 213, 136, 38, 129,
                        126, 174, 201, 209, 134, 0, 0, 1, 0, 0, 0, 63, 133, 48, 72, 47, 214, 208, 227, 161, 152, 195, 39, 245, 206, 4, 35, 242, 235,
                        158, 34, 190, 140, 58, 176, 85, 77, 8, 20, 103, 178, 10, 91, 232, 3, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 2, 0, 0, 0, 5, 0, 116,
                        101, 115, 116, 10, 5, 0, 0, 0, 116, 101, 115, 116, 10, 1, 0, 0, 1, 230, 169, 8, 214, 204, 191, 212, 183, 151, 162, 133, 140,
                        11, 64, 103, 228, 128, 10, 152, 125, 63, 93, 84, 9, 108, 111, 56, 70, 40, 229, 167, 235, 69, 247, 230, 255, 241, 195, 95, 71,
                        219, 203, 115, 153, 25, 80, 124, 38, 203, 245, 197, 187, 225, 236, 13, 42, 45, 211, 189, 9, 123, 50, 2, 20, 195, 252, 249,
                        164, 38, 177, 239, 206, 68, 56, 231, 239, 220, 56, 124, 79, 239, 218, 127, 255, 149, 38, 183, 107, 129, 57, 66, 249, 111, 117,
                        99, 6, 16, 13, 11, 0, 0, 0, 0, 0
                      };

      var serialized = message.Serialize();

      CollectionAssert.AreEqual(expected, serialized);
    }

    [TestMethod]
    public void TestMessageDeserializationWithTransactionPayload()
    {
      var messageBytes = new byte[]
      {
        16, 255, 159, 217, 95, 187, 160, 90, 3, 103, 185, 89, 227, 178, 237, 138, 138, 223, 222, 182, 229, 161, 239, 229, 115, 231, 202,
        238, 92, 172, 145, 230, 66, 32, 64, 125, 32, 67, 20, 114, 25, 190, 186, 17, 98, 239, 152, 58, 101, 112, 177, 17, 157, 99, 86,
        97, 193, 33, 120, 58, 155, 157, 151, 58, 23, 180, 107, 210, 106, 233, 151, 166, 164, 210, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
        192, 129, 65, 230, 240, 157, 141, 160, 199, 213, 96, 78, 62, 59, 171, 32, 115, 25, 72, 193, 32, 175, 66, 213, 136, 38, 129,
        126, 174, 201, 209, 134, 0, 0, 1, 0, 0, 1, 63, 133, 48, 72, 47, 214, 208, 227, 161, 152, 195, 39, 245, 206, 4, 35, 242, 235,
        158, 34, 190, 140, 58, 176, 85, 77, 8, 20, 103, 178, 10, 91, 232, 3, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 2, 0, 0, 0, 5, 0, 116,
        101, 115, 116, 10, 5, 0, 0, 0, 116, 101, 115, 116, 10, 1, 0, 0, 1, 230, 169, 8, 214, 204, 191, 212, 183, 151, 162, 133, 140,
        11, 64, 103, 228, 128, 10, 152, 125, 63, 93, 84, 9, 108, 111, 56, 70, 40, 229, 167, 235, 69, 247, 230, 255, 241, 195, 95, 71,
        219, 203, 115, 153, 25, 80, 124, 38, 203, 245, 197, 187, 225, 236, 13, 42, 45, 211, 189, 9, 123, 50, 2, 20, 195, 252, 249,
        164, 38, 177, 239, 206, 68, 56, 231, 239, 220, 56, 124, 79, 239, 218, 127, 255, 149, 38, 183, 107, 129, 57, 66, 249, 111, 117,
        99, 6, 16, 13, 11, 0, 0, 0, 0, 0
      };

     // var message = Message<TransactionPayload<SignatureUnlockBlock>>.Deserialize(messageBytes);
    }

    private static Message<MilestonePayload> GetMilestone()
    {
      return JsonConvert.DeserializeObject<Message<MilestonePayload>>(
        "{\"networkId\":\"1454675179895816119\",\"parentMessageIds\":[\"0481396ba54c93fa9a84aa87c787d7c7a9665d98a2ab945756966801229b8891\",\"29da994d7f4a66b50af58b1271f50c91dc217762ac1d6857afb321c83765e08e\",\"aad1f763e2634eb681661f180f2c3cc3948ee4426c67332e111dee028deadde7\",\"b24d2ae393c60b69f0cb13065bb67eaec88a64f3f6e4ff29085ef47b83dae29e\",\"d530f398f61a527b3494fc5bfb0325a52003fcee089a86b64b4bcf6c3283402a\",\"d7fbc7a180d5f72318f0566229362c67b7497d6a79ebd944d6d7ad40ff46ff62\"],\"payload\":{\"type\":1,\"index\":76695,\"timestamp\":1620387319,\"parentMessageIds\":[\"0481396ba54c93fa9a84aa87c787d7c7a9665d98a2ab945756966801229b8891\",\"29da994d7f4a66b50af58b1271f50c91dc217762ac1d6857afb321c83765e08e\",\"aad1f763e2634eb681661f180f2c3cc3948ee4426c67332e111dee028deadde7\",\"b24d2ae393c60b69f0cb13065bb67eaec88a64f3f6e4ff29085ef47b83dae29e\",\"d530f398f61a527b3494fc5bfb0325a52003fcee089a86b64b4bcf6c3283402a\",\"d7fbc7a180d5f72318f0566229362c67b7497d6a79ebd944d6d7ad40ff46ff62\"],\"inclusionMerkleProof\":\"0e5751c026e543b2e8ab2eb06099daa1d1e5df47778f7787faab45cdf12fe3a8\",\"nextPoWScore\":0,\"nextPoWScoreMilestoneIndex\":0,\"publicKeys\":[\"365fb85e7568b9b32f7359d6cbafa9814472ad0ecbad32d77beaf5dd9e84c6ba\",\"a9b46fe743df783dedd00c954612428b34241f5913cf249d75bed3aafd65e4cd\"],\"receipt\":null,\"signatures\":[\"e5046adc0a5a7eea3efc7de097f230835541162eb1b94a57dbe558f4cdf0701748aa2331b903fadb538f3993da385f9cb5d69bbdc8fc747eda4d6cf5ca3c320e\",\"64ce45d94f01195844fd5278526ee4111477698b2eb3c9a56a691b1c4fcdea174acad1c9ee11fd914be45e907bf18ef3b570f7ee2d5a8844fcbeeac132c52405\"]},\"nonce\":\"9486896952193555981\"}");
    }
  }
}