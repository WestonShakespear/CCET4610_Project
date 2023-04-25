namespace FileManager;

public class Decode
{
    public void test() {
        Console.WriteLine("testing1");
    }

    public byte[] decodeFileB64(string content) {
        return Convert.FromBase64String(content);
    }

}
