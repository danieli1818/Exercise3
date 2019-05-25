using Ex3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Ex3.Controllers
{
    public class DisplayController : Controller
    {
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

        public ActionResult displayIP(string ip, int port, int? time)
        {
            if (Session["sc"] != null)
            {
                SimulatorCommunicator scs = Session["sc"] as SimulatorCommunicator;
                if (scs != null)
                {
                    if (scs.IP.Equals(ip) && scs.Port == port)
                    {
                        ViewBag.time = time;
                        return View();
                    }
                    else
                    {
                        scs.close();
                    }
                }
            }
            SimulatorCommunicator sc = new SimulatorCommunicator(ip, port);
            Session["sc"] = sc;
            ViewBag.time = time;
            return View();
        }

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

        public const string SCENARIO_FILE = "~/App_Data/{0}.txt";           // The Path of the Secnario

        public ActionResult displaySavedSimulation(string filename, int time)
        {
            ViewBag.filename = filename;
            ViewBag.time = time;
            ViewBag.isSavedSimulation = true;
            return View();
        }

        [HttpPost]
        public string GetFileSavedData(string filename)
        {
            string filepath = System.Web.HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, filename));
            string fileLines = System.IO.File.ReadAllText(filepath);
            return fileLines;
        }
        
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