import pyodbc

cnxn   = pyodbc.connect("Initial Catalog=LoginExample;Integrated Security=True; DRIVER={SQL Server}; SERVER={DESKTOP-BRM0244}")
cursor = cnxn.cursor()

cursor.execute("Select [User].[FirstName], [User].[LastName], [PersonalDetails].[PhoneNumber] from [LoginExample].[dbo].[User] INNER JOIN [LoginExample].[dbo].[PersonalDetails] on [User].[Details] = [PersonalDetails].[ID]")
for p in cursor:
    print(p)
    

print("")
print("")
print("")

cursor.execute("Select [User].[FirstName], [User].[LastName], [Permission].[Name] as [Permissions] from [LoginExample].[dbo].[User] INNER JOIN [LoginExample].[dbo].[UserHasPermission] on [User].[ID] = [UserHasPermission].[UserID] INNER JOIN [LoginExample].[dbo].[Permission] on [UserHasPermission].[PermissionID] = [Permission].[ID]")
for p in cursor:
    print(p)


print("")
print("")
print("")