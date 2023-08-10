using AdamControllerLibrary.Collections;
using AdamControllerProjectsTest.CommonComponents;
using NUnit.Framework;
using System.Collections.Generic;

namespace AdamControllerProjectsTest.AdamControllerLibraryTests.CollectionTest
{
    [TestFixture]
    public class CollectionUtilsTest
    {
        [Test]
        public void CombineByteArrayAndByteArray()
        {
            byte[] byteArrayTwo = new byte[] 
            { 
                5, 6, 7, 8 
            };

            byte[] byteArrayOne = new byte[]
            {
                 0, 1, 2, 3, 4 
            };

            Assert.AreEqual(new byte[] { 5, 6, 7, 8, 0, 1, 2, 3, 4, }, byteArrayTwo.Combine(byteArrayOne));
            Utils.PrintTestResult(new byte[] { 5, 6, 7, 8, 0, 1, 2, 3, 4, }, byteArrayTwo.Combine(byteArrayOne));

            Assert.AreEqual(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, }, byteArrayOne.Combine(byteArrayTwo));
            Utils.PrintTestResult(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, }, byteArrayOne.Combine(byteArrayTwo));
        }

        [TestCase(new byte[] { 0, 0 }, "servo", new byte[] { 115, 101, 114, 118, 111, 0, 0, })]
        [TestCase(new byte[] { 0, 0 }, "eyes", new byte[] { 101, 121, 101, 115, 0, 0, })]
        [TestCase(new byte[] { }, "", new byte[] { })]
        [TestCase(new byte[] { 0, 0 }, null, new byte[] { 0, 0, })]
        [TestCase(new byte[] { }, null, new byte[] { })]
        public void AddStringAsByteArray(byte[] bytes, string @string, byte[] expectedBytes)
        {
            Assert.AreEqual(expectedBytes, bytes.AddStringAsByteArray(@string));
            Utils.PrintByteArray(bytes.AddStringAsByteArray(@string));
        }

        [Test]
        public void ConvertListToByteArray()
        {
            List<byte[]> lists = new();
            lists.Add(new byte[] { 1, 2, 3 });
            lists.Add(new byte[] { 4, 5, 6 });
            lists.Add(new byte[] { 7, 8, 9 });

            Assert.AreEqual(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, }, lists.ConvertToByteArray());
            Utils.PrintTestResult(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, }, lists.ConvertToByteArray());
        }
    }
}
