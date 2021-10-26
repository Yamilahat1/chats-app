using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace XmlManagement
{
    internal static class XmlManagement
    {
        /// <summary>
        /// Method will serialize message into XML
        /// </summary>
        /// <param name="root"> The root tag </param>
        /// <param name="data"> The data of the XML, represented as a dictionary </param>
        /// <returns></returns>
        public static string Serialization(string root, Dictionary<string, string> data)
        {
            string str = string.Format("<{0}>", root);
            foreach (KeyValuePair<string, string> curr in data)
            {
                str += string.Format("<{0}>{1}</{0}>", curr.Key, curr.Value);
            }

            return string.Format("{0}</{1}>", str, root);
        }

        /// <summary>
        /// Method will deserialize XML into dictionary of tags and their values
        /// </summary>
        /// <param name="xml"> The XML as a string </param>
        /// <returns> A dictionary which represents each tag and its value </returns>
        public static Dictionary<string, string> XmlDeserialization(string xml)
        {
            var res = new Dictionary<string, string>();
            Regex rg = new Regex(@"<(?<Tag>\w+)>(?<Data>[^<]*)");
            MatchCollection matches = rg.Matches(xml);
            foreach (Match match in matches) if (match.Groups["Data"].Value != "") res.Add(match.Groups["Tag"].Value, match.Groups["Data"].Value);
            return res;
        }
    }
}