using System;
using System.Collections.Generic;
using System.Text;
using Utilities;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using XmlManagement;

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
            Dictionary<string, string> parsed = XmlManagement.XmlManagement.XmlDeserialization(buff.ExtractData());
            obj.username = parsed["Username"];
            obj.password = parsed["Password"];
            return obj;
        }
        public SignupRequest deserializeSignupRequest(List<char> buff)
        {
            SignupRequest obj;
            var parsed = XmlManagement.XmlManagement.XmlDeserialization(buff.ExtractData());
            obj.username = parsed["Username"];
            obj.password = parsed["Password"];
            return obj;
        }
    }
}
