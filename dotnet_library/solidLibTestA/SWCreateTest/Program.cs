using SW_Interface;


class SWCreate_Test {
    static void Main(string[] args) {
        SW_Create swC = new SW_Create();
        Console.WriteLine(swC.startFromArgs(args));
        
    }
}