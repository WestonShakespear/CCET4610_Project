using SWLib;

class Program {
    static void Main(string[] args) {
        SW_Instance swC = new SW_Instance();
        Console.WriteLine(swC.startFromArgs(args));

        SW_DocMgr doc = new SW_DocMgr(swC);
        string n = doc.open(@"C:\Users\Initec\source\github\ccet4610_project\testFiles\cube.SLDPRT", false);
        Console.WriteLine("ret" + n);
        doc.open(@"C:\Users\Initec\source\github\ccet4610_project\testFiles\asm.SLDASM", false);
        doc.open(@"C:\Users\Initec\source\github\ccet4610_project\testFiles\drawing.SLDDRW", false);
        doc.activate(n);
    }
}
