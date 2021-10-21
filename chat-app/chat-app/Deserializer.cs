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
            return string.Join("", buff.ToArray()).Substring((int)Defines.DATA_BEGIN);
        }
    }

    public class Deserializer
    {
        public static LoginRequest deserializeLoginRequest(List<char> buff)
        {
            LoginRequest obj;
            Dictionary<string, string> parsed = XmlManagement.XmlManagement.XmlDeserialization(buff.ExtractData());
            obj.username = parsed["Username"];
            obj.password = parsed["Password"];
            return obj;
        }
        public static SignupRequest deserializeSignupRequest(List<char> buff)
        {
            SignupRequest obj;
            var parsed = XmlManagement.XmlManagement.XmlDeserialization(buff.ExtractData());
            obj.username = parsed["Username"];
            obj.password = parsed["Password"];
            return obj;
        }
        public static SignoutRequest deserializeSignoutRequest(List<char> buff)
        {
            SignoutRequest obj;
            var parsed = XmlManagement.XmlManagement.XmlDeserialization(buff.ExtractData());
            obj.username = parsed["Username"];
            return obj;
        }
    }
}
