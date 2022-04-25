using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Server.Utilities
{
    public class TcpConnector
    {
        public TcpConnector(){}
        public List<string> tryRead(String server, String message)
        {
            try
            {
                // Create a TcpClient.
                Int32 port = 5005;
                TcpClient client = new TcpClient(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                NetworkStream stream = client.GetStream();

                stream.Write(data,0,data.Length);
                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                List<string> messages = new List<string>();


                while(client.Connected)
                {
                    // Reads data from the stream and stores inside data, (from index, length of data), returns number of bytes read
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    if (bytes <= 0) {
                        break;
                    }
                    // Convert to bytes array to string
                    Console.WriteLine(System.Text.Encoding.ASCII.GetString(data, 0, bytes));
                }

                // Close everything.
                stream.Close();
                client.Close();
                return messages;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
                return null;
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                return null;
            }
        }
    }
}