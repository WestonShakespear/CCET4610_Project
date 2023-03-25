using FileManager;
using Newtonsoft.Json.Linq;


class Program {
    static void Main(string[] args) {

        string url = "http://127.0.0.1:5000/";
        string pathHead = @"D:\School\4610\API\local\";
        string user = "weston";

        API api = new API(url, pathHead, user);

        Console.WriteLine(api.testConnection());

        string filePath = @"D:\School\4610\";
        string fileName = "Brake_Disk.SLDPRT";
        bool res = api.sendFile(filePath, @"Project B\file\", fileName);

    //     Console.WriteLine(res);

        // create project
        // dynamic settings = new JObject();
        // settings.name = "Project B";
        // settings.units = "inch";
        // settings.prefix = "wshakespear";
        // settings.suffix = "";

        // bool res = api.createProject(settings);
        // Console.WriteLine(res);



        // list projects
        var res2 = api.listProjects();
        foreach(var project in res2)
                {
                    foreach( var file in project.files) {
                        Console.WriteLine(file);
                    }
                }

        Console.WriteLine(res2);

        // string remoteDir = @"project\sub\";
        // string fileName = "bolt.SLDPRT";
        
        



        // bool res = api.recvFile(remoteDir, fileName);

        // Console.WriteLine(res);
        
        


        
    }

    // string url = "http://127.0.0.1:5000/";
    //     string pathHead = @"D:\School\4610\API\local\";
    //     string user = "weston";

    //     API api = new API(url, pathHead, user);

    //     Console.WriteLine(api.testConnection());

    //     string filePath = @"\\wsl$\Ubuntu\home\westonshakespear\ccet4610\flaskTest1\test\";
    //     string fileName = "bolt.SLDPRT";
    //     bool res = api.sendFile(filePath, @"New Project Name\file\", fileName);
































    // public static void fileUploadTest() {
    //     API api = new API();

    //     string filePath = @"\\wsl$\Ubuntu\home\westonshakespear\ccet4610\flaskTest1\test\";
    //     string fileName = "bolt.SLDPRT";

    //     string remoteDir = "project/sub/";

    //     string url = "http://127.0.0.1:5000/file_upload";

    //     Console.WriteLine("Uploading file:");
    //     Console.WriteLine("URL: " + url);
    //     Console.WriteLine("Name: " + fileName);
    //     Console.WriteLine("Upload from: "+ filePath);
    //     Console.WriteLine("Upload to: "+ remoteDir);


    //     bool res = api.sendFile(url, filePath, fileName, remoteDir);
    //     Console.WriteLine(res);

    // }

    // public static void fileDownloadTest() {
    //     API api = new API();

    //     string filePath = @"\\wsl$\Ubuntu\home\westonshakespear\ccet4610\flaskTest1\test\other\";
    //     string fileName = "bolt.SLDPRT";
    //     string remoteDir = "project/sub/";
    //     string url = "http://127.0.0.1:5000/file_download";

    //     Console.WriteLine("Downloading file:");
    //     Console.WriteLine("URL: " + url);
    //     Console.WriteLine("Name: " + fileName);
    //     Console.WriteLine("Download to: "+ filePath);
    //     Console.WriteLine("Download from: "+ remoteDir);

    //     bool res = api.recvFile(url, filePath, fileName, remoteDir);
    //     Console.WriteLine(res);
    // }

    // public static void testEncode() {
    //     Encode enc = new Encode();

    //     string filePath = @"\\wsl$\Ubuntu\home\westonshakespear\ccet4610\flaskTest1\test\bolt.SLDPRT";

    //     byte[]? data = enc.readFileBytes(filePath);

    //     if (data != null) {
    //         string base64 = enc.encodeFileB64(data);
    //         string checksum = enc.getFileChecksum(base64);

    //         // Console.WriteLine(base64);
    //         Console.WriteLine(checksum);
    //     }
    // }

    // public static void testDecode() {
    //     Decode man = new Decode();
    // }

    // public static void testJSONUpload() {
    //     API api = new API();

    //     dynamic jsonData = new JObject();
    //     jsonData.username = "weston";
    //     jsonData.password = "shakespear";

    //     dynamic response = api.sendJSON("http://127.0.0.1:5000/api_test", jsonData);
        
    //     Console.WriteLine(response.username);
    //     Console.WriteLine(response.password);
    // }
}