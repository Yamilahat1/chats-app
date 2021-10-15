using System;
using System.Collections.Generic;
using System.Text;
using Utilities;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;

namespace Deserializer
{
    public static class ListExtention
    { 
        public static string ExtractData(this List<char> buff)
        {
            return buff.ToString().Substring((int)Defines.DATA_BEGIN);
        }
    }

    class Deserializer
    {
        public LoginRequest deserializeLoginRequest(List<char> buff)
        {
            LoginRequest obj;
            XDocument xml = XDocument.Parse(buff.ExtractData()); // Create an XML "file" variable
            XmlSerializer reader = new XmlSerializer(typeof(LoginRequest));
            StreamReader file = new StreamReader(xml);
            obj.username = xml.
        }
    }
}
