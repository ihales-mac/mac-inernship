/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

:r .\Post-Deployment\AspNetUsers.Table.sql
GO
:r .\Post-Deployment\AspNetRoles.Table.sql
GO
:r .\Post-Deployment\AspNetClaims.Table.sql
GO
:r .\Post-Deployment\AspNetDetails.Table.sql
GO
:r .\Post-Deployment\AspNetLogins.Table.sql
GO
:r .\Post-Deployment\AspNetUserDetails.Table.sql
GO
