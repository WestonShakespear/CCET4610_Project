using SWLib;

class Program {
    static void Main(string[] args) {
        SW_Instance swC = new SW_Instance();
        
        Console.WriteLine(swC.startFromArgs(args));
        int[] win = {0, 0, 1920, 1080};
        swC.setWindow(win);
        swC.refreshWindow();

        SW_DocMgr doc = new SW_DocMgr(swC);

        string root = @"C:\Users\Initec\source\github\";
        string n = doc.openDoc(root + @"ccet4610_project\testFiles\cube.SLDPRT", false);

        doc.openDoc(root + @"ccet4610_project\testFiles\cylinder.SLDPRT", false);
        Console.WriteLine("ret" + n);
        doc.openDoc(root + @"ccet4610_project\testFiles\asm.SLDASM", false);
        doc.openDoc(@"C:\Users\Initec\source\github\ccet4610_project\testFiles\drawing.SLDDRW", false);
        doc.activate(n);
        doc.newDoc("prt", @"C:\Users\Initec\source\github\ccet4610_project\testFiles\create\testNew.SLDPRT");
        doc.newDoc("asm", @"C:\Users\Initec\source\github\ccet4610_project\testFiles\create\testNew2.SLDASM");
        doc.newDoc("drw", @"C:\Users\Initec\source\github\ccet4610_project\testFiles\create\testNew3.SLDDRW");
    }
}
