using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace Server
{
    public class ApiAccess
    {
        /// <summary>
        /// Method will perform a GET request to a web server
        /// </summary>
        /// <param name="url"> The url for the request  </param>
        /// <returns> The value part </returns>
        static public string GetRequest(string url)
        {
            WebRequest wrGETURL = WebRequest.Create(url);
            Stream objStream = wrGETURL.GetResponse().GetResponseStream();

            StreamReader objReader = new StreamReader(objStream);
            dynamic json = JsonConvert.DeserializeObject(objReader.ReadToEnd());
            foreach (var category in json.categories)
            {
                if (category.Value == "explicit") return GetRequest(url);
            }
            return json.value;
        }
    }
}
