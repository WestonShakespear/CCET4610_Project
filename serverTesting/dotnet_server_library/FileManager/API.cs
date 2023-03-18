namespace FileManager;

using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;



public class API
{

    public dynamic sendJSON(string path, dynamic jsonData) {
        string response = string.Empty;
        WebRequest postRequest = null;
        HttpWebResponse postResponse = null;

        Uri uri = new Uri(path);

        postRequest = (HttpWebRequest)WebRequest.Create(uri);
        postRequest.ContentType = "application/json";
        postRequest.Method = "POST";

        using (StreamWriter streamWriter = new StreamWriter(postRequest.GetRequestStream()))
        {
            streamWriter.Write(jsonData.ToString());
        }



        postResponse = (HttpWebResponse)postRequest.GetResponse();
        using (StreamReader streamReader = new StreamReader(postResponse.GetResponseStream()))
        {
            response = streamReader.ReadToEnd();
        }
        dynamic retData = new JObject();

        retData = JObject.Parse(response);
        return retData;
    }
}


    




