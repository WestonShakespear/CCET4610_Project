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

    private Dictionary<string, Dictionary<string, string>> templates = new Dictionary<string, Dictionary<string, string>>();



    private Dictionary<string, string> localProjects = new Dictionary<string, string>();

    private Dictionary<string, List<string>> localFiles = new Dictionary<string, List<string>>();


    private Dictionary<string, Dictionary<string, Dictionary<string, string>>> localTree =
        new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

    private Dictionary<string, Dictionary<string, Dictionary<string, string>>> cloudTree =
        new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

    private Dictionary<string, string> localLookup = new Dictionary<string, string>();
    private Dictionary<string, string> cloudLookup = new Dictionary<string, string>();



    API? api;


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
            this.templates[type] = new Dictionary<string, string>();

            foreach (string fileName in fileList) {
                this.templates[type].Add(Path.GetFileNameWithoutExtension(fileName), fileName);
            }
        }
    }

    public List<string> getTemplateNames(string type) {
        List<string> ret = new List<string>();

        if (this.templates.ContainsKey(type)) {
            foreach(KeyValuePair<string, string> template in this.templates[type])
            {
                ret.Add(template.Key);
            }
        }
        
        
        
        return ret;
    }



    public string? getPathFromTemplateName(string type, string name) {
        if (this.templates.ContainsKey(type)) {
            if (this.templates[type].ContainsKey(name)) {
                return this.templates[type][name];
            }
            return null;
        }
        return null;
    }

    // public bool createFileFromTemplate(string template, string filename) {

    //     File.Copy(template, filename);


    //     return true;
    // }

    public string getJustExtension(string path)
    {
        return Path.GetExtension(path).Substring(1);
    }


    //template is something like in mm or m
    //returns null if file exists
    //true for override

    // return 0 for success
    // 1 for file exists
    // 2 for filename taken
    // 3 for fatal
    public int createFileFromTemplate(string template, string filepath, bool force) {

        string type = this.getJustExtension(filepath);
        string name = Path.GetFileName(filepath);
        string project = this.projectFromFilename(filepath);

        // if (this.localTree[this.projectFromFilename(filename)].ContainsKey(name))
        // {
        //     return 2;
        // }

        if (File.Exists(filepath)) {
            if (!force)
            {
                Console.WriteLine("exists but not overwrite");
                return 1;
            }
            
            Console.WriteLine("deleting");
            File.Delete(filepath);
        }

        if (this.templates.ContainsKey(type))
        {
            if (this.templates[type].ContainsKey(template))
            {
                string temp_path = this.templates[type][template];
                File.Copy(temp_path, filepath);

                this.addEmptyEntry(project, name, filepath);
            
                return 0;
            }
      
        }
        return 3;
    }

    // return 0 for success
    // 2 for filename taken
    public int addEmptyEntry(string project, string filename, string filepath)
    {

        if (this.localTree[project].ContainsKey(filename))
        {
            return 2;
        }

        this.localTree[project][filename] = new Dictionary<string, string>( );

        this.localTree[project][filename]["version"] = "0";


        this.localTree[project][filename]["path"] = this.comb(project, this.splitRootFromFile(this.localPathHead, filename));
        this.localTree[project][filename]["relations"] = "";

        this.localLookup[filename] = project;

        return 0;
    }

    public void updateEntryVersion(string filename, string version)
    {
        string project = this.localLookup[filename];
        if (this.localTree[project].ContainsKey(filename))
        {
            this.localTree[project][filename]["version"] = version;
        }
    }

    public void incEntryVersion(string filename)
    {
        string project = this.localLookup[filename];
        if (this.localTree[project].ContainsKey(filename))
        {
            string version = this.localTree[project][filename]["version"];
            version = (Int32.Parse(version) + 1).ToString();
            this.localTree[project][filename]["version"] = version;
        }
    }


    public string projectFromFilename(string path)
    {
        string ret = "";

        string rootStructure = this.splitRootFromFile(this.localPathHead, path);

        ret = rootStructure.Split("\\")[0];

        return ret;
    }


    private string splitRootFromFile(string root, string filePath)
    {
        string retPath = "";

        string[] rootSplit = root.Split(@"\", StringSplitOptions.RemoveEmptyEntries);
        string[] filePathSplit = filePath.Split(@"\", StringSplitOptions.RemoveEmptyEntries);

        for(int i = rootSplit.Length; i < filePathSplit.Length - 1; i++)
        {
            retPath += filePathSplit[i] + @"\";
        }

        
        return retPath;
    }


    public void listProjects() {
        foreach(KeyValuePair<string,string> project in this.localProjects) {
            Console.WriteLine("|Project|   |Path|");
            Console.WriteLine(project.Key + "    " + project.Value);
        }
    }

    // public void listFiles(string project) {
    //     List<string> files = this.localFiles[project];
    //     foreach (string file in files)
    //     {
    //         Console.Write("FILE: " + file);
    //     }
    // }



    public int addLocalProject(string name) {
        Console.WriteLine("Creating: " + name);

        string projectPath = this.comb(this.localPathHead, name);

        this.localTree[name] = new Dictionary<string, Dictionary<string, string>>();
        this.cloudTree[name] = new Dictionary<string, Dictionary<string, string>>();

        this.localProjects.Add(name, projectPath);

        this.addFolder(projectPath);
        return 1;
    }

    public string comb(string a, string b)
    {
        string[] temp = {a, b};

        return Path.Combine(temp);
    }

    public string comb(string a, string b, string c)
    {
        string[] temp = {a, b, c};

        return Path.Combine(temp);
    }

    

    public bool updateLocalProjectsFromList(List<string> roots)
    {
        foreach (string root in roots)
        {
            if (!this.localProjects.ContainsKey(root))
            {
                this.addLocalProject(root);
            }
            
            if (!this.localTree.ContainsKey(root))
            {
                this.localTree[root] = new Dictionary<string, Dictionary<string, string>>();
            }
            Console.WriteLine("creating root: " + root);
        }
        return true;
    }



    public string listTree(bool local) {
        string output = "";
        string n = "\n\r";
        string b = n+n;

        var tree = this.localTree;

        if (!local) {
            tree = this.cloudTree;
        }

        foreach( 
            KeyValuePair<string, 
            Dictionary<string, 
            Dictionary<string, string>>> 
            project in tree)
        {
            output += "-Project: " + project.Key + n;

            foreach(
                KeyValuePair<string, 
                Dictionary<string, string>>
                file in project.Value)
            {
                output += "    *" + file.Key + n;
                output += "        -version:" + file.Value["version"] + n;
                output += "        -path:" + file.Value["path"] + n;
                output += "        -relations:" + file.Value["relations"] + n;
            }
            
            output += b;

        }

        return output;
    }


    public bool refreshLocalFileList() {
        bool res = true;
        foreach (KeyValuePair<string,Dictionary<string, Dictionary<string, string>>> project in this.cloudTree) {
            res = res && this.refreshLocalProject(project.Key);
        }
        
        return res;
    }

    public string getFullPathFromName(string name)
    {
        string path = "";

        if (this.localLookup.ContainsKey(name))
        {
            path = this.comb(
                this.localPathHead,
                this.localTree[this.localLookup[name]][name]["path"],
                name);
        }

        return path;
    }

    public bool refreshLocalProject(string pname) {
        string search = this.comb(this.localPathHead, pname);

        if (!this.localTree.ContainsKey(pname)) {
            this.localTree[pname] = new Dictionary<string, Dictionary<string, string>>();

            string[] peerDirs = Directory.GetDirectories(this.localPathHead, "*", SearchOption.TopDirectoryOnly);
            

            if (peerDirs.Contains(search)) {
                // Console.WriteLine(pname);
                
                
            } else {
                this.addFolder(search);
            }
            return true;
        } else {
            string[] fileList = Directory.GetFiles(search, "*", SearchOption.AllDirectories);
            foreach (string filepath in fileList)
            {
                string filename = Path.GetFileName(filepath);
                
                // Console.WriteLine(filename);
                if (this.cloudTree[pname].ContainsKey(filename))
                {
                    // Console.WriteLine("Found info for: " + filename);
                    this.localTree[pname][filename] = this.cloudTree[pname][filename];
                    this.localLookup[filename] = pname;
                }
                
            }
            return true;
        }

        return false;
    }

    public bool addProject(string rootFolder) {


        return true;
    }

    //status 0 good
    //status 1 new
    //status 2 old
    // public int? getFileStatus(string filename)
    // {
    //     string pname = this.cloudLookup[filename];
    //     string localVersion = "";
    //     string cloudVersion = "";        

    //     if(this.localTree[pname].ContainsKey(filename))
    //     {
    //         localVersion = this.localTree[pname][filename]["version"];
    //     } else {
    //         return null;
    //     }

    //     if (this.cloudTree[pname].ContainsKey(filename))
    //     {
    //         cloudVersion = this.cloudTree[pname][filename]["version"];
    //     } else {
    //         return null;
    //     }

    //     int lV = Int32.Parse(localVersion);
    //     int cV = Int32.Parse(cloudVersion);

    //     if (lV == cV)
    //     {
    //         return 0;
    //     } 
    //     else if (lV > cV)
    //     {
    //         return 1;
    //     } 
    //     else
    //     {
    //         return 2;
    //     }
    // }




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

    // public string getPathFromName(string name) {
    //     if (this.localProjects.ContainsKey(name)) {
    //         return this.localProjects[name].; 
    //     } else {
    //         return "";
    //     }
    // }





//----------------------------------API METHODS----------------------------------//

// returns true for connected
    // false for not
    public bool connectToServer(string url, string user, string localHead)
    {
        bool result = false;

        try
        {
            this.api = new API(url, localHead, user);
            bool res = api.testConnection();

            result = res;
        }
        catch (Exception ex)
        {
            return false;
        }

        return result;
    }


    public bool serverUploadFile(string filePath, string relations)
    {
        string filename = Path.GetFileName(filePath);
        string imagename = filename + ".BMP";
        string pname = this.localLookup[filename];
        string? remoteDir = this.localTree[pname][filename]["path"];

        Console.WriteLine(remoteDir);


        string a = "    ";
        Console.WriteLine(a + pname);
        Console.WriteLine(a + remoteDir);
        Console.WriteLine(a + filename);
        Console.WriteLine(a + imagename);


        if (api != null)
        {
            string root = this.localPathHead;
            api.sendFile(root, pname, remoteDir, filename, relations, false);
            api.sendFile(root, pname, remoteDir, imagename, relations, true);

            return true;
        }
        return false;
    }

    public bool uploadFile(string filename, string relations)
    {
        this.serverUploadFile(filename, relations);
        this.incEntryVersion(filename);

        return true;
    }

    public bool serverCreateNewProject(dynamic settings)
    {
        bool res = false;

        if (api != null)
        {
            res = this.api.createProject(settings);
        }
        
        return res;
    }

    public bool createProject(dynamic settings)
    {
        string name = settings.name;
        if (name != null)
        {
            Console.WriteLine("Adding proj");
            this.addLocalProject(name);
            this.serverCreateNewProject(settings);

            return true;
        }

        return false;
    }

     public string getProjectCloudLookup(string filename)
     {
         string pname = this.cloudLookup[filename];

         return pname;
     }

    public void listCloudLookup()
    {
        foreach (KeyValuePair<string, string> file in this.cloudLookup)
        {
            Console.WriteLine(file.Key + "   " + file.Value);
        }
    }

    public string? getCloudFileVersion(string name)
    {
        if (this.cloudLookup.ContainsKey(name))
        {
            string pname = this.cloudLookup[name];
            var obj = this.cloudTree[pname][name];
            string version = obj["version"];

            return version;
        }
        
        return null;
    }

    public string? getLocalFileVersion(string name)
    {
        if (this.localLookup.ContainsKey(name))
        {
            string pname = this.localLookup[name];
            var obj = this.localTree[pname][name];
            string version = obj["version"];

            return version;
        }

        return null;
        
    }

    public bool testForLocalFile(string name)
    {
        if (this.localLookup.ContainsKey(name))
        {
            return true;
        }
        return false;
    }

    //status 0 good synced
    //status 1 new  push
    //status 2 old  pull
    public int? getFileStatus(string name)
    {
        if(this.cloudLookup.ContainsKey(name))
        {
            string pname = this.cloudLookup[name];

            if (this.localLookup.ContainsKey(name))
            {
                string c = this.cloudTree[pname][name]["version"];
                string l = this.localTree[pname][name]["version"];
                if (c == l)
                {
                    return 0;
                } else if (Int32.Parse(c) > Int32.Parse(l))
                {
                    return 2;
                } else {
                    return 1;
                }
            } else {
                return 2;
            }
        } else {
            return null;
        }
    }

    public bool downloadfile(string name)
    {
        if (this.cloudLookup.ContainsKey(name) && api != null)
        {
            string pname = this.cloudLookup[name];
            string localPath = this.comb(this.localPathHead, this.cloudTree[pname][name]["path"], name);

            Console.WriteLine("Project: " + pname);
            Console.WriteLine("Name: " + name);
            Console.WriteLine("path: " + localPath);

            return this.api.recvFile(pname, name, localPath);
        }
        return false;
    }





    




    public Dictionary<string, List<string>>? updateCloudTree()
    {
        if (api != null)
        {
            Dictionary<string, List<string>> tree = new Dictionary<string, List<string>>();

            this.cloudTree = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
            this.cloudLookup = new Dictionary<string, string>();

            dynamic res2 = api.listProjects();
            foreach (dynamic project in res2)
            {
                List<string> data = new List<string>();
                string pname = project.name;
                dynamic files = project.files;

                this.cloudTree[pname] = new Dictionary<string, Dictionary<string, string>>();

                // iterate over filenames
                foreach (var file in project["files"].Children())
                {
                    var filename = file.Name;
                    this.cloudTree[pname][filename] = new Dictionary<string, string>();
                    this.cloudLookup.Add(filename, pname);

                    var value = file.Value;

                    string version = value.version;
                    string path = value.path;

                    string relations = value.relations;
                    this.cloudTree[pname][filename].Add("version", version);
                    this.cloudTree[pname][filename].Add("path", path);
                    this.cloudTree[pname][filename].Add("relations", relations);

                    data.Add(filename);
                }

                tree.Add(pname, data);
            }

            return tree;
        } else {
            return null;
        }
        
    }

    public Dictionary<string, Dictionary<string, Dictionary<string, string>>> getCloudTree()
    {
        return this.cloudTree;
    }

    public Dictionary<string, Dictionary<string, Dictionary<string, string>>> getLocalTree()
    {
        return this.localTree;
    }



    }