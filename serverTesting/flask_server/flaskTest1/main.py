
from flask import Flask, render_template, redirect, request, render_template_string, jsonify

import json
import os
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

db = Database()

# setup file manager and logger
file_man_logger = Log(log_dir, log_prefix + '-FilMan').logger
fileMan = FileManager(file_man_logger, data_location)

@app.before_first_request
def startup():
    return
    if os.path.exists(db_location):
        # load database from existing file
        db.loadDB(db_location)
    else:
        print("No db found")
        a = input("create?(y/n):>")
        if a == 'y':
            # create the database at that location
            db.createDB(db_location)
        else:
            sys.exit(0)



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
        response.status_code = 200  
    except:
        exception_message = sys.exc_info()[1]
        response = json.dumps({"content":exception_message})
        response.status_code = 400
    return response


@app.route("/file_upload", methods=["POST"])
def file_upload():
    request_data = request.get_json()
    
    json_data = json.loads(json.dumps(request_data))
    up_content = json_data["content"]
    up_checksum = json_data["checksum"]
    up_filepath = json_data["filePath"]
    up_filename = json_data["fileName"]
    up_remotedir = json_data["remoteDir"]


    if (fileMan.getStringChecksum(up_content.encode("utf-8")) == up_checksum):
        fileMan.saveB64(up_content, up_filepath, up_filename, up_remotedir)


        return "good"
    else:
        return "bad"
    
@app.route("/file_download", methods=["GET"])
def file_download():
    argsImmutable = request.args
    args = argsImmutable.to_dict()
    
    dw_filepath = args["filePath"]
    dw_filename = args["fileName"]
    dw_remotedir = args["remoteDir"]

    data = fileMan.getB64(dw_filepath, dw_filename, dw_remotedir)


    return json.dumps(data)
    



if __name__ == "__main__":
    print("Main Entry")

    


    # start flask app
    app.run(host='0.0.0.0', port=5000)

    