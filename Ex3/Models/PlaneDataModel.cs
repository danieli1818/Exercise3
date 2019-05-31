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
        public double? Throttle { get; set; }
        public double? Rudder { get; set; }

        public PlaneDataModel()
        {

        }

        public PlaneDataModel(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            Throttle = null;
            Rudder = null;
        }

        public PlaneDataModel(double latitude, double longitude, double throttle, double rudder)
        {
            Latitude = latitude;
            Longitude = longitude;
            Throttle = throttle;
            Rudder = rudder;
        }

        public void ToXML(XmlWriter writer)
        {
            writer.WriteStartElement("PlaneData");
            writer.WriteElementString("Latitude", Latitude.ToString());
            writer.WriteElementString("Longitude", Longitude.ToString());
            if (Throttle != null)
            {
                writer.WriteElementString("Throttle", Throttle.ToString());
            }
            if (Rudder != null)
            {
                writer.WriteElementString("Rudder", Rudder.ToString());
            }
            writer.WriteEndElement();
        }

        public void SetValue(string name, double value)
        {
            if (name.Equals("Latitude"))
            {
                Latitude = value;
            } else if (name.Equals("Longitude"))
            {
                Longitude = value;
            } else if (name.Equals("Throttle"))
            {
                Throttle = value;
            } else if (name.Equals("Rudder"))
            {
                Rudder = value;
            } else
            {
                throw new Exception("Not Valid Name!!!!");
            }
        }
    }
}