import pydoc
# version 1
# cnxn = pyodbc.connect('DRIVER=SQL SERVER;SERVER=DESKTOP-QLSBE2K\SQLEXPRESS;Initial Catalog=dbo.detistry;Integrated Security=True')
# version 2
cnxn = pydoc.connect("Trusted_Connection=yes", driver="{SQL Server}", server="DESKTOP-QLSBE2K\SQLEXPRESS",
                      database="dentistry")

cursor = cnxn.cursor()
cursor.execute("select * from dentistry.dbo.Dentist")
rows = cursor.fetchall()
for row in rows:
    print("{} - {}".format(row.ssn, row.name))
