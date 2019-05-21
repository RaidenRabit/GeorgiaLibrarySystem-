USE master
go
IF DB_ID('GTL') IS NOT NULL
ALTER DATABASE GTL SET SINGLE_USER WITH ROLLBACK IMMEDIATE
DROP DATABASE IF EXISTS GTL;
GO

CREATE DATABASE GTL;
Go

use GTL;
GO

--Creates

CREATE TABLE Location (
    PostalCode int PRIMARY KEY,
    City varchar(100) NOT NULL,
);

CREATE TABLE Address (
    AddressID int PRIMARY KEY IDENTITY,
	PostalCode int FOREIGN KEY REFERENCES Location(PostalCode),
    Street varchar(100) NOT NULL,
    Number int NOT NULL
);

CREATE TABLE Library (
    LibraryName varchar(50) PRIMARY KEY,
    AddressID int FOREIGN KEY REFERENCES Address(AddressID)
);

CREATE TABLE Person (
    SSN int PRIMARY KEY,
    AddressID int FOREIGN KEY REFERENCES Address(AddressID),
	CampusID int FOREIGN KEY REFERENCES Address(AddressID),
	Phone int NOT NULL,
	Password varchar(16) NOT NULL
);

CREATE TABLE LibrarianType (
	TypeName varchar(50) PRIMARY KEY
);

CREATE TABLE Librarian (
    SSN int PRIMARY KEY FOREIGN KEY REFERENCES Person(SSN),
	TypeName varchar(50) FOREIGN KEY REFERENCES LibrarianType(TypeName)
);

CREATE TABLE MemberCard (
    MemberCardID int PRIMARY KEY IDENTITY,
	Photo varbinary(max) NOT NULL,
	ExpirationDate datetime NOT NULL
);

CREATE TABLE MemberType (
	TypeName varchar(50) PRIMARY KEY,
	LendingLenght int NOT NULL,
	GracePeriod int NOT NULL,
	NrOfBooks int NOT NULL
);

CREATE TABLE Member (
    SSN int PRIMARY KEY FOREIGN KEY REFERENCES Person(SSN),
	MemberCardID int FOREIGN KEY REFERENCES MemberCard(MemberCardID),
	TypeName varchar(50) FOREIGN KEY REFERENCES MemberType(TypeName),
	OnboardDate timestamp NOT NULL
);

CREATE TABLE MaterialType (
	TypeName varchar(50) PRIMARY KEY,
	Lendable bit NOT NULL
);

CREATE TABLE Material (
    ISBN varchar(50) PRIMARY KEY,
	Title varchar(100) NOT NULL,
	Description varchar(max) NOT NULL,
	Author varchar(50)
);

CREATE TABLE Copy (
    CopyID int PRIMARY KEY IDENTITY,
	ISBN varchar(50) FOREIGN KEY REFERENCES Material(ISBN),
	TypeName varchar(50) FOREIGN KEY REFERENCES MaterialType(TypeName),
	LibraryName varchar(50) FOREIGN KEY REFERENCES Library(LibraryName)
);

CREATE TABLE Borrow (
    CopyID int FOREIGN KEY REFERENCES Copy(CopyID),
	SSN int FOREIGN KEY REFERENCES Member(SSN),
	FromDate date NOT NULL,
	ToDate date,
	PRIMARY KEY (CopyID, SSN, FromDate)
);

CREATE TABLE Notice (
    CopyID int NOT NULL,
	SSN int NOT NULL,
	FromDate date NOT NULL,
	noticeSent bit NOT NULL,
	FOREIGN KEY (CopyID, SSN, FromDate) REFERENCES Borrow (CopyID, SSN, FromDate),
	PRIMARY KEY (CopyID, SSN, FromDate)
);
GO

--Procedures
DROP PROCEDURE IF EXISTS Login
GO
CREATE PROCEDURE Login @SSN int, @Password nvarchar(16)
AS

IF (LEN(@Password) > 0) AND (LEN(@Password) <= 16) AND (LEN(@SSN) = 9) AND EXISTS
    (
		SELECT * 
		FROM Person 
		WHERE SSN = @SSN AND Password = @Password
    )
    BEGIN
        SELECT 1
    END
ELSE
    BEGIN
        SELECT 0
    END
GO

drop procedure if exists [CreateMaterials]
go
CREATE PROCEDURE [CreateMaterials] @SSN int, @ISBN int, @library varchar(50), @Author nvarchar(50), @Description varchar(max),
									@Title nvarchar(100), @TypeName varchar(50), @Quantity int
