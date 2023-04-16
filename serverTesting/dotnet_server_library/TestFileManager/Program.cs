using FileManager;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

class Program {
    static void Main(string[] args) {

        string head = @"D:\School\4610\API\local\";
        string temp = @"D:\School\4610\sld resource files\";
        LocalFileManage lFM = new LocalFileManage(head, temp);
        


        string url = "http://127.0.0.1:5000/";
        string pathHead = @"D:\School\4610\API\local\";
        string user = "weston";
        Console.WriteLine("SERVER: " + lFM.connectToServer(url, user, pathHead).ToString());

        
        lFM.updateCloudTree();
       // Console.WriteLine(lFM.listTree(false));
        lFM.refreshLocalFileList();
        Console.WriteLine(lFM.listTree(true));
        
        // dynamic settings = new JObject();
        // settings.name = "debugProject";
        // settings.units = "";
        // settings.prefix = "";
        // settings.suffix = "";
        Console.WriteLine(lFM.getFullPathFromName("file5.SLDPRT"));
        Console.WriteLine(lFM.testForLocalFile("file5.SLDPRT"));
        // lFM.createProject(settings);
        // lFM.createFileFromTemplate("inch", lFM.comb(head, "debugProject", "file5.SLDPRT"), false);
        // Console.WriteLine(lFM.listTree(true));
        
        // lFM.uploadFile("file5.SLDPRT");
        

        //test create new project
        

        //lFM.listCloudLookup();

        //Console.WriteLine(lFM.getCloudFileVersion("abc.SLDPRT"));

        


        /////////////////////////////
        //lFM.updateCloudTree();


        

        
        // lFM.addLocalProject("testB");

        // lFM.listProjects();

        // 
        // lFM.refreshLocalFileList();



        // List<string> projectNames = new List<string>();

        // projectNames.Add("a");
        // projectNames.Add("b");
        // projectNames.Add("c");
        // projectNames.Add("d");

        // Console.WriteLine("\n\r\n\rCREATING ROOTS");
        // lFM.updateLocalProjectsFromList(projectNames);

        // Console.Write(lFM.projectFromFilename(lFM.comb(head, "a\\b\\c", "file.SLDPRT")));


        
        // lFM.createFileFromTemplate("inch", lFM.comb(head, "a", "file.SLDPRT"), true);
        // lFM.createFileFromTemplate("inch", lFM.comb(head, "a\\b", "file2.SLDPRT"), true);



        // Console.WriteLine("\n\r\n\rlocalTree");
        // Console.WriteLine(lFM.listTree(true));
        // Console.WriteLine("\n\r\n\rcloudTree");
        // Console.WriteLine(lFM.listTree(false));
        // Console.WriteLine("\n\r\n\r");


        // foreach (var a in lFM.getTemplateNames("SLDPRT")){

        //     Console.WriteLine(lFM.getPathFromTemplateName("SLDPRT", a));
        //     Console.WriteLine(a);
        // }


        















        

        API api = new API(url, pathHead, user);

        // Console.WriteLine(api.testConnection());
        // Console.WriteLine(api.listProjects());



        // //create project
        // // dynamic settings = new JObject();
        // // settings.name = "Project B";
        // // settings.units = "inch";
        // // settings.prefix = "wshakespear";
        // // settings.suffix = "";

        // // bool res2 = api.createProject(settings);
        // // Console.WriteLine(res2);



        // // add file
        // string filePath = @"D:\School\4610\";

        // string project = "Project B";
        // string remote_dir = @"Project B\file\";
        // string fileName = "Brake_Disk.SLDPRT.BMP";
        // bool res = api.sendFile(filePath, project, remote_dir, fileName, true);

        // Console.WriteLine(res);
        

// read JSON directly from a file
    // using (StreamReader file = File.OpenText(@"c:\videogames.json"))
    // using (JsonTextReader reader = new JsonTextReader(file))
    // {
    //     JObject o2 = (JObject)JToken.ReadFrom(reader);
}

        



        // list projects
        // var res2 = api.listProjects();
        // foreach(var project in res2)
        //         {
        //             foreach( var file in project.files) {
        //                 Console.WriteLine(file);
        //             }
        //         }

        // Console.WriteLine(res2);

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
// }