using FileManager;
using Newtonsoft.Json.Linq;


class Program {
    static void Main(string[] args) {
        // testEncode();
        // testDecode();


        testJSONUpload();
    }

    public static void testEncode() {
        Encode enc = new Encode();

        string filePath = @"\\wsl$\Ubuntu\home\westonshakespear\ccet4610\flaskTest1\test\bolt.SLDPRT";

        byte[]? data = enc.readFileBytes(filePath);

        if (data != null) {
            string base64 = enc.encodeFileB64(data);
            string checksum = enc.getFileChecksum(base64);

            // Console.WriteLine(base64);
            Console.WriteLine(checksum);
        }
    }

    public static void testDecode() {
        Decode man = new Decode();
    }

    public static void testJSONUpload() {
        API api = new API();

        dynamic jsonData = new JObject();
        jsonData.username = "weston";
        jsonData.password = "shakespear";

        dynamic response = api.sendJSON("http://127.0.0.1:5000/api_test", jsonData);
        
        Console.WriteLine(response.username);
        Console.WriteLine(response.password);
    }
}