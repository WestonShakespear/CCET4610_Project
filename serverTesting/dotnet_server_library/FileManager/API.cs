namespace FileManager;

using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
//Install the older version of the RestSharp NuGet package (<=106.15).
using RestSharp;



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

    public void sendFile(string url, string filePath, string fileName, string remoteDir) {
        Encode enc = new Encode();

        byte[]? data = enc.readFileBytes(filePath + fileName);

        if (data != null) {
            // encode file
            string base64 = enc.encodeFileB64(data);
            string checksum = enc.getFileChecksum(base64);
            Console.WriteLine(checksum);

            // pack file for post request
            // string postJsonString = string.Empty;
            // string responseContent = string.Empty;


            // dynamic postJson = new JObject();
            // postJson.content = base64;
            // postJson.checksum = checksum;
            // postJson.filePath = filePath;
            // postJson.fileName = fileName;
            // postJson.remoteDir = remoteDir;

            // postJsonString = postJson.ToString();

            // execute post request
            var client = new RestClient(url);
            var request = new RestRequest(url, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            var body = new {
                content = base64,
                checksum = checksum,
                filePath = filePath,
                fileName = fileName,
                remoteDir = remoteDir
            };
            var bodyy = JsonConvert.SerializeObject(body);
            request.AddBody(bodyy, "application/json");
            RestResponse response = client.Execute(request);
            var output = response.Content;
            Console.WriteLine(output);

            // RestRequest restRequest = new RestRequest(Method.POST);
            // restRequest.AddHeader("content-type", "application/json");
            // restRequest.AddParameter("application/json", postJsonString, ParameterType.RequestBody);
            // RestClient restClient = new RestClient(path);
            // IRestResponse iRestResponse = restClient.Execute(restRequest);

            // // test response
            // string errorMessage = iRestResponse.ErrorMessage;
            // if (string.IsNullOrEmpty(errorMessage))
            // {
            //     responseContent = iRestResponse.Content;
            // }
            // else
            // {
            //     responseContent = errorMessage;
            // }

            // Console.WriteLine(responseContent);

        }
    }


    public void recvFile(string url, string filePath, string fileName, string remoteDir) {
        var client = new RestClient(url);
        var request = new RestRequest(url, Method.Get);
        request.AddParameter("filePath", filePath);
        request.AddParameter("fileName", fileName);
        request.AddParameter("remoteDir", remoteDir);

        RestResponse response = client.Execute(request);


        
        dynamic responseData =  JsonConvert.DeserializeObject(response.Content);

        string checksum = responseData.checksum;
        string content = responseData.content;

        Decode dec = new Decode();
        Encode enc = new Encode();

        if (enc.getFileChecksum(content) != checksum) {
            Console.WriteLine("Checksum mismatch!");
        } else {
            Console.WriteLine("Checksum matches");
            byte[] fileData = dec.decodeFileB64(content);


        }

    }



    //  public string RestSharpPythonRestfulApiImageClassificationBase64(string urlWebAPI, string imagePathName, out string exceptionMessage)
    //     {
    //         string base64String = string.Empty;
    //         string imageJsonString = string.Empty;
    //         exceptionMessage = string.Empty;
    //         string responseContent = string.Empty;
    //         try
    //         {
    //             base64String = ImageFileToBase64String(imagePathName);
    //             imageJsonString = BuildImageJsonString(base64String);                
    //             RestRequest restRequest = new RestRequest(Method.POST);
    //             restRequest.AddHeader("content-type", "application/json");
    //             restRequest.AddParameter("application/json", imageJsonString, ParameterType.RequestBody);
    //             RestClient restClient = new RestClient(urlWebAPI);
    //             IRestResponse iRestResponse = restClient.Execute(restRequest);
    //             string errorMessage = iRestResponse.ErrorMessage;
    //             if (string.IsNullOrEmpty(errorMessage))
    //             {
    //                 responseContent = iRestResponse.Content;
    //             }
    //             else
    //             {
    //                 responseContent = errorMessage;
    //             }
    //         }
    //         catch (Exception ex)
    //         {
    //             exceptionMessage = $"An error occurred. {ex.Message}";
    //         }
    //         return responseContent;
    //     }





}


    




