using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace Ex3.Models
{
    public class DataSaver
    {

        public const string SCENARIO_FILE = "~/App_Data/{0}.txt";           // The Path of the Secnario

        public DataSaver()
        {

        }

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