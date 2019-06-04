using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Web;

/// <summary>
/// The Ex3.Models Namespace Of The Models Of The Project.
/// </summary>
namespace Ex3.Models
{
    /// <summary>
    /// The SimulatorCommunicator Class Which is responsible for the communication with the simulator.
    /// </summary>
    public class SimulatorCommunicator
    {
        /// <summary>
        /// The string IP of the simulator.
        /// </summary>
        public string IP
        {
            get;
            private set;
        }

        /// <summary>
        /// The int Port of the simulator.
        /// </summary>
        public int Port
        {
            get;
            private set;
        }

        /// <summary>
        /// The TcpClient tcpClient which will be the client of the simulator.
        /// </summary>
        private TcpClient tcpClient;

        /// <summary>
        /// The SimulatorCommunicator Constructor.
        /// </summary>
        /// <param name="ip">string ip of the Simulator.</param>
        /// <param name="port">int port of the Simulator.</param>
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

        /// <summary>
        /// The GetLatitude Function returns the Latitude from the simulator.
        /// </summary>
        /// <returns>returns the latitue.</returns>
        public double GetLatitude()
        {
            string latitudePath = "/position/latitude-deg";
            return GetValue(latitudePath, double.Parse);

        }

        /// <summary>
        /// The GetLongitude Function returns the Longitude from the simulator.
        /// </summary>
        /// <returns>returns the longitude.</returns>
        public double GetLongitude()
        {
            string longitudePath = "/position/longitude-deg";
            return GetValue(longitudePath, double.Parse);
        }

        /// <summary>
        /// The GetThrottle Function returns the Throttle from the simulator.
        /// </summary>
        /// <returns>returns the throttle.</returns>
        public double GetThrottle()
        {
            string throttlePath = "/controls/engines/current-engine/throttle";
            return GetValue(throttlePath, double.Parse);
        }

        /// <summary>
        /// The GetRudder Function returns the Rudder from the simulator.
        /// </summary>
        /// <returns>returns the rudder.</returns>
        public double GetRudder()
        {
            string rudderPath = "/controls/flight/rudder";
            return GetValue(rudderPath, double.Parse);
        }

        /// <summary>
        /// The GetValue Function gets as parameters a string path and a Func castingFunction
        /// and returns the value which we get from the simulator after running castingFunction on it.
        /// </summary>
        /// <typeparam name="T">return Type.</typeparam>
        /// <param name="path">string path of the value in the simulator.</param>
        /// <param name="castingFunction">Func castingFunction which casts the string value from the simulator to the T Type.</param>
        /// <returns>returns the value which we get from the simulator after running castingFunction on it.</returns>
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

        /// <summary>
        /// The close function closes the connection with the simulator.
        /// </summary>
        public void close()
        {
            tcpClient.Close();
        }

    }
}