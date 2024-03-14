using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdamController.Core.Collections
{
    public static class CollectionUtils
    {
        /// <summary>
        /// Складывает два массива
        /// </summary>
        public static byte[] Combine(this byte[] firstByteArray, byte[] secondByteArray)
        {
            byte[] ret = new byte[firstByteArray.Length + secondByteArray.Length];
            Buffer.BlockCopy(firstByteArray, 0, ret, 0, firstByteArray.Length);
            Buffer.BlockCopy(secondByteArray, 0, ret, firstByteArray.Length, secondByteArray.Length);
            return ret;
        }

        /// <summary>
        /// Добаваляет @string в начало массива bytes
        /// </summary>
        public static byte[] AddStringAsByteArray(this byte[] bytes, string @string)
        {
            if (string.IsNullOrEmpty(@string))
                return bytes;

            byte[] servoStringBytes = Encoding.ASCII.GetBytes(@string);
            byte[] combineBytes = servoStringBytes.Combine(bytes);

            return combineBytes;
        }

        /// <summary>
        /// Convert to byte[]
        /// </summary>
        public static byte[] ConvertToByteArray(this List<byte[]> arrays)
        {
            byte[] ret = new byte[arrays.Sum(x => x.Length)];
            int offset = 0;

            foreach (byte[] data in arrays)
            {
                Buffer.BlockCopy(data, 0, ret, offset, data.Length);
                offset += data.Length;
            }

            return ret;
        }

    }
}
