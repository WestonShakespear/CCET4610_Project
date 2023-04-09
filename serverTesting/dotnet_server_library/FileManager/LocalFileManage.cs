namespace FileManager;

using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
//Install the older version of the RestSharp NuGet package (<=106.15).
using RestSharp;



public class LocalFileManage
{

    private string localPathHead = "";

    private string localTemplateDir = "";

    private Dictionary<string, List<string>> templates = new Dictionary<string, List<string>>();



    private Dictionary<string, string> localProjects = new Dictionary<string, string>();

    private Dictionary<string, List<string>> localFiles = new Dictionary<string, List<string>>();


    public LocalFileManage(string setLocalPathHead, string setLocalTemplateDir) {
        this.localPathHead = setLocalPathHead;
        this.localTemplateDir = setLocalTemplateDir;

        this.createTemplateDictionary();

    }

    private void createTemplateDictionary() 
    {
        string[] types = {"SLDPRT", "SLDASM", "SLDDRW"};

        foreach (string type in types)
        {
            string[] fileList = Directory.GetFiles(this.localTemplateDir, "*." + type, SearchOption.AllDirectories);
            this.templates[type] = new List<string>();

            foreach (string fileName in fileList) {
                this.templates[type].Add(fileName);
            }
        }
    }

    public List<string> getTemplateNames(string type) {
        List<string> ret = new List<string>();

        if (this.templates.ContainsKey(type)) {
            foreach(string path in this.templates[type])
            {
                ret.Add(Path.GetFileNameWithoutExtension(path));
            }
        }
        
        
        
        return ret;
    }

    public void listProjects() {
        foreach(KeyValuePair<string,string> project in this.localProjects) {
            Console.WriteLine("|Project|   |Path|");
            Console.WriteLine(project.Key + "    " + project.Value);

            this.listFiles(project.Key);
        }
    }

    public void listFiles(string project) {
        List<string> files = this.localFiles[project];
        foreach (string file in files)
        {
            Console.Write("FILE: " + file);
        }
    }



    public bool createProject(string name) {
        Console.WriteLine("Creating: " + name);

        string projectPath = this.localPathHead + name;

        this.addFolder(projectPath);
        this.localProjects.Add(name, projectPath);
        this.localFiles[name] = new List<string>();


        return true;
    }

    public bool refreshLocalFileList() {
        bool res = true;
        foreach (KeyValuePair<string,string> project in this.localProjects) {
            res = res && this.refreshLocalProject(project.Key);
        }
        
        return res;
    }

    public bool createRootsFromList(List<string> roots)
    {
        foreach (string root in roots)
        {
            this.createProject(root);
        }
        


        return true;
    }



    public bool refreshLocalProject(string name) {
        if (this.localProjects.ContainsKey(name)) {
            string projectPath = this.localPathHead + name;

            string[] fileList = Directory.GetFiles(projectPath, "*", SearchOption.AllDirectories);

            foreach (string file in fileList) {
                bool inFileList = this.localFiles[name].Contains(file);

                if (!inFileList) {
                    this.localFiles[name].Add(file);
                }
                Console.WriteLine(file + "  " + inFileList.ToString());
            }

            return true;
        }


        

        return false;
    }

    public bool addProject(string rootFolder) {


        return true;
    }




    private bool addFolder(string path) {
        Console.WriteLine(path);
        if (Directory.GetParent(path) != null) {
                string parentFolder = Directory.GetParent(path).FullName;

                string[] peerDirs = Directory.GetDirectories(parentFolder, "*", SearchOption.TopDirectoryOnly);

                if (!peerDirs.Any(path.Contains)) {
                    Directory.CreateDirectory(path);
                    Console.WriteLine("created");
                } else {
                    Console.WriteLine("exists");
                }
            

            
        }
        return true;
    }

    private bool moveStructure(string rootPath) {


        return true;
    }

    private bool addNewFilePath(string path) {


        return true;
    }





    

}


    




