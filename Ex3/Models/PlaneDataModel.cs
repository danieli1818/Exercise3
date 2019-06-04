using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

/// <summary>
/// The Ex3.Models Namespace Of The Models Of The Project.
/// </summary>
namespace Ex3.Models
{
    /// <summary>
    /// The PlaneDataModel Class Which Holds The Plane Data.
    /// </summary>
    public class PlaneDataModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Throttle { get; set; }
        public double? Rudder { get; set; }

        /// <summary>
        /// The Constructor of the PlaneDataModel.
        /// </summary>
        public PlaneDataModel()
        {

        }

        /// <summary>
        /// The PlaneDataModel Constructor of the PlaneDataModel which gets 2 parameters double latitude and double longitude.
        /// </summary>
        /// <param name="latitude">The double latitude.</param>
        /// <param name="longitude">The double longitude.</param>
        public PlaneDataModel(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            Throttle = null;
            Rudder = null;
        }

        /// <summary>
        /// The PlaneDataModel Constructor which gets as parameters double latitude, double longitude, double throttle and double rudder.
        /// </summary>
        /// <param name="latitude">The double latitude</param>
        /// <param name="longitude">The double longitude</param>
        /// <param name="throttle">The double throttle</param>
        /// <param name="rudder">The double rudder</param>
        public PlaneDataModel(double latitude, double longitude, double throttle, double rudder)
        {
            Latitude = latitude;
            Longitude = longitude;
            Throttle = throttle;
            Rudder = rudder;
        }
        /// <summary>
        /// The ToXML Function gets as a parameter a XmlWriter writer and writes to it the PlaneData's xml form.
        /// </summary>
        /// <param name="writer">The XmlWriter writer to write to.</param>
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

        /// <summary>
        /// The SetValue Function gets as parameters string name and double value and changes the property with the name, name to have the value, value.
        /// </summary>
        /// <param name="name">string name of the name of the property to change its value</param>
        /// <param name="value">double value of the new value of the property</param>
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