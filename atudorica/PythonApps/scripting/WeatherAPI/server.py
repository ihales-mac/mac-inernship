
#API key ff68b4d00d8a0ce2bbde72abeeb6afcd
import requests
import json
from io import StringIO
from http.server import BaseHTTPRequestHandler,HTTPServer
from json2html import *
from os import curdir, sep
from urllib.parse import *

PORT_NUMBER = 8080


class myHandler(BaseHTTPRequestHandler):
    def getDataForLocation(self,location):
        return requests.get("http://api.openweathermap.org/data/2.5/forecast?q="+location+"&APPID=ff68b4d00d8a0ce2bbde72abeeb6afcd")

    def getValuableContent(self,data):
        justMeteo=data['list']
        justMeteo=justMeteo[0:9]
        result = []
        for s in justMeteo :
            print(s)
            s1=s['main']
            s2=s['weather']
            print(s2[0])
            s4=s['wind']
            x={}
            x['General']={"temperature": s1['temp'], "minimal temperature":s1['temp_min'], "maximal temperature":s1['temp_max'], "wind speed": s4['speed'],  "pressure":s1['pressure'], "humidity":s1['humidity']}
            x["Weather"]={"name": s2[0]['main'],  "description": s2[0]['description']}
            x["Date And Time"]=s['dt_txt']
            print(x)
            result.append(x)
        print(result)
        return result

    def do_GET(self):
        try:
            if (self.path.endswith("favicon.ico")):
                return
            self.send_response(200)
            self.send_header('Content-type','text/html')
            self.end_headers()    
            print(self.path)
            
            if(self.path.endswith(".css") or self.path.endswith(".ico")):
                f = open(curdir + sep + self.path)
                self.wfile.write(bytes(f.read(),'utf-8'))
                f.close()
                return
            self.send_response(200)
            self.send_header('Content-type','text/html')
            self.end_headers()
            query = urlparse(self.path).query
            query_components = dict(qc.split("=") for qc in query.split("&"))
            location = query_components["location"]
            print(query_components)

            response = self.getDataForLocation(location)
            parsed=response.json()
            output = self.getValuableContent(parsed)
            
            print("data printed in browser")
            #./Table_Fixed_Header/Table_Fixed_Header/css/main.css
            rsp = StringIO()
            rsp.write("""<!doctype html>
                            <html>
                            <head>
                                <meta charset="UTF-8">
                                <meta name="viewport" content="width=device-width, initial-scale=1">
                            <!--===============================================================================================-->	
                                <link rel="icon" type="image/png" href="favicon.ico"/>
                            <!--===============================================================================================-->
                                <link rel="stylesheet" type="text/css" href="vendor/bootstrap/css/bootstrap.min.css">
                            <!--===============================================================================================-->
                                <link rel="stylesheet" type="text/css" href="fonts/font-awesome-4.7.0/css/font-awesome.min.css">
                            <!--===============================================================================================-->
                                <link rel="stylesheet" type="text/css" href="vendor/animate/animate.css">
                            <!--===============================================================================================-->
                                <link rel="stylesheet" type="text/css" href="vendor/select2/select2.min.css">
                            <!--===============================================================================================-->
                                <link rel="stylesheet" type="text/css" href="vendor/perfect-scrollbar/perfect-scrollbar.css">
                            <!--===============================================================================================-->
                                <link rel="stylesheet" type="text/css" href="css/util.css">
                                <link rel="stylesheet" type="text/css" href="css/main.css">
                            <!--===============================================================================================-->
                            """)
            rsp.write(json2html.convert(json=output, table_attributes="class=\"table table-bordered table-hover\""))
            rsp.write('</body></html>')
            self.wfile.write(bytes(rsp.getvalue(),'utf-8'))
            return
        except IOError:
            self.send_error(404,'File Not Found: %s' % self.path)
            return

try:
	server = HTTPServer(('', PORT_NUMBER), myHandler)
	print ('Started http server on port ' , PORT_NUMBER)
	server.serve_forever()

except KeyboardInterrupt:
	print ('^C received, shutting down the web server')
	server.socket.close()