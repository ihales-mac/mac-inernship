import argparse
import os, shutil
from shutil import copytree, ignore_patterns
import zipfile

'''
1.Script moves a folder structure from a source directory to a destination directory
2.Script create a zip file with the files starting with a given letter
'''

parser = argparse.ArgumentParser()

parser.add_argument('action', help='action to be performed( copy or make_zip )',
                    type=str)
parser.add_argument('source', help='Absolute path to source directory'
                    , type=str)
parser.add_argument('destination', help='Absolute path to destination directory'
                    , type=str)
parser.add_argument('zip_name', help='The name of the zip archive'
                    , type=str)
parser.add_argument('prefix',
                    help='for copy exclude file starting with prefix, for make_zip include all the file that start with prefix'
                    , type=str)
args = parser.parse_args()

action = args.action
# Source folder
root_src_dir = args.source
# Destination folder
root_dst_dir = args.destination
# Archive name
zip_name = args.zip_name
# File prefix
prefix = args.prefix

# Version 1
# without generators(only walk)
'''
for src_dir, dirs, files in os.walk(root_src_dir):
    dst_dir = src_dir.replace(root_src_dir, root_dst_dir, 1)
    if not os.path.exists(dst_dir):
        os.makedirs(dst_dir)
    for file_ in files:
        if file_.startswith(prefix):
            continue
        src_file = os.path.join(src_dir, file_)
        dst_file = os.path.join(dst_dir, file_)
        if os.path.exists(dst_file):
            os.remove(dst_file)
        shutil.copy(src_file, dst_dir)

'''

# Version 2
# with generator and walk
'''
def get_file():
    for src_dir, dirs, files in os.walk(root_src_dir):

        dst_dir = src_dir.replace(root_src_dir,root_dst_dir,1)
        if not os.path.exists(dst_dir):
            os.makedirs(dst_dir)
        for file in files:
            if file.startswith(prefix):
                continue
            yield (os.path.join(src_dir, file)),(os.path.join(dst_dir, file))

for src,dest in get_file():
    shutil.copy(src,dest)

'''
# Version 3
# with copy tree
'''
copytree(root_src_dir,root_dst_dir,ignore=ignore_patterns('{}*'.format(prefix)))
'''


# Version 4
# with 2 generators

def dfs(source_path):
    '''
    Copy a folder structure by traversing the tree structure using a dfs algorithm
    and ignore all the file that start with a prefix given as command line argument
    :param source_path: The root directory from where the file will be copied
    :return: the aboslute path of the source file and the absolute path of the destination path
    '''
    for entry in os.listdir(source_path):
        if entry.startswith(prefix):
            continue
        dest_dir = source_path.replace(root_src_dir, root_dst_dir, 1)
        source_path = os.path.join(source_path, entry)
        dest_path = os.path.join(dest_dir, entry)

        if os.path.isfile(source_path):
            yield (source_path, dest_path)

        elif os.path.isdir(source_path):
            if not os.path.exists(dest_path):
                os.makedirs(dest_path)
            yield from dfs(source_path)

        lst = source_path.split("\\")
        source_path = "\\".join(lst[:-1])


def tree_cpy():
    for path in dfs(root_src_dir):
        shutil.copy(path[0], path[1])


# Make zip function

def get_files():
    '''
    Return all the file the start with a given prefix and an archive name of that file
    which is computed as a relative path to the source directory

    :return: Yield tuple containing absolute path of a file and the archive name of the file
    '''
    for dir_path, dirs, files in os.walk(root_src_dir):
        for file in files:
            if not file.startswith(prefix):
                continue
            arcname = os.path.relpath(dir_path, root_src_dir)
            yield (os.path.join(dir_path, file), os.path.join(arcname, file))


def make_zip():
    zip = zipfile.ZipFile(zip_name, 'w', zipfile.ZIP_DEFLATED)
    for file in get_files():
        zip.write(file[0], file[1])
    zip.close()


if __name__ == '__main__':
    if action == 'copy':
        tree_cpy()
    elif action == 'make_zip':
        make_zip()
