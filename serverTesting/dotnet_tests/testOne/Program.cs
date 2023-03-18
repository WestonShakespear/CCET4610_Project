using System;
using System.Drawing;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using RestSharp;



// string exceptionMessage = string.Empty;
// string webResponse = string.Empty;

// string uirWebAPI = "http://127.0.0.1:5000/api_test";


// try
// {
//     Uri uri = new Uri(uirWebAPI);
//     WebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
//     httpWebRequest.ContentType = "application/json";
//     httpWebRequest.Method = "POST";
//     using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
//     {
//         dynamic employee = new JObject();
//         employee.username = "theUserName";
//         employee.password = "thePassword";
//         streamWriter.Write(employee.ToString());
//     }
//     HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
//     using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
//     {
//         webResponse = streamReader.ReadToEnd();
//     }
// }
// catch (Exception ex)
// {
//     exceptionMessage = $"An error occurred. {ex.Message}";
// }

// Console.WriteLine(webResponse);


// Console.WriteLine(exceptionMessage);

string filePath = @"\\wsl$\Ubuntu\home\westonshakespear\ccet4610\flaskTest1\test\bolt.SLDPRT";
string base64_encoded = string.Empty;

byte[] fileBytes = File.ReadAllBytes(filePath);
base64_encoded = Convert.ToBase64String(fileBytes);
// Console.WriteLine(base64_encoded);


string hash = string.Empty;

using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
    {
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(base64_encoded);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        hash = Convert.ToHexString(hashBytes); // .NET 5 +

   
    }

Console.WriteLine(hash);




string outPath = @"\\wsl$\Ubuntu\home\westonshakespear\ccet4610\flaskTest1\test\outc.SLDPRT";

byte[] outBytes = Convert.FromBase64String(base64_encoded);
File.WriteAllBytes(outPath, outBytes);