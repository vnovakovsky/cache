SELECT TOP (1000) [UserID]
      ,[NationalIDNumber]
      ,[LoginID]
      --,[OrganizationNode]
      ,[OrganizationLevel]
      ,[JobTitle]
      ,[BirthDate]
      ,[MaritalStatus]
      ,[Gender]
      ,[HireDate]
      ,[VacationHours]
      ,[SickLeaveHours]
   
  FROM [AdventureWorks].[Cache].[Employee]
  WHERE UserID = 42
  order by UserId desc


  SELECT * FROM sys.dm_db_index_physical_stats (DB_ID(N'AdventureWorks'), OBJECT_ID(N'[AdventureWorks].[HumanResources].[Employee]'), NULL, NULL , 'DETAILED')

  SELECT * FROM sys.dm_db_index_physical_stats (DB_ID(N'AdventureWorks'), OBJECT_ID(N'[AdventureWorks].[Cache].[Employee]'), NULL, NULL , 'DETAILED')

  ---------------

  declare @table nvarchar(128)
declare @idcol nvarchar(128)
declare @sql nvarchar(max)

--initialize those two values
set @table = '[AdventureWorks].[Person].[Person]'
set @idcol = '1'

set @sql = 'select ' + @idcol +' , (0'

select @sql = @sql + ' + isnull(datalength(' + name + '), 1)' 
        from  sys.columns 
        where object_id = object_id(@table)
        and   is_computed = 0
set @sql = @sql + ') as rowsize from ' + @table + ' order by rowsize desc'

PRINT @sql

exec (@sql)

  ---------------

SELECT *
INTO [AdventureWorks].[Cache].Employee
FROM [AdventureWorks].[HumanResources].[Employee]


exec sp_executesql N'SELECT TOP(5) [UserID]
                        ,[NationalIDNumber]
                        ,[LoginID]
                        ,[OrganizationLevel]
                        ,[JobTitle]
                        ,[BirthDate]
                        ,[MaritalStatus]
                        ,[Gender]
                        ,[HireDate]
                        ,[VacationHours] 
                        ,[SickLeaveHours]

                    FROM[AdventureWorks].[Cache].[Employee] WHERE [UserID] >= @userId',N'@userId int',@userId=42


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create procedure insert_new_employee(
				@UserID int,
				@NationalIDNumber nchar(15),
				@LoginID nvarchar(256),
				@OrganizationNode hierarchyid,
				@OrganizationLevel smallint,
				@JobTitle nchar(50),
				@BirthDate date,
				@MaritalStatus nchar(1),
				@Gender nchar(1),
				@HireDate date,
				@VacationHours smallint,
				@SickLeaveHours smallint,
				@ModifiedDate datetime)
as

insert into AdventureWorks.Cache.Employee values(@UserID,
				@NationalIDNumber,
				@LoginID,
				@OrganizationNode,
				@OrganizationLevel,
				@JobTitle,
				@BirthDate,
				@MaritalStatus,
				@Gender,
				@HireDate,
				@VacationHours,
				@SickLeaveHours,
				@ModifiedDate)

GO



USE [AdventureWorks]
GO

DECLARE	@return_value int

EXEC	@return_value = [dbo].[update_new_employee]
		@UserID = 999,
		@NationalIDNumber = N'12345',
		@LoginID = N'test',
		@OrganizationNode = 0x,
		@OrganizationLevel = 0,
		@JobTitle = N'testTitle',
		@BirthDate = '1963-03-02',
		@MaritalStatus = N'M',
		@Gender = N'M',
		@HireDate = '1963-03-02',
		@VacationHours = 100,
		@SickLeaveHours = 333

SELECT	'Return Value' = @return_value

GO

