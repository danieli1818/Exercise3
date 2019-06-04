using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

/// <summary>
/// The Ex3.Models Namespace Of The Models Of The Project.
/// </summary>
namespace Ex3.Models
{
    /// <summary>
    /// The DataSaver Class Which Is Responsible To Save Data In Files In The App_Data Relative Folder.
    /// </summary>
    public class DataSaver
    {
        /// <summary>
        /// The SCENARIO_FILE const string of the App_Data Files Relative Path.
        /// </summary>
        public const string SCENARIO_FILE = "~/App_Data/{0}.txt";           // The Path of the Secnario

        /// <summary>
        /// The DataSaver Constructor.
        /// </summary>
        public DataSaver()
        {

        }

        /// <summary>
        /// The saveData Function gets as parameters a string filename and a string data and saves the data in the file with the filename name in the App_Data Folder.
        /// </summary>
        /// <param name="filename">The string filename of the file to save in.</param>
        /// <param name="data">string data to save.</param>
        public void saveData(string filename, string data)
        {
            List<PlaneDataModel> datas = ConvertDataFromStringToListOfPlaneDataModels(data);
            string filepath = System.Web.HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, filename));
            System.IO.File.WriteAllText(@filepath, "");
            if (datas.Count >= 1)
            {
                string xml = ToXML(datas[0]);
                System.IO.File.AppendAllText(filepath, xml);
            }
            for (int i = 1; i < datas.Count; i++)
            {
                string xml = ToXML(datas[i]);
                xml = "\r\n" + xml;
                System.IO.File.AppendAllText(filepath, xml);
            }
        }

        /// <summary>
        /// The ConvertDataFromStringToListOfPlaneDataModels gets as a parameter a string data and converts it to a List of PlaneDataModels.</PlaneDataModel>
        /// </summary>
        /// <param name="data">string data to convert to a List of PlaneDataModels</param>
        /// <returns>returns a List of PlaneDataModels of the string data.</returns>
        private List<PlaneDataModel> ConvertDataFromStringToListOfPlaneDataModels(string data)
        {
            List<PlaneDataModel> datas = new List<PlaneDataModel>();
            string[] stringDatas = data.Split(new string[] { "{", "}" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string stringData in stringDatas)
            {
                if (stringData.Equals("[") || stringData.Equals(",") || stringData.Equals("]"))
                {
                    continue;
                }
                datas.Add(ConvertDataFromStringToPlaneDataModel(stringData));
            }
            return datas;
        }

        /// <summary>
        /// The ConvertDataFromStringToPlaneDataModel Function gets as a parameter a string data and converts it to the PlaneDataModel.
        /// </summary>
        /// <param name="data">string data to convert to PlaneDataModel</param>
        /// <returns>the PlaneDataModel of the string data.</returns>
        private PlaneDataModel ConvertDataFromStringToPlaneDataModel(string data)
        {
            string[] planeDatas = data.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            PlaneDataModel pdm = new PlaneDataModel();
            foreach (string planeData in planeDatas)
            {
                string[] nameAndValue = planeData.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                string name = nameAndValue[0].Substring(1, nameAndValue[0].Length - 2);
                string value = nameAndValue[1];
                if (value.StartsWith("\""))
                {
                    value = value.Substring(1);
                }
                if (value.EndsWith("\""))
                {
                    value = value.Substring(0, value.Length - 1);
                }
                pdm.SetValue(name, double.Parse(value));
            }
            return pdm;
        }

        /// <summary>
        /// The ToXML Function gets as a parameter a PlaneDataModel pdm and returns its string xml form.
        /// </summary>
        /// <param name="pdm">PlaneDataModel pdm.</param>
        /// <returns>returns the string xml form of the pdm.</returns>
        private string ToXML(PlaneDataModel pdm)
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


    }
}