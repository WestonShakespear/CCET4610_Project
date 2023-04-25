using SWLib;

class Program {
    static void Main(string[] args) {
        SW_Instance swC = new SW_Instance();
        
        swC.startFromArgs(args);

        SW_DocMgr doc = new SW_DocMgr(swC);

        string root = @"C:\Users\wes\github-repos\";
        // string imRoot = @"C:\Users\wes\Documents\";

        string[] files = {
            @"ccet4610_project\testFiles\cube.SLDPRT",
            @"ccet4610_project\testFiles\cylinder.SLDPRT",
            @"ccet4610_project\testFiles\asm.SLDASM",
            @"ccet4610_project\testFiles\drawing.SLDDRW",
        };


        doc.closeAll();
        // System.Threading.Thread.Sleep(1000);

        for (int a = 0; a < files.Length; a++) {
            string modelName = doc.openDoc(root + files[a], false);
            
            Console.WriteLine(doc.listAllDetails(modelName));
        }

        // doc.activate(a);

        

        // Dictionary<string, Dictionary<string, string>> v = new Dictionary<string, Dictionary<string, string>>();

        // Dictionary<string, string> m = new Dictionary<string, string>();

        // m.Add("test", "this");
        // m.Add("okay", "yes");

        // v.Add("first", m);

        // foreach (var item in v) {
        //     Console.WriteLine(item.Value["test"]);
        // }

        
    }
}
