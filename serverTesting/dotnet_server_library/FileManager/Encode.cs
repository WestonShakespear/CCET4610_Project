namespace FileManager;

using System.IO;



public class Encode
{

    public void test() {
        Console.WriteLine("testing");
    }

    public byte[]? readFileBytes(string filePath) {
        try {
            byte[] binary_data = File.ReadAllBytes(filePath);
            return binary_data;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred. {ex.Message}");
            return null;
        }        
    }

    public string encodeFileB64(byte[] binary_data) {
        string encoded = string.Empty;

        encoded = Convert.ToBase64String(binary_data);
        
        return encoded;
    }

    public string getFileChecksum(string encoded) {
        string checksum = string.Empty;

        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(encoded);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            checksum = Convert.ToHexString(hashBytes);
        }

        return checksum;
    }
}

