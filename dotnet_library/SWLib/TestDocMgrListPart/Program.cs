using SWLib;

class Program {
    static void Main(string[] args) {
        SW_Instance swC = new SW_Instance();
        
        swC.startFromArgs(args);

        SW_DocMgr doc = new SW_DocMgr(swC);

        string root = @"C:\Users\wes\github-repos\";
        // string imRoot = @"C:\Users\wes\Documents\";

        string[] files = {
            // @"ccet4610_project\testFiles\cube.SLDPRT",
            @"ccet4610_project\testFiles\cylinder.SLDPRT",
            @"ccet4610_project\testFiles\asm.SLDASM"
            // @"ccet4610_project\testFiles\drawing.SLDDRW",
        };


        doc.closeAll();
        // System.Threading.Thread.Sleep(1000);

        for (int a = 0; a < files.Length; a++) {
            string modelName = doc.openDoc(root + files[a], false);
            
            Console.WriteLine(doc.listAllDetails(modelName));
        }

        // doc.activate(a);


    }
}
