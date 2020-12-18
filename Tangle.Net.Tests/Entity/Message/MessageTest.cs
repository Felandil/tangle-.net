namespace Tangle.Net.Tests.Entity.Message
{
  using System;

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
  }
}