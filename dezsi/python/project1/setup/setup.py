import os

def make_folders():
    os.chdir('C:/Users/Intern/PycharmProjects/project1/folders')
    os.mkdir('folder1')
    os.mkdir('folder1/folder11')
    os.mkdir('folder1/folder11/folder111')
    os.mkdir('folder1/folder12')
    os.mkdir('folder2')
    os.mkdir('folder2/folder21')
    open('a_file1.txt', 'a').close()
    open('folder1/a_file2.txt', 'a').close()
    open('folder1/a_file3.txt', 'a').close()
    open('folder1/folder11/a_file4.txt', 'a').close()
    open('folder2/a_file5.txt', 'a').close()
    open('folder2/folder21/a_file6.txt', 'a').close()

    open('folder1/folder12/b_file1.txt', 'a').close()
    open('folder2/b_file2.txt', 'a').close()
    open('folder2/folder21/b_file3.txt', 'a').close()

    with open('c_file1.txt', 'a') as f:
        f.write("texttexttext")

    open('folder1/c_file2.txt', 'a').close()
    open('folder1/folder11/c_file3.txt', 'a').close()
    open('folder1/folder11/folder111/c_file4.txt', 'a').close()
    open('folder2/folder21/c_file5.txt', 'a').close()


if __name__ == "__main__":
    make_folders()
