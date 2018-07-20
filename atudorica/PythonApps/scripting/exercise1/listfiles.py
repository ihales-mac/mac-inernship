import os
import zipfile
import sys
from shutil import copyfile
def listPathsToEachFile():
    for root, _ , filenames in os.walk('.\\folder1'):
        for filename in filenames: 
            if not filename.startswith('c'):
                yield( filename, root)

def listPathsCustom(root,prefix,include):
    """Generator for files from a file tree with the root in the root argument
        the generator includes or excludes the prefix given as the second argument based on the 
        boolean value of the third argument """
    for p in os.listdir(root):
        if(os.path.isfile(os.path.join(root,p))):
            if (p.startswith(prefix)==include):  
                yield (p,root)
        if(os.path.isdir(os.path.join(root,p))):  
            yield from listPathsCustom(os.path.join(root,p),prefix,include)

def copyFileTreeExcludePrefix(root1,root2,prefix):
    """Copies the entire file tree from root1 (first argument)
        to root2 (second argument), excluding filest starting with a certain prefix """
    m=listPathsCustom(root1,prefix,0)
    for p in m:
        pathToCheck=root2+'\\'+'\\'.join(p[1].split('\\')[2:])
        if not(os.path.isdir(pathToCheck)):
            os.makedirs(pathToCheck)
            print('created '+pathToCheck)
        copyfile(os.path.join(p[1],p[0]),os.path.join(pathToCheck,p[0]))
    print('copied files from {} to {}'.format(root1,root2))

def  makeZipFromFileTreeOnlyOnePrefix(root1,zipname,prefixToInclude):
    """makes a zip file with the entire file tree in root1 (first argument)
        with the name given in the second argument (include extension in name!).
        It includes only the files with the names starting with the prefix given in the third argument"""
    m=listPathsCustom(root1,prefixToInclude,1)
    zipf=zipfile.ZipFile(zipname,'w',zipfile.ZIP_DEFLATED)
    for p in m:
        zipf.write(os.path.join(p[1],p[0]))
    zipf.close()


def main(mode,source,dest,prefix):
    """
    Args:
        mode: copy or zip
        source: root of source folder for copying or zipping
        dest: root for copy, zipname (with extension) for zip
        prefix: prefix to exclude from copy or include in zip"""
    if(mode=='copy'):
        copyFileTreeExcludePrefix(source,dest,prefix)
    if(mode=='zip'):
        makeZipFromFileTreeOnlyOnePrefix(source,dest,prefix)


if(__name__=='__main__'):
    print(sys.argv[1],sys.argv[2],sys.argv[3],sys.argv[4])
    main(sys.argv[1],sys.argv[2],sys.argv[3],sys.argv[4])