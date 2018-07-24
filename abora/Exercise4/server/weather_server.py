from http.server import HTTPServer, BaseHTTPRequestHandler
import requests
from urllib.parse import urlparse
import json
from datetime import date, datetime
from pprint import pprint as pp


def filter_by_date(temps, today):
    '''
    Filter data such that I get weather information only for today.
    :param temps:
    :param today:
    :return:
    '''
    today_info = []
    for temp in temps:
        day_str = temp['dt_txt'].split()[0]
        day = datetime.strptime(day_str, '%Y-%m-%d').date()
        present = today
        if day == present:
            today_info.append(temp)
    return today_info


def get_weather(city):
    '''
    Get weather information for a city for today.
    The information is given from 3 to 3 hours
    :param city: Name of the city for which you want the information
    :return: list of weather information for the given city
    '''
    api = 'http://api.openweathermap.org/data/2.5/forecast'
    key = '39128e884986429587be6822242549ea'

    url = api + '?q=' + city + '&APPID=' + key
    print(url)
    r = requests.get(url).json()

    temps = r['list']
    filtered = filter_by_date(temps, date.today())
    pp(filtered)

    return filtered


class S(BaseHTTPRequestHandler):
    '''
        Handler class
    '''

    def _set_headers(self):
        self.send_response(200)
        self.send_header('Content-type', 'application/json')
        self.end_headers()

    def do_GET(self):
        self._set_headers()
        parsed_path = urlparse(self.path)
        city = parsed_path.path[1:]
        print('city = {}'.format(city))
        response = get_weather(city)
        self.wfile.write(json.dumps(response).encode())


def run(server_class=HTTPServer, handler_class=S, port=8080):
    server_address = ('127.0.0.1', port)
    httpd = server_class(server_address, handler_class)
    print('Starting httpd...')

    try:
        httpd.serve_forever()
    except KeyboardInterrupt:
        print('^C pressed')
        httpd.server_close()


if __name__ == "__main__":
    '''
    1.Start server python weather_server.py
    2.Make an GET api call to http://127.0.0.1:8080/{city}
    to get weather information
    '''
    from sys import argv

    if len(argv) == 2:
        run(port=int(argv[1]))
    else:
        run()
