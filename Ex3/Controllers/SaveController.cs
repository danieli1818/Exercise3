﻿using Ex3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Ex3.Controllers
{
    public class SaveController : Controller
    {



        public const string SCENARIO_FILE = "~/App_Data/{0}.txt";           // The Path of the Secnario

        // GET: Save
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult save(string ip, int port, int time, int timer, string filename)
        {
            ViewBag.time = time;
            ViewBag.timer = timer;
            ViewBag.filename = filename;
            string filepath = System.Web.HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, filename));
            // System.IO.File.WriteAllText(filepath, "");
            if (Session["sc"] != null)
            {
                SimulatorCommunicator scs = Session["sc"] as SimulatorCommunicator;
                if (scs != null)
                {
                    if (scs.IP.Equals(ip) && scs.Port == port)
                    {
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
            }
            else
            {
                SimulatorCommunicator sc = Session["sc"] as SimulatorCommunicator;
                if (sc == null)
                {
                    return null;
                }
                else
                {
                    PlaneDataModel pdm = new PlaneDataModel(sc.GetLatitude(), sc.GetLongitude(), sc.GetThrottle(), sc.GetRudder());
                    string xml = ToXML(pdm);
                    return xml;
                }
            }
        }

        [HttpPost]
        public void SaveInFile(string filename, string data)
        {
            if (filename == null)
            {
                throw new Exception("File Name Is Missing");
            } else
            {
                DataSaver ds = new DataSaver();
                ds.saveData(filename, data);
            }
        }
    }
}