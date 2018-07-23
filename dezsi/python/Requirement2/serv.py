import datetime
from http.server import HTTPServer, BaseHTTPRequestHandler
import urllib.request
from urllib.parse import urlparse
import json


class Serv(BaseHTTPRequestHandler):

    @staticmethod
    def get_date():
        now = datetime.datetime.now()
        return now.year, now.month, now.day

    @staticmethod
    def get_location(name):
        with urllib.request.urlopen("https://www.metaweather.com/api/location/search/?query="+name) as url:
            data = json.loads(url.read().decode())
        return data[0]["woeid"]

    def get_param(self, param):
        query = urlparse(self.path).query
        query_components = dict(qc.split("=") for qc in query.split("&"))
        return query_components[param]

    def do_GET(self):

        param = self.get_param("location")
        try:
            with urllib.request.urlopen("https://www.metaweather.com/api/location/{}/{}/{}/{}/".format(self.get_location(param), self.get_date()[0],
                                                                                                       self.get_date()[1], self.get_date()[2]
                                                                                                       )) as url:
                data = json.loads(url.read().decode())
            char = self.path.find('?')

            #filter data
            for i in range(0, len(data)):
                data[i].pop("id")
                data[i].pop("created")
                data[i].pop("weather_state_abbr")
                data[i].pop("applicable_date")
                data[i].pop("wind_direction_compass")


            open(self.path[1:char], 'w').write("<html><head><title>{}</title></head><body>{}</body></html>"
                                               .format(param.capitalize()+" ("+str(self.get_date()[0])+"/"+str(self.get_date()[1])
                                                       +"/"+str(self.get_date()[2])+")", data))
            file_to_open = open(self.path[1:char]).read()
            self.send_response(200)

        except Exception as e:
            print(e)
            file_to_open = '<html style="background: repeating-linear-gradient' \
                           '(#c6e5f2, #c6e5f2 20px, #9198e5 20px, #9198e5 25px);">' \
                           '<head><title>404</title></head><body><div style= "border-radius: 360px; background: ' \
                           'radial-gradient(white, #24179b);position:relative; margin-left:600px; ' \
                           'margin-top:200px;height:150px; width:400px; border:4px solid black;">' \
                           '<h1 style="text-align: center;color:#000080">404</h1>' \
                           '<h2 style="color:#1725e8;text-align: center;">File Not Found</h2></div></body></html>'
            self.send_response(404)

        self.end_headers()
        self.wfile.write(bytes(file_to_open, 'utf-8'))


if __name__ == '__main__':
    httpd = HTTPServer(('localhost', 8080), Serv)
    httpd.serve_forever()
