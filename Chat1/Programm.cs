using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chat1
{
    public class Programm
    {
        public static event Action<string> MessageReceived;

        static void Main(string[] args)
        {
            MessageReceived += ConfirmMessageReceived;
            Server("Hello");
        }

        public static void ConfirmMessageReceived(string message)
        {
            Console.WriteLine($"Подтверждение: сообщение '{message}' получено.");
        }

        public static void Server(string name)
        {
            UdpClient udpClient = new UdpClient(12345);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Ожидание сообщений...");

            while (true)
            {
                byte[] buffer = udpClient.Receive(ref iPEndPoint);
                if (buffer == null) break;
                var messageText = Encoding.UTF8.GetString(buffer);
                MessageReceived?.Invoke(messageText); 
                Message message = Message.DeserializeFromJson(messageText);
                message.Print();
            }
        }
    }
}