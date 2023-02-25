namespace SWLib;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;


public class SW_DocMgr
{
    private ISldWorks app;
    
    private IDictionary<string, ModelDoc2> openDocs = new Dictionary<string, ModelDoc2>();

    private string[] templatePaths = new string[3];

    public SW_DocMgr(SW_Instance swInstanceIn) {
        this.app = swInstanceIn.getApp();
        this.getDocTemplates();
    }

    public void activate(string name) {
        int options = 0;
        int errors = 0;
        app.ActivateDoc3(name, true, options, ref errors);
    }

    // isn't working for drawings
    public void close(string name) {
        this.app.CloseDoc(name);
    }

    public void closeAll() {
        this.app.CloseAllDocuments(true);
    }

    public string[,]? getDependents(string name) {
        ModelDocExtension modEx = this.getModelExtFromName(name);
        String[] vDepend = (String[])modEx.GetDependencies(true, true, true, true, true);

        int length = vDepend.Length;
        string[,] ret = new string[length/3, 3];
        if (length == 0) {
            return null;
        }

        for (int i = 0; i < vDepend.Length; i+=3) {
            ret[i/3, 0] = vDepend[i];
            ret[i/3, 1] = vDepend[i+1];
            ret[i/3, 2] = "True";

            string ro = vDepend[i+2];
            if (ro == "FALSE") {
                ret[i/3, 2] = "False";
            }

        }
        return ret;
    }

    public string[,]? getEquations(string name) {
        EquationMgr swEqnMgr = this.getEquMgrFromName(name);
      
        int nCount = swEqnMgr.GetCount();
        if (nCount == 0) {
            return null;
        }

        string[,] ret = new string[nCount, 4];
        for (int i = 0; i < nCount; i++)
        {
            ret[i, 0] = swEqnMgr.get_Equation(i);
            ret[i, 1] = swEqnMgr.get_Value(i).ToString();
            ret[i, 2] = swEqnMgr.Status.ToString();
            ret[i, 3] = swEqnMgr.get_GlobalVariable(i).ToString();
        }

        return ret;
    }

    private ModelDoc2 getModelFromName(string name) {
        return this.openDocs[name];
    }

    private ModelDocExtension getModelExtFromName(string name) {
        return (ModelDocExtension)this.getModelFromName(name).Extension;
    }

    private EquationMgr getEquMgrFromName(string name) {
        return (EquationMgr)this.getModelFromName(name).GetEquationMgr();
    }

    public string getPath(string name) {
        return this.getModelFromName(name).GetPathName();
    }

    public string getType(string name) {
        int type = this.getModelFromName(name).GetType();

        return type switch{
            (int)swDocumentTypes_e.swDocPART => "prt",
            (int)swDocumentTypes_e.swDocASSEMBLY   => "asm",
            (int)swDocumentTypes_e.swDocDRAWING   => "drw",
            _     => "err"
        };
    }

    public string listAllDetails(string name) {
        Console.WriteLine("---------------------------------");
        
        Console.WriteLine("  Name: " + name);
        Console.WriteLine("  Path: "+ this.getPath(name));
        Console.WriteLine();

        Console.WriteLine("  |Dependents|");
        // find dependents
        string[,]? depend = this.getDependents(name);
        if (depend != null) {
            for (int i = 0; i < depend.Rank; i++) {
                Console.WriteLine("    " + i.ToString() + ":");
                Console.WriteLine("      Name: " + depend[i, 0]);
                Console.WriteLine("      Path: " + depend[i, 1]);
                Console.WriteLine("      R/O:  " + depend[i, 2]);
            }
        } else {
            Console.WriteLine("    No dependents");
        }
        Console.WriteLine();

        //equations
        Console.WriteLine("  |Equations|");
        
        string[,]? equations = this.getEquations(name);

        if (equations != null) {
            for (int i = 0; i < equations.Rank; i++)
            {
                Console.WriteLine("    " + i + ":");
                Console.WriteLine("      Funct:  " + equations[i, 0]);
                Console.WriteLine("      Value:  " + equations[i, 1]);
                Console.WriteLine("      Index:  " + equations[i, 2]);
                Console.WriteLine("      Global: " + equations[i, 3]);
            }
        } else {
            Console.WriteLine("    No equations");
        }
        Console.WriteLine();
        





        return "";
    }

    public string openDoc(string docName, bool readOnly) {
        int status = 0;
        int warnings = 0;

        ModelDoc2 model = null;
        string name = Path.GetFileNameWithoutExtension(docName);

        int docOptions = (int)swOpenDocOptions_e.swOpenDocOptions_LoadModel;
        if (readOnly == true) {
            docOptions = (int)swOpenDocOptions_e.swOpenDocOptions_ReadOnly;
        }

        switch (Path.GetExtension(docName))
        {
            case ".SLDASM":
                AssemblyDoc asm = (AssemblyDoc)app.OpenDoc6(
                    docName,
                    (int)swDocumentTypes_e.swDocASSEMBLY,
                    docOptions, "", ref status, ref warnings);
                model = (ModelDoc2)asm;
                break;
            case ".SLDPRT":
                PartDoc part = (PartDoc)app.OpenDoc6(
                    docName,
                    (int)swDocumentTypes_e.swDocPART,
                    docOptions, "", ref status, ref warnings);
                model = (ModelDoc2)part; 
                break;

            case ".SLDDRW":
                DrawingDoc drawing = (DrawingDoc)app.OpenDoc6(
                    docName,
                    (int)swDocumentTypes_e.swDocDRAWING,
                    docOptions, "", ref status, ref warnings);
                model = (ModelDoc2)drawing;
                break;
        }
        if (model != null) {
            openDocs.Add(name, model);
            // 
        }
        return name;
    }

    public void savePreviewBMP(string model, string fileLocation, int x, int y) {
        this.openDocs[model].SaveBMP(fileLocation + model + ".bmp", x, y);
    }

    public void newDoc(string type, string location)
    {
        string template = type switch{
            "prt" => this.templatePaths[0],
            "asm" => this.templatePaths[1],
            "drw" => this.templatePaths[2],
            _     => type
        };

        ModelDoc2 model = (ModelDoc2)app.INewDocument2(template, (int)swDwgPaperSizes_e.swDwgPaperAsize, 0, 0);

        string name = Path.GetFileNameWithoutExtension(location);
        // Console.WriteLine("Name: " + name);
        // Console.WriteLine("Loc: " + location);
        // Console.WriteLine(model.GetPathName());

        // bool status = false;
        // int errors = 0;
        // int warnings = 0;
        // ModelDocExtension modEx = (ModelDocExtension)model.Extension;

        // int opt = (int)swSaveWithReferencesOptions_e.swSaveWithReferencesOptions_None;
        // object options = modEx.GetAdvancedSaveAsOptions(opt);

        // status = modEx.SaveAs3(location, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, options, ref errors, ref warnings);

        // Console.WriteLine(status);
        // Console.WriteLine(errors);
        // Console.WriteLine(warnings);

        openDocs.Add(name, model);
    }

    public void getDocTemplates() {
        this.templatePaths[0] = app.GetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swDefaultTemplatePart);
        this.templatePaths[1] = app.GetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swDefaultTemplateAssembly);
        this.templatePaths[2] = app.GetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swDefaultTemplateDrawing);
    }


    

}