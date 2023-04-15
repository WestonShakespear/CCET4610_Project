namespace FileManager;

using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
//Install the older version of the RestSharp NuGet package (<=106.15).
using RestSharp;



public class API
{

    private string localPathHead = "";
    private string user = "";
    private string baseURL = "";

    public API(string setBaseURL, string setLocalPathHead, string setLocalUser) {
        this.baseURL = setBaseURL;
        this.localPathHead = setLocalPathHead;
        this.user = setLocalUser;
    }

    public bool testConnection() {
        string url = this.baseURL + "/";

        var client = new RestClient(url);
        var request = new RestRequest(url, Method.Get);

        RestResponse response = client.Execute(request);


        var output = response.Content;
        if (output != null) {
            string data = output;
            if (data.Contains("testing")) {
                return true;
            }
        }
        return false;
    }

    public dynamic sendJSON(string path, dynamic jsonData) {
        string response = string.Empty;
        WebRequest postRequest = null;
        HttpWebResponse postResponse = null;

        Uri uri = new Uri(this.baseURL + path);

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

    public bool sendFile(string filePath, string project, string remoteDir, string fileName, bool resource) {
        string url = this.baseURL + "file_upload";

        Encode enc = new Encode();

        string[] fullPath = {filePath, remoteDir, fileName};

        byte[]? data = enc.readFileBytes(Path.Combine(fullPath));

        if (data != null) {
            // encode file
            string base64 = enc.encodeFileB64(data);
            string checksum = enc.getFileChecksum(base64);

            // execute post request
            var client = new RestClient(url);
            var request = new RestRequest(url, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            var body = new {
                content = base64,
                checksum = checksum,
                filePath = filePath,
                fileName = fileName,
                project = project,
                remoteDir = remoteDir,
                user = this.user,
                resource = resource.ToString()
            };
            var bodyy = JsonConvert.SerializeObject(body);
            request.AddBody(bodyy, "application/json");
            RestResponse response = client.Execute(request);
            var output = response.Content;

            if (output != null) {
                if (output.Contains("good")) {
                    return true;
                }
            }

            return false;

        }
        return false;
    }


    public bool recvFile(string remoteDir, string fileName) {
        string url = this.baseURL + "file_download";

        var client = new RestClient(url);
        var request = new RestRequest(url, Method.Get);
        request.AddParameter("fileName", fileName);
        request.AddParameter("remoteDir", remoteDir);

        RestResponse response = client.Execute(request);


        var output = response.Content;

        if (output != null) {
            dynamic responseData =  JsonConvert.DeserializeObject(output);

            string checksum = responseData.checksum;
            string content = responseData.content;

            Decode dec = new Decode();
            Encode enc = new Encode();

            if (enc.getFileChecksum(content) == checksum) {
                byte[] fileData = dec.decodeFileB64(content);

                return this.saveFile(fileData, remoteDir, fileName);
            }
        }
        
        return false;

    }


    public bool saveFile(byte[] fileData, string remoteDir, string fileName) {

        string location = this.localPathHead + remoteDir + fileName;
        string[] needed = remoteDir.Split(@"\");
        
        string[] dirs;
        string currentPath = this.localPathHead;
        string sub;

        for(int i = 0; i < needed.Length - 1;i++) {
            dirs = Directory.GetDirectories(currentPath, "*", SearchOption.TopDirectoryOnly);
            sub = currentPath + needed[i];
            //Console.WriteLine("Searching: " + currentPath+ " for: " + sub);

            if (!dirs.Any(sub.Contains)) {
                Directory.CreateDirectory(sub);
            }

            currentPath += needed[i];

            if (i != needed.Length - 1) {
                currentPath += @"\";
            } 
        }

        using var writer = new BinaryWriter(File.OpenWrite(location));
        writer.Write(fileData);

        return true;
    }


    public bool createProject(dynamic settings) {
        settings.owner = this.user;
        dynamic response = this.sendJSON("create_project", settings);

        try {
            string success = response.success;
            if (success.Contains("true")) {
                return true;
            }
        }
        catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException) {
            Console.WriteLine("error parsing");
        }

        return false;
        
    }

    public dynamic listProjects() {
        dynamic response = this.sendJSON("list_projects", new JObject());

        try {
            string success = response.success;
            if (success.Contains("true")) {
                var data = response.data;

                
                return data;
            }
        }
        catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException) {
            Console.WriteLine("error parsing");
        }

        return false;
        
    }

}


    




