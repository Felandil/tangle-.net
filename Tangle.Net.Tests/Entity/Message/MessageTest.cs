namespace Tangle.Net.Tests.Entity.Message
{
  using System;
  using System.Collections.Generic;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Entity.Message;
  using Tangle.Net.Entity.Message.Payload;
  using Tangle.Net.Utils;

  [TestClass]
  public class MessageTest
  {
    [TestMethod]
    public void TestMessageSerializationWithIndexationPayload()
    {
      var message = new Message<IndexationPayload>
                      {
                        NetworkId = "6530425480034647824",
                        Nonce = "0",
                        Payload = new IndexationPayload { Index = "Tangle .Net", Data = "Hello world!".ToHex() },
                        Parent2MessageId = "80ccbb4d519b28e855e2682b393439fafab437d6f2f3c9747f3225caeef95bac",
                        Parent1MessageId = "7d52ed02a67ddfe3617345c8f2639363250f880d6b1380615bf5df411d4e59a4"
                      };

      var expected = new byte[]
                       {
                         16, 255, 159, 217, 95, 187, 160, 90, 125, 82, 237, 2, 166, 125, 223, 227, 97, 115, 69, 200, 242, 99, 147, 99, 37, 15, 136,
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
                             16, 255, 159, 217, 95, 187, 160, 90, 125, 82, 237, 2, 166, 125, 223, 227, 97, 115, 69, 200, 242, 99, 147, 99, 37, 15, 136,
                             13, 107, 19, 128, 97, 91, 245, 223, 65, 29, 78, 89, 164, 128, 204, 187, 77, 81, 155, 40, 232, 85, 226, 104, 43, 57, 52, 57,
                             250, 250, 180, 55, 214, 242, 243, 201, 116, 127, 50, 37, 202, 238, 249, 91, 172, 33, 0, 0, 0, 2, 0, 0, 0, 11, 0, 84, 97, 110,
                             103, 108, 101, 32, 46, 78, 101, 116, 12, 0, 0, 0, 72, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100, 33, 0, 0, 0, 0, 0, 0,
                             0, 0
                           };

      var message = Message<IndexationPayload>.Deserialize(messageBytes);

      Assert.AreEqual("6530425480034647824", message.NetworkId);
      Assert.AreEqual("0", message.Nonce);
      Assert.AreEqual("7d52ed02a67ddfe3617345c8f2639363250f880d6b1380615bf5df411d4e59a4", message.Parent1MessageId);
      Assert.AreEqual("80ccbb4d519b28e855e2682b393439fafab437d6f2f3c9747f3225caeef95bac", message.Parent2MessageId);
      Assert.AreEqual(2, message.Payload.Type);
      Assert.AreEqual("Tangle .Net", message.Payload.Index);
      Assert.AreEqual("Hello world!".ToHex(), message.Payload.Data);
    }

    [TestMethod]
    public void TestMessageSerializationWithMilestonePayload()
    {
      var message = new Message<MilestonePayload>
                      {
                        NetworkId = "6530425480034647824",
                        Nonce = "2229935",
                        Parent1MessageId = "6d945f7937e25a397cdf6519c99ed1609b913169341f67e2b5a2d2ccad592986",
                        Parent2MessageId = "75ba7a6fef36cf2aaef4abf9076eee745d940172166224139add684afeae591c",
                        Payload = new MilestonePayload
                                    {
                                      Index = 64849,
                                      Timestamp = 1608073977,
                                      Parent1MessageId = "6d945f7937e25a397cdf6519c99ed1609b913169341f67e2b5a2d2ccad592986",
                                      Parent2MessageId = "75ba7a6fef36cf2aaef4abf9076eee745d940172166224139add684afeae591c",
                                      InclusionMerkleProof = "0e5751c026e543b2e8ab2eb06099daa1d1e5df47778f7787faab45cdf12fe3a8",
                                      PublicKeys =
                                        new List<string>
                                          {
                                            "7205c145525cee64f1c9363696811d239919d830ad964b4e29359e6475848f5a",
                                            "e468e82df33d10dea3bd0eadcd7867946a674d207c39f5af4cc44365d268a7e6"
                                          },
                                      Signatures = new List<string>
                                                     {
                                                       "119695b7d7d0182052f05ab1f172b85db955e2c88b26b87f38c8906243de6d2ad69703f6298aa63bc409868e944c11f588a27986c35ee3462fa130ecfcd1b401",
                                                       "e0d0926198ec951d586dc5d544315b1cd19f26a095f0184301835b412dfc95248b356d77901ad2d7a399de43c6795ef503b32ed2673fe6fd4261fce26ff90302"
                                                     }
                                    },
                      };

      var expected = new byte[]
                 {
                         16, 255, 159, 217, 95, 187, 160, 90, 109, 148, 95, 121, 55, 226, 90, 57, 124, 223, 101, 25, 201, 158, 209, 96, 155, 145, 49,
                         105, 52, 31, 103, 226, 181, 162, 210, 204, 173, 89, 41, 134, 117, 186, 122, 111, 239, 54, 207, 42, 174, 244, 171, 249, 7,
                         110, 238, 116, 93, 148, 1, 114, 22, 98, 36, 19, 154, 221, 104, 74, 254, 174, 89, 28, 50, 1, 0, 0, 1, 0, 0, 0, 81, 253, 0, 0,
                         249, 66, 217, 95, 0, 0, 0, 0, 109, 148, 95, 121, 55, 226, 90, 57, 124, 223, 101, 25, 201, 158, 209, 96, 155, 145, 49, 105,
                         52, 31, 103, 226, 181, 162, 210, 204, 173, 89, 41, 134, 117, 186, 122, 111, 239, 54, 207, 42, 174, 244, 171, 249, 7, 110,
                         238, 116, 93, 148, 1, 114, 22, 98, 36, 19, 154, 221, 104, 74, 254, 174, 89, 28, 14, 87, 81, 192, 38, 229, 67, 178, 232, 171,
                         46, 176, 96, 153, 218, 161, 209, 229, 223, 71, 119, 143, 119, 135, 250, 171, 69, 205, 241, 47, 227, 168, 2, 114, 5, 193, 69,
                         82, 92, 238, 100, 241, 201, 54, 54, 150, 129, 29, 35, 153, 25, 216, 48, 173, 150, 75, 78, 41, 53, 158, 100, 117, 132, 143,
                         90, 228, 104, 232, 45, 243, 61, 16, 222, 163, 189, 14, 173, 205, 120, 103, 148, 106, 103, 77, 32, 124, 57, 245, 175, 76, 196,
                         67, 101, 210, 104, 167, 230, 2, 17, 150, 149, 183, 215, 208, 24, 32, 82, 240, 90, 177, 241, 114, 184, 93, 185, 85, 226, 200,
                         139, 38, 184, 127, 56, 200, 144, 98, 67, 222, 109, 42, 214, 151, 3, 246, 41, 138, 166, 59, 196, 9, 134, 142, 148, 76, 17,
                         245, 136, 162, 121, 134, 195, 94, 227, 70, 47, 161, 48, 236, 252, 209, 180, 1, 224, 208, 146, 97, 152, 236, 149, 29, 88, 109,
                         197, 213, 68, 49, 91, 28, 209, 159, 38, 160, 149, 240, 24, 67, 1, 131, 91, 65, 45, 252, 149, 36, 139, 53, 109, 119, 144, 26,
                         210, 215, 163, 153, 222, 67, 198, 121, 94, 245, 3, 179, 46, 210, 103, 63, 230, 253, 66, 97, 252, 226, 111, 249, 3, 2, 175, 6,
                         34, 0, 0, 0, 0, 0
                 };

      var actual = message.Serialize();

      CollectionAssert.AreEqual(expected, actual);
    }
  }
}