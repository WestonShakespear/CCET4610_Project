using SWLib;

class Program {
    static void Main(string[] args) {
        SW_Instance swC = new SW_Instance();
        
        Console.WriteLine(swC.startFromArgs(args));

        SW_DocMgr doc = new SW_DocMgr(swC);

        string root = @"C:\Users\wes\github-repos\";
        string imRoot = @"C:\Users\wes\Documents\";

        string[] files = {
            @"ccet4610_project\testFiles\cube.SLDPRT",
            @"ccet4610_project\testFiles\cylinder.SLDPRT",
            @"ccet4610_project\testFiles\asm.SLDASM",
            // @"ccet4610_project\testFiles\drawing.SLDDRW",
        };


        doc.closeAll();
        System.Threading.Thread.Sleep(1000);

        for (int a = 0; a < files.Length; a++) {
            string modelName = doc.openDoc(root + files[a], false);
            
            doc.savePreviewBMP(modelName, imRoot, 1080, 1080);
            // doc.close(modelName);
        }

        // doc.activate(a);


    }
}
