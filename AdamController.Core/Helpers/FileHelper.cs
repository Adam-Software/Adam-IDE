using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AdamController.Core.Helpers
{
    /*
    public class FileHelper
    {
        private const int cBufferSize = 0x4096;
        public static async Task WriteAsync(string path, string file)
        {
            using StreamWriter writer = File.CreateText(path);
            await writer.WriteAsync(file);
        }

        public static Task<string> ReadTextAsStringAsync(string path) => ReadTextToStringAsync(path);
        public static Task<byte[]> ReadTextAsByteArray(string path) => ReadTextToByteArrayAsync(path);

        private static async Task<string> ReadTextToStringAsync(string filePath)
        {
            using FileStream sourceStream = OpenRead(filePath);
            
            StringBuilder sb = new();

            byte[] buffer = new byte[0x1000];
            int numRead;

            while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                string text = Encoding.UTF8.GetString(buffer, 0, numRead);
                _ = sb.Append(text);
            }

            return sb.ToString();
        }

        private static async Task<byte[]> ReadTextToByteArrayAsync(string filePath)
        {
            using var fs = OpenRead(filePath);

            var buff = new byte[fs.Length];
            _ = await fs.ReadAsync(buff.AsMemory(0, (int) fs.Length));
            return buff;
        }

        /// <summary>
        /// Opens an existing file for asynchronous reading.
        /// </summary>
        /// <param name="path">Full file path</param>
        /// <returns>A read-only FileStream on the specified path.</returns>
        public static FileStream OpenRead(string path)
        {
            // Open a file stream for reading and that supports asynchronous I/O
            return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: cBufferSize, useAsync: true);
        }
    }
    */


}
