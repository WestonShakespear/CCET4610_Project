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
            // model.SaveBMP(docName + ".bmp", 1080, 1080);
        }
        return name;
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