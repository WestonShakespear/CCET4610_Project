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

    public Dictionary<string, Dictionary<string, string>>? getDependents(string name) {

        ModelDocExtension modEx = this.getModelExtFromName(name);
        String[] vDepend = (String[])modEx.GetDependencies(true, true, true, true, true);

        int length = vDepend.Length;
        if (length == 0) {
            return null;
        }

        Dictionary<string, Dictionary<string, string>> ret;
        ret = new Dictionary<string, Dictionary<string, string>>();

        for (int i = 0; i < vDepend.Length; i+=3) {
            Dictionary<string, string> data = new Dictionary<string, string>();

            data.Add("name", vDepend[i]);
            data.Add("path", vDepend[i+1]);

            string ro = vDepend[i+2];
            if (ro == "FALSE") {
                data.Add("rw", "False");
            } else {
                data.Add("rw", "False");
            }
            ret.Add(data["name"], data);

        }
        return ret;
    }

    public Dictionary<string, Dictionary<string, string>>? getEquations(string name) {
        EquationMgr swEqnMgr = this.getEquMgrFromName(name);
      
        int nCount = swEqnMgr.GetCount();
        if (nCount == 0) {
            return null;
        }

        Dictionary<string, Dictionary<string, string>> ret;
        ret = new Dictionary<string, Dictionary<string, string>>();

        for (int i = 0; i < nCount; i++)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            data.Add("name", swEqnMgr.get_Equation(i));
            data.Add("value", swEqnMgr.get_Value(i).ToString());
            data.Add("index", swEqnMgr.Status.ToString());
            data.Add("global", swEqnMgr.get_GlobalVariable(i).ToString());

            ret.Add(data["name"], data);
        }

        return ret;
    }

    public Dictionary<string, Dictionary<string, string>>? getFeatures(string name) {
        FeatureManager featMgr = this.getFeatureMgrFromName(name);
        int featureCount = featMgr.GetFeatureCount(true);

        if (featureCount == 0) {
            return null;
        }

        Feature feat = (Feature)this.getModelFromName(name).FirstFeature();

        Dictionary<string, Dictionary<string, string>> ret;

        ret = new Dictionary<string, Dictionary<string, string>>();

        for (int f = 0; f < featureCount; f++) {

            Dictionary<string, string> featData = new Dictionary<string, string>();

            string featureName = feat.Name;
            featData.Add("name", featureName);

            featData.Add("type", feat.GetTypeName2());
            featData.Add("extref", feat.ListExternalFileReferencesCount().ToString());

            featData.Add("base", feat.IsBase2().ToString());


            feat = (Feature)feat.GetNextFeature();

            ret.Add(featData["name"], featData);
        }
        return ret;
    }

    public void getConfigurations(string name) {
        ConfigurationManager configMgr = this.getConfigMgrFromName(name);

        Configuration activeConfig = (Configuration)configMgr.ActiveConfiguration;


        Console.WriteLine("    Current: " + activeConfig.Name);

        bool derived = activeConfig.IsDerived();
        Console.WriteLine("    Derived: " + derived.ToString());

        if (derived == true) {
            string parentName = activeConfig.GetParent().Name;
            Console.WriteLine("        From: " + name);
        }
    }

    

    

    public string listAllDetails(string name) {
        Console.WriteLine("---------------------------------");
        
        Console.WriteLine("  Name: " + name);
        Console.WriteLine("  Path: "+ this.getPath(name));
        Console.WriteLine("  Type: "+ this.getType(name));
        Console.WriteLine();


        // find dependents
        Console.WriteLine("  |Dependents|");
        
        Dictionary<string, Dictionary<string, string>>? depend;
        depend = this.getDependents(name);
        this.outputDictionaryInfo(depend);
        Console.WriteLine();


        //equations
        Console.WriteLine("  |Equations|");
        
        Dictionary<string, Dictionary<string, string>>? equations;
        equations = this.getEquations(name);
        this.outputDictionaryInfo(equations);
        Console.WriteLine();


        //features and their references
        Console.WriteLine("  |Features|");

        Dictionary<string, Dictionary<string, string>>? features;
        features = this.getFeatures(name);
        this.outputDictionaryInfo(features);
        Console.WriteLine();
 
        return "";
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

    private ConfigurationManager getConfigMgrFromName(string name) {
        return (ConfigurationManager)this.getModelFromName(name).ConfigurationManager;
    }

    private FeatureManager getFeatureMgrFromName(string name) {
        return (FeatureManager)this.getModelFromName(name).FeatureManager;
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

    public void outputDictionaryInfo(Dictionary<string, Dictionary<string, string>>? dict) {
        if (dict != null) {
            int i = 0;
            foreach (var item in dict) {
                Console.WriteLine("    " + i.ToString() + ":");
                i++;
                foreach (var value in item.Value) {
                    Console.WriteLine("      " + value.Key + ":  " + value.Value);
                }
            }
        } else {
            Console.WriteLine("    None");
        }
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
            try
            {
                openDocs.Add(name, model);
            } 
            catch (System.ArgumentException)
            {

            }
            
            // 
        }
        return name;
    }

    public void savePreviewBMP(string model, string fileLocation, int x, int y) {
        this.openDocs[model].ViewZoomtofit2();
        this.openDocs[model].SaveBMP(fileLocation, x, y);
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