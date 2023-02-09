namespace SWLib;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

public class SW_DocMgr
{
    private ISldWorks app;
    public SW_DocMgr(SW_Instance swInstanceIn) {
        this.app = swInstanceIn.getApp();
        Console.WriteLine("appin");
    }

    public void activate(string name) {
        int options = 0;
        int errors = 0;
        app.ActivateDoc3(name, true, options, ref errors);
    }

    public string open(string docName, bool readOnly) {
        int status = 0;
        int warnings = 0;

        ModelDoc2 model;

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
        return Path.GetFileNameWithoutExtension(docName);
    }
}