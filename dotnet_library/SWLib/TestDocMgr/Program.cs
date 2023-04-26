using SWLib;

class Program {
    static void Main(string[] args) {



        



        SW_Instance swC = new SW_Instance();
        swC.connectToProcess(20216);
        
        // //Console.WriteLine(swC.startFromArgs(args));
        // // int[] win = {0, 0, 1920, 1080};
        // // swC.setWindow(win);
        // // swC.refreshWindow();

        SW_DocMgr doc = new SW_DocMgr(swC);

        doc.exportDWG("asdf");

        // doc.openDoc(@"D:\School\4610\API\local\Tuesday5\the assy.SLDASM", false);

        // doc.exportActiveDoc("-4242022-140", "x__t");
        //doc.exportDoc(@"D:\School\4610\API\local\Tuesday5\", "the assy", "-4242022-140", "x__t");


        // doc.activate("the assy");
        
        // var dep = doc.getDependents("the assy");
        // if (dep != null)
        // {
        //     Dictionary<string, Dictionary<string, string>> dependents = dep;

        //     foreach (KeyValuePair<string, Dictionary<string, string>> depSingle in dependents)
        //     {
        //         Console.WriteLine(depSingle.Value["name"]);
        //         Console.WriteLine(depSingle.Value["path"]);
        //     }
        // }
        

        // string root = @"C:\Users\Initec\source\github\";
        // string n = doc.openDoc(root + @"ccet4610_project\testFiles\cube.SLDPRT", false);

        // doc.openDoc(root + @"ccet4610_project\testFiles\cylinder.SLDPRT", false);
        // Console.WriteLine("ret" + n);
        // doc.openDoc(root + @"ccet4610_project\testFiles\asm.SLDASM", false);
        // doc.openDoc(@"C:\Users\Initec\source\github\ccet4610_project\testFiles\drawing.SLDDRW", false);
        // doc.activate(n);
        // doc.newDoc("prt", @"C:\Users\Initec\source\github\ccet4610_project\testFiles\create\testNew");
        // doc.newDoc("asm", @"C:\Users\Initec\source\github\ccet4610_project\testFiles\create\testNew2");
        // doc.newDoc("drw", @"C:\Users\Initec\source\github\ccet4610_project\testFiles\create\testNew3");
    }
}
