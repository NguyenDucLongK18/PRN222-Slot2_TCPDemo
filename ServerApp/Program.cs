using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;

class Program {
    static void ProcessMessage(object param)
    {
        string data;
        int count;
        try
        {
            TcpClient client = param as TcpClient;

            Byte[] bytes = new Byte[256];

            NetworkStream stream = client.GetStream();

            while ((count = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, count);
                Console.WriteLine($"Received: {data} at {DateTime.Now:t}");
                data = $"{data.ToUpper()}";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                stream.Write(msg, 0, msg.Length);
                Console.WriteLine($"Sent: {data}");
            }

            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("{0}", ex.Message);
            Console.WriteLine("Wating message");
        }
    }

    static void ExecuteServer(string host, int port) {
        int Count = 0;
        TcpListener server = null;
        try
        {
            Console.Title = "Server Application";
            IPAddress localAddr = IPAddress.Parse(host);
            server = new TcpListener(localAddr, port);

            server.Start();
            Console.W
            Console.WriteLine(new string('*', 40));
            Console.WriteLine("Waiting for a connection... ");

            while (true) {
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine($"Number of client connected: {++Count}");
                Console.WriteLine(new string('*', 40));

                Thread thread = new Thread(new ParameterizedThreadStart(ProcessMessage));
                thread.Start(client);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: {0}", ex.Message);
        }
        finally {
            server.Stop();
            Console.WriteLine("Server stopped. Press any key to exit...");
        }
        Console.Read();
    }
    static void Main(String[] args) {
        string host = "127.0.0.1";
        int port = 13000;
        ExecuteServer(host, port);
    }
}