using SWLib;

class Program {
    static void Main(string[] args) {
        SW_Instance swC = new SW_Instance();
        Console.WriteLine(swC.startFromArgs(args));
    }
}
