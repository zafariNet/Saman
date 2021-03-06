/*
   يكشنبه، 02 فوريه 201405:42:22 ب.ظ
   User: 
   Server: PROGRAMMER
   Database: Saman
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE Emp.ToDoResult
	DROP CONSTRAINT FK_ToDoResult_ToDo
GO
ALTER TABLE Emp.ToDo SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE Emp.Tmp_ToDoResult
	(
	ToDoResultID uniqueidentifier NOT NULL,
	ReferedEmployeeID uniqueidentifier NULL,
	ToDoID uniqueidentifier NOT NULL,
	ToDoResultDescription nvarchar(500) NULL,
	RemindeTime nvarchar(10) NULL,
	SecondaryClosed bit NULL,
	Remindable bit NULL,
	EmployeeID uniqueidentifier NOT NULL,
	CreateDate nvarchar(19) NOT NULL,
	ModifiedEmployeeID uniqueidentifier NULL,
	ModifiedDate nvarchar(19) NULL,
	RowVersion int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE Emp.Tmp_ToDoResult SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM Emp.ToDoResult)
	 EXEC('INSERT INTO Emp.Tmp_ToDoResult (ToDoResultID, ReferedEmployeeID, ToDoID, ToDoResultDescription, RemindeTime, SecondaryClosed, Remindable, EmployeeID, CreateDate, ModifiedEmployeeID, ModifiedDate, RowVersion)
		SELECT ToDoResultID, ReferedEmployeeID, ToDoID, ToDoResultDescription, RemindeTime, SecondaryClosed, Remindable, EmployeeID, CreateDate, ModifiedEmployeeID, ModifiedDate, RowVersion FROM Emp.ToDoResult WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE Emp.ToDoResult
GO
EXECUTE sp_rename N'Emp.Tmp_ToDoResult', N'ToDoResult', 'OBJECT' 
GO
ALTER TABLE Emp.ToDoResult ADD CONSTRAINT
	FK_ToDoResult_ToDo FOREIGN KEY
	(
	ToDoID
	) REFERENCES Emp.ToDo
	(
	ToDoID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