AS
if EXISTS(select * from Librarian where Librarian.SSN = @SSN) and
	exists(select * from Library where Library.LibraryName like @library) and
	exists(select * from MaterialType where MaterialType.TypeName like @TypeName)
	begin
	if not exists(select * from Material where Material.ISBN = @ISBN)
		begin
		INSERT INTO Material (ISBN, Title, Description, Author)VALUES (@ISBN,@Title, @Description, @Author)
		end
	while @Quantity > 0
		begin
		INSERT INTO Copy (ISBN, TypeName, LibraryName)VALUES (@ISBN, @TypeName,@library)
		set @Quantity = @Quantity - 1
		end
	select 1
	end
else
	begin
	select 0
	end
go

drop procedure if exists DeleteMaterial
go
CREATE PROCEDURE DeleteMaterial @SSN int, @ISBN int
AS
if EXISTS(select * from Librarian where Librarian.SSN = @SSN) and
   exists(select * from Material where Material.ISBN = @ISBN) and
   not exists(select * from Copy where Copy.ISBN = @ISBN)
	begin
	DELETE FROM Material WHERE Material.ISBN = @ISBN
	select 1
	end
else
	begin
	select 0
	end
go

drop procedure if exists DeleteCopy
go
CREATE PROCEDURE DeleteCopy @SSN int, @CopyId int
AS
if EXISTS(select * from Librarian where Librarian.SSN = @SSN) and
   exists(select * from Copy where Copy.CopyID = @CopyId)
   begin
	DELETE FROM Copy WHERE Copy.CopyID = @CopyId
	select 1
	end
else
	begin
	select 0
	end
go

DROP PROCEDURE IF EXISTS Returning
GO
CREATE PROCEDURE Returning @CopyId int
AS

UPDATE Borrow
	SET ToDate = GETDATE()
	FROM Borrow
	where CopyID = @CopyID
		AND ToDate is null
GO

DROP PROCEDURE IF EXISTS NoticeFilling
GO
CREATE PROCEDURE NoticeFilling
AS
	INSERT INTO Notice (CopyID, SSN, FromDate, noticeSent)
	SELECT Borrow.CopyID, Borrow.SSN, Borrow.FromDate, 0
	FROM Borrow 
		inner join Member on Borrow.SSN = Member.SSN
		inner join MemberType on Member.TypeName = MemberType.TypeName
		left join Notice on Borrow.CopyID = Notice.CopyID and
			Borrow.SSN = Notice.SSN and
			Borrow.FromDate = Notice.FromDate
	WHERE ToDate IS NULL 
		and Notice.SSN IS NULL 
		and CONVERT(date, getdate()) >= DATEADD(DAY, MemberType.LendingLenght + MemberType.GracePeriod, Borrow.FromDate);
GO

--sql scheduling
--EXEC NoticeFilling

--Triggers

CREATE OR ALTER TRIGGER Lending
ON Borrow
FOR INSERT
AS
BEGIN
	DECLARE @CopyID INT;
	DECLARE @SSN INT;
	DECLARE @FromDate Date;
	SELECT @CopyID = CopyID, @SSN = SSN, @FromDate = FromDate FROM INSERTED

	DECLARE @NrOfBooks INT;
	SELECT @NrOfBooks = NrOfBooks
	FROM Member INNER JOIN MemberType ON Member.TypeName = MemberType.TypeName
	WHERE SSN = @SSN;

    IF (Select Count(*) from Borrow where SSN = @SSN AND ToDate IS NULL) > @NrOfBooks OR --user limit not exceeded
		(Select Count(*) from Borrow where CopyID = @CopyID AND ToDate IS NULL) > 1 --book is available
		BEGIN
			Rollback transaction
		END
	ELSE
		BEGIN
			UPDATE Borrow
			SET FromDate = GETDATE(), ToDate = null
			FROM Borrow
			where CopyID = @CopyID
				AND SSN = @SSN
				AND FromDate = @FromDate
		END
End
GO


--Views
drop view if exists [readAllMaterials]
go
CREATE VIEW readAllMaterials AS
with a as ( 
  SELECT Material.ISBN, Material.Title, Material.Author, Material.Description, copy.LibraryName as Location, copy.TypeName
  FROM Copy left join Borrow on Copy.CopyID = Borrow.CopyID
   inner join Material on Material.isbn = Copy.isbn
   where Borrow.CopyID is null or (Borrow.ToDate is not null and Borrow.CopyID is not null)
  )

SELECT distinct Material.ISBN, Material.Title, Material.Author, Material.Description, copy.LibraryName as Location, copy.TypeName,
(select COUNT(*) from a where copy.TypeName LIKE a.TypeName and a.Location like copy.LibraryName and a.ISBN = copy.ISBN) as Available_Copies
  FROM Copy inner join Material on Material.isbn = Copy.isbn 
go