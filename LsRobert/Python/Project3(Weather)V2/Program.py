import sys
from urllib.parse import urlparse
from json2html import *
import requests
from pprint import pprint
from datetime import date, timedelta
from http.server import HTTPServer, BaseHTTPRequestHandler


class Server(BaseHTTPRequestHandler):

    def getResult(self):
        location = str(urlparse(self.path).query)
        url = 'http://api.openweathermap.org/data/2.5/forecast?q={},uk&appid=e8c60ac3f6afbd80af5e1050373af45f&units=metric'.format(location)

        res = requests.get(url).json()

        data = ""
        for i in range(0, 9):
            data = data + res["list"][i]["dt_txt"]
            data = data + str(res["list"][i]) + "</br></br>"
        return data

    def do_GET(self):
        try:
            data= self.getResult();

            file_to_send = "<html><head></head><title>Vreme</title></head><body>{}</body></html>".format(data)
            self.send_response(200)
            self.end_headers()
            self.wfile.write(bytes(file_to_send, 'utf-8'))

            if (self.path[1:] == "style.css"):
                file_to_open = open("style.css").read()
                self.send_response(200)
                self.end_headers()
                self.wfile.write(bytes(file_to_open, 'utf-8'))
                return

            if(self.path[2:] == 'favicon.ico'):
               return

        except Exception as e:
            file_to_send = "<html><head></head><title>Error</title></head><body><h1>The city is wrong</h1></body></html>"
            self.send_response(400)
            self.end_headers()
            self.wfile.write(bytes(file_to_send, 'utf-8'))



if __name__ == "__main__":
    try:
        address = ("127.0.0.1", 8080)
        httpd = HTTPServer(address, Server)
        httpd.serve_forever()
    except KeyboardInterrupt:
        print("Bye")
