using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace XmlManagement
{
    static class XmlManagement
    {
		public static string Serialization(string root, Dictionary<string, string> data)
		{
			string str = string.Format("<{0}>", root);
			foreach (KeyValuePair<string, string> curr in data)
			{
				str += string.Format("<{0}>{1}</{0}>", curr.Key, curr.Value);
			}

			return string.Format("{0}</{1}>", str, root);
		}
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
