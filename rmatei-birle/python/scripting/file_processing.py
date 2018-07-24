import argparse
import sys
import zipfile
import os
import os.path
from shutil import copyfile



def get_files_recursively(initial_path, path, ignore):
    path_to_search_in = os.path.join(initial_path, path)
    for p in os.listdir(path_to_search_in):
        current = os.path.join(path_to_search_in, p)
        
        if os.path.isdir(current):
            yield from get_files_recursively(initial_path, os.path.join(path,p), ignore)
        
        if os.path.isfile(current):
            if not p.startswith(ignore):
                yield (path, p)

def copy_files(from_where, to_where, prefix_to_ignore):
    ignored = get_files_recursively(from_where, "", prefix_to_ignore)

    for (path, fl) in ignored:
        where_to = os.path.join(to_where, path)
        os.makedirs(where_to, exist_ok = True)
        copyfile(os.path.join(os.path.join(home, path), fl), os.path.join(where_to, fl))

def zip_files(from_where, prefix_to_ignore, zip_name):
    ignored = get_files_recursively(from_where, "", prefix_to_ignore)
    zipf = zipfile.ZipFile(os.path.join('{}.zip'.format(zip_name)), 'w', zipfile.ZIP_DEFLATED)

    for (path, fl) in ignored:
        what_to_write = os.path.join(from_where, os.path.join(path, fl))
        zipf.write(what_to_write)

    zipf.close()


parser = argparse.ArgumentParser(description = "Move and zip files.")
parser.add_argument('-s', '--source')
parser.add_argument('-d', '--dest')
parser.add_argument('-z', '--zname')

args = parser.parse_args()

destination = str(args.dest)
home = str(args.source)
zname = str(args.zname)

if not os.path.isdir(home):
    print ("Home path: {} is not a directory".format(home))
    sys.exit()

if not os.path.isdir(destination):
    print ("Destination path: {} is not a directory".format(destination))
    sys.exit()

copy_files(home, destination, "pre3")

zip_files(destination, "pre1", zname)



