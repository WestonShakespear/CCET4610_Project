namespace SWLib;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

public class SW_DocMgr
{
    private ISldWorks app;
    public SW_DocMgr(ISldWorks appIn) {
        this.app = appIn;
        Console.WriteLine("appin");
    }
}