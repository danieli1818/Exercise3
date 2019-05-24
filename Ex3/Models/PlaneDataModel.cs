using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Ex3.Models
{
    public class PlaneDataModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public PlaneDataModel(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public void ToXML(XmlWriter writer)
        {
            writer.WriteStartElement("PlaneData");
            writer.WriteElementString("Latitude", Latitude.ToString());
            writer.WriteElementString("Longitude", Longitude.ToString());
            writer.WriteEndElement();
        }
    }
}