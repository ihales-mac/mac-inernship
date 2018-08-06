import argparse
import os
import shutil
import sys
import zipfile


#getAll()-It is a generator that's looking for all the files and directories from 'root' and will exclude
#the files that have the prefix == 'exclude'.
def getAll(root,exclude):
    list=os.listdir(root)
    for item in list:
        path=os.path.join(root,item)
        if (os.path.isfile(path)):
            if(item[0] != exclude):
                yield (path)
        elif(os.path.isdir(path)):
            yield from getAll(path, exclude)

#Copies everything(havin the same structure) from 'rootSource' to 'rootDestination'
#without the files that have the prefix=='exclude'
def copyFromSourceToDestination(rootSource,rootDestination,exclude):
    list = getAll(rootSource,exclude)
    for item in list:
        listOfItem = item.split("\\")
        path = os.path.join(rootDestination, "\\".join(listOfItem[1:-1]))
        if not (os.path.isdir(path)):
            os.makedirs(path)
        shutil.copyfile(item, os.path.join(path, listOfItem[-1]))
    print("Everything was copied from " + rootSource +  " to " + rootDestination)

#make a zip from a folder('source') , with the name of the zip given in the first argument('name')
def makeZip(name,source,exclude):
    list=getAll(source,exclude)
    zip = zipfile.ZipFile(name, 'w', zipfile.ZIP_DEFLATED)
    for item in list:
        zip.write(item)
    zip.close();
    print("Zip was createad.")



if __name__ == "__main__":
    parser=argparse.ArgumentParser();
    parser.add_argument("copy_zip", help="Decide if you what to copy from Source to Destination or to Zip one file(write 'copy' or 'zip)'")
    parser.add_argument("Source", help="The source folder")
    parser.add_argument("Destination",help="The destination folder")
    parser.add_argument("Exclude",help="What kind of file to exclude")
    parser.add_argument("Zip_name")
    args=parser.parse_args()


    if(args.copy_zip == "copy"):
        copyFromSourceToDestination(args.Source,args.Destination,args.Exclude)
    else:
        makeZip(args.Zip_name,args.Source,args.Exclude)





