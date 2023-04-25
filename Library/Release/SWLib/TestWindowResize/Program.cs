using SWLib;

class Program {
    static void Main(string[] args) {
        SW_Instance swC = new SW_Instance();
        Console.WriteLine(swC.startFromArgs(args));
        int[] win = {0, 0, 3840/2, 2160 - 40};
        swC.setWindow(win);
        swC.setTaskPanePinned(true);
        swC.refreshWindow();
    }
}
