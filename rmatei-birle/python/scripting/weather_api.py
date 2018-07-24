#api call: http://api.openweathermap.org/data/2.5/forecast?id=524901&APPID={APIKEY}

import requests as req
import json
import http.server as httpserver
from http.server import BaseHTTPRequestHandler
import socketserver
from json2html import *
import urllib

class WeatherHandler(BaseHTTPRequestHandler):
    _api_key = "45530cecc46160e09c605fbd1b4006a5"
    _location = "cluj-napoca,ro"

    def format_response(self):
        response = req.get("http://api.openweathermap.org/data/2.5/forecast?q={}&APPID={}".format(_location, _api_key))
        response = response.json()

        composing_response = response["list"]
        composing_response = composing_response[0:9]

        response = []


        for item in composing_response:
            d={}
            d["Time"] = item["dt_txt"]

            p={}
            p["Average Temperature"] = '%.2f deg C' % (item["main"]["temp"] - 274.15)
            p["Maximum Temperature"] = '%.2f deg C' % (item["main"]["temp_max"] - 274.15)
            p["Minimum Temperature"] = '%.2f deg C' % (item["main"]["temp_min"] - 274.15)
            p["Humidity"] = str(item["main"]["humidity"]) + "%"
            p["Pressure"] = item["main"]["pressure"]
            p["Downfall"] = item["weather"][0]["description"]
            d["Main"] = p
            
            d["Wind"] = item["wind"]
            response.append(d)
        return response


    def do_GET(self):
        self.send_response(200)
        self.send_header("Content-type","text/html")
        self.end_headers()
        
        s = self.path
        print(s)

        if(s == "/favicon.ico"):
            return

        loc = urllib.parse.parse_qs(s[2:])
        
        if len(loc.keys()) != 0:
            try:
                _location = loc["city"][0] + "," + loc["country"][0]
                print(_location)
            except: 
                _location = "cluj-napoca,ro"
        else:
                _location = "cluj-napoca,ro"


        self.wfile.write(bytes("<h1>Weather for the next 24 hours in {}!</h1><br><br>".format(_location), "utf-8"))
        _bytes_to_write = bytes(json2html.convert(json=response), "utf-8")
        self.wfile.write(_bytes_to_write)
        print("sent response")
        return

def main():
    try:
        httpd = socketserver.TCPServer(("", 80), WeatherHandler)
        httpd.serve_forever()
    except KeyboardInterrupt:
        httpd.socket.close()


if __name__ == '__main__':
    main()