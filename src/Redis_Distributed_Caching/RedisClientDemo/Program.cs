using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace RedisClientDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            const string host = "127.0.0.1";
            const int port = 6379;
            socket.Connect(host, port);

            var responseStream = new BufferedStream(new NetworkStream(socket), 1024);

            //number of commands (*2), new line(\r\n),
            //length of first parameter ($3), new line (\r\n), first parameter (GET), new line (\r\n)
            //length of second parameter ($3), new line (\r\n), second parameter (dog), new line (\r\n)
            const string requestString = "*2\r\n$3\r\nGET\r\n$3\r\ndog\r\n";
            byte[] request = Encoding.UTF8.GetBytes(requestString);
            socket.Send(request);

            var result = new StringBuilder();
            int @byte;

            while ((@byte = responseStream.ReadByte()) != -1)
            {
                if (@byte == '\r') //approaching the end of line
                {
                    responseStream.ReadByte();
                    break;
                }

                result.Append((char)@byte);
            }

            //instead of range operator you could use: `.Substring(1))`
            var responseLength = int.Parse(result.ToString()[1..]);
            var responseValue = new byte[responseLength];
            responseStream.Read(responseValue, 0, responseLength);
            Console.WriteLine($"result {Encoding.UTF8.GetString(responseValue)}");

            Console.ReadKey();
        }
    }
}