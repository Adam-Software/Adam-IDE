using System;
using System.Text;

namespace AdamControllerProjectsTest.CommonComponents
{
    public static class Utils
    {
        public static void PrintByteArray(byte[] bytes)
        {
            Console.WriteLine(bytes.ByteArrayToString());
        }

        public static void PrintTestResult(object expectedResult, object actualResult)
        {
            Console.WriteLine($"expected: { expectedResult }\nactual: { actualResult }");
        }

        public static void PrintTestResult(object expectedResult, byte[] actualResult)
        {
            Console.WriteLine($"expected: { expectedResult }\nactual: { actualResult.ByteArrayToString() }");
        }

        public static void PrintTestResult(byte[] expectedResult, object actualResult)
        {
            Console.WriteLine($"expected: { expectedResult.ByteArrayToString() }\n actual: { actualResult}");
        }

        public static void PrintTestResult(byte[] expectedResult, byte[] actualResult)
        {
            Console.WriteLine($"expected: { expectedResult.ByteArrayToString() }\nactual: { actualResult.ByteArrayToString() }");
        }

        #region Extension

        public static string ByteArrayToString(this byte[] bytes)
        {
            StringBuilder sb = new("new byte[] { ");

            foreach (byte b in bytes)
            {
                _ = sb.Append(b + ", ");
            }
            _ = sb.Append("}");
            
            return sb.ToString();
        }

        #endregion
    }
}
