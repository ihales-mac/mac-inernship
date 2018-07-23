import argparse
import os
import shutil

from zipfile import ZipFile

parser = argparse.ArgumentParser(description="zip files with the same prefix recursively")
parser.add_argument('to_exclude', type=str, help='The type to be excluded')
parser.add_argument('home', type=str, help='Home of folders')
parser.add_argument('dest', type=str, help='Destination of zips')
parser.add_argument('zipname', type=str, help='Name of the zips')
args = parser.parse_args()


def get_files(prefix:str, home:str, current:str, dest:str, action):

    for e in os.listdir(current):
        curr = os.path.join(home,current,e)
        if os.path.isdir(curr):
            destination = curr.replace(home,dest)
            yield from get_files(prefix, home,curr, dest, action)

        elif os.path.isfile(curr) and ((action == "exclude" and not(e.startswith(prefix))) or
                                     ((action =="include_only") and e.startswith(prefix))):

            destination = curr.replace(home, dest)
            yield destination


def get_wrapper(prefix, home, dest, action):
    return get_files(prefix,home,home,dest,action)


def move_files(prefix, home, dest):
    for m in get_wrapper(prefix,home, dest, "exclude"):
        os.makedirs(os.path.dirname(m), exist_ok=True)
        src = m.replace(dest, home)
        shutil.copyfile(src,m)


def create_zips(prefix):
    with ZipFile(prefix+args.zipname+'.zip', 'w') as zf:
        for m in get_wrapper(prefix, args.home, args.dest, "include_only"):
            zf.write(m)


if __name__ == "__main__":
    move_files(args.to_exclude, args.home, args.dest)
    for prefix in ["b_", "c_"]:
        create_zips(prefix)





