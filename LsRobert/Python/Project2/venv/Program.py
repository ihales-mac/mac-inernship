import pyodbc as pyodbc

connetion = pyodbc.connect("Initial Catalog=PythonDB; DRIVER={SQL Server}; SERVER={DESKTOP-MS32J8R};Integrated Security=True")
cursor=connetion.cursor()


def SelectQueryOne():
    cursor.execute("SELECT [Nume] FROM PythonDB.[dbo].[User]")
    row = cursor.fetchone()
    if row:
        print(row)
    print("End of Query")

def SelectQueryAll():
    cursor.execute("select [Id],[Nume] FROM PythonDB.[dbo].[User]")
    rows = cursor.fetchall()
    for row in rows:
        print(row)
    print("End of Query")

def InsertQuery():
    cursor.execute("Insert into PythonDB.[dbo].[User] Values('2','Andrei','Tudorica')")
    connetion.commit()
    print('End of Query-Insert completed')


def SelectCountAll():
    cursor.execute("SELECT Count(*) FROM PythonDB.[dbo].[User]")
    row = cursor.fetchone()
    if row:
        number=str(row)
        print("There are " + number + " users")

if __name__ == "__main__":
    SelectCountAll()
    SelectQueryOne()
    SelectQueryAll()
    InsertQuery()
    SelectCountAll()