using Ex3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

/// <summary>
/// The Ex3.Controllers Namespace Of The Controllers.
/// </summary>
namespace Ex3.Controllers
{
    /// <summary>
    /// The DisplayController Class Which Extends The Controller Class.
    /// It is the controller of the display route.
    /// </summary>
    public class DisplayController : Controller
    {
        /// <summary>
        /// The function Index.
        /// </summary>
        /// <returns>ActionResult of the default view of the display.</returns>
        // GET: Display
        public ActionResult Index()
        {
            return View();
        }

        /*public ActionResult display(string ip, int port)
        {
            ViewBag.ip = ip;
            ViewBag.port = port;
            ViewBag.time = null;
            return View();
        }*/

        /// <summary>
        /// The displayIP Function which gets as parameters a string ip, int port, int time.
        /// It returns the View Of the request.
        /// </summary>
        /// <param name="ip">string ip of the request.</param>
        /// <param name="port">int port of the request.</param>
        /// <param name="time">int time of the request.</param>
        /// <returns>The View Of the request.</returns>
        public ActionResult displayIP(string ip, int port, int? time)
        {
            if (Session["sc"] != null)
            {
                SimulatorCommunicator scs = Session["sc"] as SimulatorCommunicator;
                if (scs != null)
                {
                    scs.close();
                }
            }
            SimulatorCommunicator sc = new SimulatorCommunicator(ip, port);
            Session["sc"] = sc;
            ViewBag.time = time;
            return View();
        }

        /// <summary>
        /// The isValidIPv4 function gets as a parameter a string ip and returns true if it a Valid IPv4 Address else false.
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public bool isValidIPv4(string ip)
        {
            if (String.IsNullOrWhiteSpace(ip))
            {
                return false;
            }

            string[] ipNumbersStrings = ip.Split('.');
            if (ipNumbersStrings.Length != 4)
            {
                return false;
            }

            byte parseByte;

            return ipNumbersStrings.All(number => byte.TryParse(number, out parseByte));
        }

        /// <summary>
        /// The SCENARIO_FILE is a const string of the Relative Path Of The App_Data Files.
        /// </summary>
        public const string SCENARIO_FILE = "~/App_Data/{0}.txt";           // The Path of the Secnario

        /// <summary>
        /// The displaySavedSimulation function gets as parameters a string filename and int time
        /// and returns the View which displays the simulation saved in the file with the filename name in the App_Data Folder
        /// Each Step In time Intervals.
        /// </summary>
        /// <param name="filename">string filename of the file to display.</param>
        /// <param name="time">int time intervals for the animation.</param>
        /// <returns>returns the View which displays the simulation saved in the file with the filename name in the App_Data Folder
        /// Each Step In time Intervals.</returns>
        public ActionResult displaySavedSimulation(string filename, int time)
        {
            if (Session["sc"] != null)
            {
                SimulatorCommunicator scs = Session["sc"] as SimulatorCommunicator;
                if (scs != null)
                {
                    scs.close();
                }
            }
            ViewBag.filename = filename;
            ViewBag.time = time;
            ViewBag.isSavedSimulation = true;
            return View();
        }

        /// <summary>
        /// The GetFileSavedData Function gets as a parameter a string filename
        /// and returns the content of the file with the filename name which is in the App_Data Relative Folder
        /// </summary>
        /// <param name="filename">string filename of the string name of the file in the App_Data Relative Folder
        /// which we want the content of.</param>
        /// <returns>returns the content of the file with the filename name which is in the App_Data Relative Folder.</returns>
        [HttpPost]
        public string GetFileSavedData(string filename)
        {
            string filepath = System.Web.HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, filename));
            string fileLines = System.IO.File.ReadAllText(filepath);
            return fileLines;
        }
        
        /// <summary>
        /// The display function which handles the request for a page with URI which ends with /display/[ip or filename]/[port or time]/[time or nothing]
        /// </summary>
        /// <param name="ip">string ip or filename.</param>
        /// <param name="port">int port or time.</param>
        /// <param name="time">int time or null.</param>
        /// <returns>returns the View Of the request.</returns>
        public ActionResult display(string ip, int port, int? time)
        {
            if (time != null || isValidIPv4(ip))
            {
                return displayIP(ip, port, time);
            } else
            {
                if (time != null)
                {
                    throw new Exception("Not Valid URI");
                }
                return displaySavedSimulation(ip, port);
            }
        }

        /*public ActionResult display(string content, string port, string time)
        {
            return 
        }*/

        /// <summary>
        /// The Function ToXML gets as a parameter a PlaneDataModel pdm and returns its string xml form.
        /// </summary>
        /// <param name="pdm">PlaneDataModel pdm.</param>
        /// <returns>returns pdm's string xml form.</returns>
        public string ToXML(PlaneDataModel pdm)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);
            writer.WriteStartDocument();

            pdm.ToXML(writer);

            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }

        /// <summary>
        /// The Function GetPlaneData returns the plane data xml form which we get from the simulator.
        /// </summary>
        /// <returns>returns the plane data xml form which we get from the simulator.</returns>
        [HttpPost]
        public string GetPlaneData()
        {
            if (Session["sc"] == null)
            {
                return null;
            } else
            {
                SimulatorCommunicator sc = Session["sc"] as SimulatorCommunicator;
                if (sc == null)
                {
                    return null;
                } else
                {
                    PlaneDataModel pdm = new PlaneDataModel(sc.GetLatitude(), sc.GetLongitude());
                    return ToXML(pdm);
                }
            }
        }
    }
}