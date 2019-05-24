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
            tcpClient.Connect(ip, port);
        }

        public double GetLatitude()
        {
            NetworkStream ns = tcpClient.GetStream();
            StreamWriter sw = new StreamWriter(ns);
            sw.WriteLine("get /position/latitude-deg");
            sw.Flush();
            StreamReader sr = new StreamReader(ns);
            string latitudeStr = sr.ReadLine();
            while (latitudeStr == null || !latitudeStr.StartsWith("/position/latitude-deg = '"))
            {
                latitudeStr = sr.ReadLine();
            }
            latitudeStr = latitudeStr.Substring("/position/latitude-deg = '".Length);
            latitudeStr = latitudeStr.Substring(0, latitudeStr.IndexOf("'"));
            double latitude = double.Parse(latitudeStr);
            return latitude;

        }

        public double GetLongitude()
        {
            NetworkStream ns = tcpClient.GetStream();
            StreamWriter sw = new StreamWriter(ns);
            sw.WriteLine("get /position/longitude-deg");
            sw.Flush();
            StreamReader sr = new StreamReader(ns);
            string longitudeStr = sr.ReadLine();
            while (longitudeStr == null || !longitudeStr.StartsWith("/position/longitude-deg = '"))
            {
                longitudeStr = sr.ReadLine();
            }
            longitudeStr = longitudeStr.Substring("/position/longitude-deg = '".Length);
            longitudeStr = longitudeStr.Substring(0, longitudeStr.IndexOf("'"));
            double longitude = double.Parse(longitudeStr);
            return longitude;
        }

        public void close()
        {
            tcpClient.Close();
        }

    }
}