using SW_Interface;

class SWCreate_Test {
    static void Main(string[] args) {
        SW_Create swC = new SW_Create();
        Console.WriteLine(swC.startFromArgs(args));
        int[] win = {0, 0, 3840/2, 2160 - 40};
        swC.setWindow(win);
        swC.setTaskPanePinned(true);
        swC.refreshWindow();
        
        int resX = 3840;
        int resY = 2160;
    }
}