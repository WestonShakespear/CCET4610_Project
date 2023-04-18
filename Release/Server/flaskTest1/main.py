
from flask import Flask, render_template, redirect, request, render_template_string, jsonify, send_file

import json
import sys

# custom libraries
from logger import Log
from database import Database


from fileManager import FileManager

# custom variables
from user_defined import db_location, data_location, app_config, log_dir, log_prefix



# setup flask app
app = Flask(__name__)
for key in app_config:
    app.config[key] = app_config[key]

database_logger = Log(log_dir, log_prefix= '-DB').logger
db = Database(db_location, data_location, database_logger)

# setup file manager and logger
file_man_logger = Log(log_dir, log_prefix + '-FilMan').logger
fileMan = FileManager(file_man_logger, data_location)

@app.before_first_request
def startup():
    return



@app.route("/")
def index():
    return "testing"


@app.route("/test")
def template_test():
    return render_template("site/test.html")


@app.route("/api_test", methods=["POST"])
def api_test():
    """
    simple c# test to call python restful api web service
    """
    try:                
#         get request json object
        request_json = request.get_json()      
#         convert to response json object 
        response = jsonify(request_json)
        print(request_json)
        response.status_code = 200  
    except:
        exception_message = sys.exc_info()[1]
        response = json.dumps({"content":exception_message})
        response.status_code = 400
    return response


@app.route("/file_upload", methods=["POST"])
def file_upload():
    print("uploaddddd")
    request_data = request.get_json()
    
    json_data = json.loads(json.dumps(request_data))
    up_content = json_data["content"]
    up_checksum = json_data["checksum"]
    up_filepath = json_data["filePath"]
    up_filename = json_data["fileName"]
    up_project = json_data["project"]

    up_remotedir = json_data["remoteDir"]
    up_remotedir = up_remotedir.replace("\\", "/")

    up_user = json_data["user"]


    up_resource = json_data['resource']
    print("preview: ", up_resource)

    if not (fileMan.getStringChecksum(up_content.encode("utf-8")) == up_checksum):
        return "retry"

    

    if up_resource == "True":
        
        print("resource sent")
        if not fileMan.saveB64Resource(up_content, up_project + "/resources/", up_filename, overwrite=True):
                return "error saving resource"

        return "good"
    
    check = db.checkUpload(up_project, up_remotedir, up_filename)

    if check == -1:
        return "bad project"
    
    else:

        if check == 0:
            # new file
            print("new file")
            filename = up_filename + ".1"
            if not fileMan.saveB64(up_content, up_remotedir, filename):
                return "error saving new file"
            db.addFileEntry(up_filename, up_remotedir, up_checksum, up_user)

        else:
            # update file
            version = str(int(check) + 1)
            filename = up_filename + "." + version
            if not fileMan.saveB64(up_content, up_remotedir, filename):
                return "error saving updated file"

            print(db.updateFileEntry(up_filename, up_remotedir, up_checksum, up_user, version))
            print("update file", version)
            
        


    # if (fileMan.getStringChecksum(up_content.encode("utf-8")) == up_checksum):
    #     fileMan.saveB64(up_content, up_filepath, up_filename, up_remotedir)

    #     if (up_preview == "False"):
    #         # add entry to db
    #         db.addFileEntry(up_filename, up_remotedir, up_checksum, up_user)

    #     return "good"
    # else:
    return "good"
    
@app.route("/file_download", methods=["GET"])
def file_download():
    argsImmutable = request.args
    args = argsImmutable.to_dict()

    dw_filename = args["fileName"]
    dw_project = args["project"]

    print(dw_filename)
    print(dw_project)

    filePath = db.getLatestPath(dw_project, dw_filename)

    print("path: ", filePath)

    data = fileMan.getB64(filePath)


    return json.dumps(data)



@app.route("/preview/<project>/<file>")
def preview(project, file):
    print(project, file)

    filename = data_location + project + '/resources/' + file
    print(filename)
    return send_file(filename + ".BMP", mimetype='image/gif')





@app.route("/create_project", methods=["POST"])
def create_project():
    """
    simple c# test to call python restful api web service
    """
    try:                
        request_json = request.get_json()   
        print(request_json)   

        response_json = {
            'success': 'false'
        }

        if db.createProject(request_json):
            response_json['success'] = 'true'

        response = jsonify(response_json)

        response.status_code = 200  
    except:
        exception_message = sys.exc_info()[1]
        response = json.dumps({"content":exception_message})
        response.status_code = 400
    return response
    


@app.route("/list_projects", methods=["POST"])
def list_projects():
    try:                
        request_json = request.get_json()   
        print(request_json)   

        projects = db.listProjects()
        response_json = {
            'success': 'true',
            'data': projects
        }

        response = jsonify(response_json)

        response.status_code = 200  
    except:
        exception_message = sys.exc_info()[1]
        response = json.dumps({"content":exception_message})
        response.status_code = 400
    return response



if __name__ == "__main__":
    print("Main Entry")
    if db.testDB() == False:
        sys.exit(0)

    


    # start flask app
    app.run(host='0.0.0.0', port=5000)

    