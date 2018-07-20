import pyodbc
cnxn=pyodbc.connect("Initial Catalog=exercise1;Integrated Security=True; DRIVER={SQL Server}; SERVER={DESKTOP-M2I77J7}")
cursor = cnxn.cursor()

cursor.execute("SELECT [Username] ,[Email] FROM [exercise1].[dbo].[User]")
rows = cursor.fetchall()
for row in rows:
    print(row)