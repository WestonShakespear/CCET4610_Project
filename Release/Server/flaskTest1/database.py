import sqlite3 as sq
import os
import json
from datetime import datetime

class Database:

    def __init__(self, db_location, data_location, logger):
        self.db_location = db_location;
        self.data_location = data_location;
        self.logger = logger



        print("Database module loaded")


    def testDB(self):
        if os.path.exists(self.db_location):
        # load database from existing file
            return self.loadDB()
        else:
            print("No db found")
            a = input("create?(y/n):>")
            if a == 'y':
                # create the database at that location
                return self.createDB()
            else:
                return False

    def createDB(self):
        projects_query = '''CREATE TABLE projects (name TEXT,
                                            owner TEXT,
                                            created TEXT,
                                            head TEXT)'''
        with sq.connect(self.db_location) as conn:
            conn.execute(projects_query)

        print("created db")
        return

    def loadDB(self):
        self.readProjects()

        # print(self.projects)

        # print(self.files)

        
        return
    

    def checkProject(self, project_name):
        
        if project_name in self.tree:
            return True
        else:
            return False


    def checkFile(self, project_name, file_name):
        if (file_name in self.tree[project_name]):
            return self.tree[project_name][file_name]['version']
        else:
            return False



    def checkUpload(self, project_name, remote_dir, file_name):
        self.loadDB()
        project_exist = self.checkProject(project_name)
        file_exist = self.checkFile(project_name, file_name)
        print("Checking: ", project_name, project_exist)
        print("FileCheck: ", file_name, file_exist)

        if not (project_exist):
            return -1
        elif not (file_exist):
            return 0
        else:
            return file_exist

    
    def addFileEntry(self, filename, remoteDir, checksum, relations, user):
        db_name = self.data_location + remoteDir.split("/")[0] + "/data.db"
        with sq.connect(db_name) as conn:
            cursor = conn.cursor()

            date = self.getDate()
            file_path = remoteDir + filename

            query = '''INSERT INTO files VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)'''
            data = (filename, user, date, "1", relations, user, date, remoteDir, checksum,)

            conn.execute(query, data)
            conn.commit()

    def updateFileEntry(self, filename, remoteDir, checksum, user, relations, version):
        print("RELATINOS ARE ", relations)
        print("VERISON IS", version)
        db_name = self.data_location + remoteDir.split("/")[0] + "/data.db"
        with sq.connect(db_name) as conn:
            cursor = conn.cursor()

            modified = self.getDate()

            query = '''UPDATE files SET version = ?, relations = ?, last_user = ?, last_modified = ?, checksum = ? WHERE filename = ?'''
            qdata = (version, relations, user, modified, checksum, filename,)

            cursor.execute(query, qdata)
            conn.commit()

        return True


    def createSettings(self, project, input):
        path = self.data_location + project + '/' + 'settings.json'
        with open(path, 'w') as file:
            file.write(input)

    def createProjectEntry(self, name, owner, created, head):
        with sq.connect(self.db_location) as conn:
            cur = conn.cursor()

            query = '''INSERT INTO projects VALUES(?, ?, ?, ?)'''
            data = (name, owner, created, head,)
            cur.execute(query, data)
        print("done")

    def createProjectDB(self, project):
        path = self.data_location + project + '/' + 'data.db'

        with sq.connect(path) as conn:
            files_query = '''CREATE TABLE files (filename TEXT,
                                            owner TEXT,
                                            created TEXT,
                                            version TEXT,
                                            relations TEXT,
                                            last_user TEXT,
                                            last_modified TEXT,
                                            path TEXT,
                                            checksum TEXT)'''
            conn.execute(files_query)

    def createProject(self, json_data):
        # parse project data
        p_name = json_data['name']
        p_head = p_name
        p_owner = json_data['owner']

        # distill for settings
        del json_data['name']
        del json_data['owner']
        setting_str = json.dumps(json_data)

        # get the date
        p_created = self.getDate()


        self.createProjectEntry(p_name, p_owner, p_created, p_head)
        self.createProjectDB(p_name)
        self.createSettings(p_name, setting_str)
        return True

     
    def getDate(self):
        now = datetime.now()
        return now.strftime("%H:%M:%S|%m/%d/%Y")

    def listProjects(self):
        from pprint import pprint
        self.loadDB();
        json_data = []

        for project_name in self.tree:
            project_files = self.tree[project_name]

            p_data = {
                'name': project_name
            }
            p_data['files'] = {}

            for filename in project_files:
                p_data['files'][filename] = {}
                p_data['files'][filename]['relations'] = project_files[filename]["relations"]
                p_data['files'][filename]['version'] = project_files[filename]["version"]
                p_data['files'][filename]['path'] = project_files[filename]["path"].replace('/', '\\')

                # p_data['versions'][filename] = self.files[project_name][filename]
            json_data.append(p_data)


        print("JSON DATA LIST")
        
        pprint(json_data)
        return json_data


    def readProjects(self):
        with sq.connect(self.db_location) as conn:
            cur = conn.cursor()

            query = '''SELECT * FROM projects'''
            cur.execute(query)

            self.tree = {}

            for entry in cur.fetchall():
                items = list(entry)
                row = {
                    'name': items[0],
                    'owner': items[1],
                    'created': items[2],
                    'head': items[3]
                }
                self.tree[row['name']] = {}
                # self.projects[row['name']] = row;
                project_db = self.data_location + row['head'];

                # self.files[row['name']] = self.readFiles(project_db)
                self.readFiles(project_db, row['name'])


    def readFiles(self, project, pname):
        db_path = project + "/data.db"
        with sq.connect(db_path) as conn:
            cur = conn.cursor()

            query = '''SELECT * FROM files'''
            cur.execute(query)

            for entry in cur.fetchall():
                items = list(entry)

                row = {
                    'filename': items[0],
                    'owner': items[1],
                    'created': items[2],
                    'version': items[3],
                    'relations': items[4],
                    'last_user': items[5],
                    'last_modified': items[6],
                    'path': items[7],
                    'checksum': items[8]
                }
                self.tree[pname][row['filename']] = row


    def getLatestPath(self, project, name):
        path = self.data_location + self.tree[project][name]['path'] + "/"

        cName = name + "." + self.tree[project][name]['version']

        return (path + cName)
