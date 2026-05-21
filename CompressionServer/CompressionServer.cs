using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CompressionServer
{
    class Program
    {
        private static int PORT = 9000;
        private static int clientCount = 0;

        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Any, PORT);
            server.Start();
            Console.WriteLine("Server Run......");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                int id = Interlocked.Increment(ref clientCount);
                Console.WriteLine($"{id} connected from {((IPEndPoint)client.Client.RemoteEndPoint).Address}");

                Thread thread = new Thread(() => HandleClient(client, id));
                thread.IsBackground = true;
                thread.Start();
            }
        }

        static void HandleClient(TcpClient client, int clientId)
        {
            try
            {
                using (NetworkStream netStream = client.GetStream())
                {
                    byte[] sizeBuffer = new byte[8];
                    ReadExact(netStream, sizeBuffer, 0, 8);
                    long fileSize = BitConverter.ToInt64(sizeBuffer, 0);
                    Console.WriteLine($"{clientId} Receiving file: {fileSize:N0} bytes");

                    byte[] fileData = new byte[fileSize];
                    ReadExact(netStream, fileData, 0, (int)fileSize);
                    Console.WriteLine($"{clientId} File received.\n Compressing...");

                    byte[] compressed = CompressGzip(fileData);
                    Console.WriteLine($"{clientId} Compressed: {compressed.Length:N0} bytes  " +
                                      $"(ratio: {100.0 * compressed.Length / fileSize:F1}%)");

                    byte[] compressedSizeBytes = BitConverter.GetBytes((long)compressed.Length);
                    netStream.Write(compressedSizeBytes, 0, 8);
                    netStream.Write(compressed, 0, compressed.Length);
                    netStream.Flush();

                    Console.WriteLine($"{clientId} Done.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{clientId} Error: {ex.Message}");
            }
            finally
            {
                client.Close();
                Console.WriteLine($"{clientId} disconnected.");
            }
        }

        static byte[] CompressGzip(byte[] data)
        {
            using (var output = new MemoryStream())
            {
                using (var gzip = new GZipStream(output, CompressionLevel.Optimal))
                {
                    gzip.Write(data, 0, data.Length);
                }
                return output.ToArray();
            }
        }

        static void ReadExact(Stream stream, byte[] buffer, int offset, int count)
        {
            int received = 0;
            while (received < count)
            {
                int n = stream.Read(buffer, offset + received, count - received);
                if (n == 0) throw new EndOfStreamException("Connection closed unexpectedly.");
                received += n;
            }
        }
    }
}
