using Ex3.Models;
using System;
using System.Collections.Generic;
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

        public ActionResult display(string ip, int port, int? time)
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
                    } else
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