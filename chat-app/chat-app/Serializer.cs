using System;
using System.Collections.Generic;
using System.Text;
using Utilities;

namespace Serializer
{
    class Serializer
    {
        private static List<char> SetToProtocol(string msg, int code)
        {
            List<char> buff = new List<char>();
            string length = msg.Length.ToString();
            buff.Add(Convert.ToChar(code));
            for (int i = 0; i < 4 - length.Length; i++) buff.Add('0');
            for (int i = 0; i < length.Length; i++) buff.Add(length[i]);
            for (int i = 0; i < msg.Length; i++) buff.Add(msg[i]);
            return buff;

        }

        public static List<char> SerializeResponse(SignupResponse res)
        {
            return SetToProtocol("{\"status\": " + res.status.ToString() + "}", 1);
        }
    }
}
