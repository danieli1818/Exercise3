using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Web;

namespace Ex3.Models
{
    public class SimulatorCommunicator
    {
        public string IP
        {
            get;
            private set;
        }

        public int Port
        {
            get;
            private set;
        }

        private TcpClient tcpClient;

        public SimulatorCommunicator(string ip, int port)
        {
            IP = ip;
            Port = port;
            tcpClient = new TcpClient();
            while (true)
            {
                try
                {
                    tcpClient.Connect(ip, port);
                    break;
                }
                catch (Exception e)
                {
                    if (e.GetType().Equals(SocketError.ConnectionRefused))
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            
            
        }

        public double GetLatitude()
        {
            string latitudePath = "/position/latitude-deg";
            return GetValue(latitudePath, double.Parse);

        }

        public double GetLongitude()
        {
            string longitudePath = "/position/longitude-deg";
            return GetValue(longitudePath, double.Parse);
        }

        public double GetThrottle()
        {
            string throttlePath = "/controls/engines/current-engine/throttle";
            return GetValue(throttlePath, double.Parse);
        }

        public double GetRudder()
        {
            string rudderPath = "/controls/flight/rudder";
            return GetValue(rudderPath, double.Parse);
        }

        public T GetValue<T>(string path, Func<string, T> castingFunction)
        {
            NetworkStream ns = tcpClient.GetStream();
            StreamWriter sw = new StreamWriter(ns);
            sw.WriteLine("get " + path);
            sw.Flush();
            StreamReader sr = new StreamReader(ns);
            string valueStr = sr.ReadLine();
            while (valueStr == null || !valueStr.StartsWith(path + " = '"))
            {
                valueStr = sr.ReadLine();
            }
            valueStr = valueStr.Substring((path + " = '").Length);
            valueStr = valueStr.Substring(0, valueStr.IndexOf("'"));
            T value = castingFunction(valueStr);
            return value;
        }

        public void close()
        {
            tcpClient.Close();
        }

    }
}