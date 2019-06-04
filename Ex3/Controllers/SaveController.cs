using Ex3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

/// <summary>
/// The Ex3.Controllers Namespace Of The Controllers of the Project.
/// </summary>
namespace Ex3.Controllers
{
    /// <summary>
    /// The SaveController Class Which Extends The Controller Class is the controller of the save requests.
    /// </summary>
    public class SaveController : Controller
    {


        /// <summary>
        /// The SCENARIO_FILE is a const string of the relative path of the Files in the App_Data Folder.
        /// </summary>
        public const string SCENARIO_FILE = "~/App_Data/{0}.txt";           // The Path of the Secnario

        /// <summary>
        /// The Function Index Handles the request for save without parameters it returns a View.
        /// </summary>
        /// <returns>view.</returns>
        // GET: Save
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// The save Function which handles the save request.
        /// It gets as parameters a string ip, int port, int time, int timer and string filename.
        /// And returns the View which saves the animation from the simulator which is in the IP ip
        /// and port port in the intervals time for timer seconds in a file in the App_Data Relative Folder
        /// with the name filename.
        /// </summary>
        /// <param name="ip">string ip</param>
        /// <param name="port">int port</param>
        /// <param name="time">int time</param>
        /// <param name="timer">int timer</param>
        /// <param name="filename">string filename</param>
        /// <returns>returns the View which saves the animation from the simulator which is in the IP ip
        /// and port port in the intervals time for timer seconds in a file in the App_Data Relative Folder
        /// with the name filename.</returns>
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
                    scs.close();
                }
            }
            SimulatorCommunicator sc = new SimulatorCommunicator(ip, port);
            Session["sc"] = sc;

            return View();
        }

        /// <summary>
        /// The ToXML Function gets as a paramter a PlaneDataModel pdm and returns its string xml form.
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
        /// The GetPlaneData Function returns the string xml form of the plane data we get from the simulator.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// The Function SaveInFile Gets As Parameters A string filename And a string data.
        /// And Saves The Data In The File In The App_Data Relative Folder With The Name filename.
        /// </summary>
        /// <param name="filename">String filename of the name of the file to save in.</param>
        /// <param name="data">String data to save in the file.</param>
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