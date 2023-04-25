import hashlib
import base64
import os

class FileManager:

    def __init__(self, log, data_path):
        self.log = log
        self.data_path = data_path
        pass

    def getStringChecksum(self, input_data):
        checksum = hashlib.md5(input_data).hexdigest().upper()

        return checksum

    def decodeString(self, input_data):
        encoded = input_data.encode("utf-8")
        out_bytes = base64.b64decode(encoded)

        return out_bytes

    def buildSavePath(self, rmDir, name):
        return self.data_path + rmDir + name

    def createNeededDirs(self, path):
        root_path_len = len(self.data_path.split("/")) - 1;
        print(root_path_len);

        path_structure = path.split("/")
        current_loc = ""

        for i, folder in enumerate(path_structure):
            
            
            # skip over data directory
            if i < root_path_len:
                current_loc += folder + "/"
                continue
            elif i == len(path_structure) - 1:
                break
            print(folder, current_loc)

            print(os.listdir(current_loc))
            if folder not in os.listdir(current_loc):
                os.makedirs(current_loc + folder)

            current_loc += folder + "/"


    def createProject(self, p_name):
        if not os.path.exists(self.data_path + p_name):
            os.makedirs(self.data_path + p_name)
            os.makedirs(self.data_path + p_name + "/resources")
            return True
        else:
            return False 
    
    def saveB64(self, input_data, path, name, overwrite=False):
        

        # decode the data
        out_bytes = self.decodeString(input_data)
        save_path = self.buildSavePath(path + "/", name)

        self.log.debug("Attempting to save " + name + " in " + save_path)

        if (os.path.exists(save_path) and (not overwrite)):
            self.log.error(name + " already exists");
        elif not(os.path.exists(self.data_path + path.split("/")[0])):
            print("Project doesn't exist");
        else:
            print()
            self.createNeededDirs(save_path)
            
            try:
                with open(save_path, "wb") as file:
                    file.write(out_bytes)
                self.log.debug("Saving " + path + " [SUCCESS]")
                return True

            except Exception as e:
                self.log.debug("Saving " + path + " [ERROR]" + e)
        return False
    
    def saveB64Resource(self, input_data, path, name, overwrite=False):
        

        # decode the data
        out_bytes = self.decodeString(input_data)
        save_path = self.buildSavePath(path + "/", name)

    
            
        try:
            with open(save_path, "wb") as file:
                file.write(out_bytes)
            self.log.debug("Saving " + path + " [SUCCESS]")
            return True

        except Exception as e:
            self.log.debug("Saving " + path + " [ERROR]" + e)
        return False
    
    


    def getB64(self, path):

        if (os.path.exists(path)):
            with open(path, "rb") as file:
                data_bytes = file.read()

            dataB64 = base64.b64encode(data_bytes)
            checksum = self.getStringChecksum(dataB64)
            print(checksum)

            data = {
                "content": dataB64.decode(),
                "checksum": checksum
            }
            return data
                
        else:
            return "null"

