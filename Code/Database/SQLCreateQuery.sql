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

EXEC sp_configure 'nested triggers', 0 ;  
GO  
RECONFIGURE;  
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
	FromDate date,
	ToDate date,
	PRIMARY KEY (CopyID, SSN, FromDate)
);
GO

--Procedures

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

CREATE PROCEDURE Returning @CopyId int
AS

UPDATE Borrow
	SET ToDate = GETDATE()
	FROM Borrow
	where CopyID = @CopyID
		AND ToDate is null
GO

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

CREATE OR ALTER TRIGGER Returning
ON Borrow
FOR UPDATE
AS
BEGIN
	DECLARE @CopyID INT;
	DECLARE @SSN INT;
	SELECT @CopyID = CopyID, @SSN = SSN FROM INSERTED

	DECLARE @LendingLenght INT;
	DECLARE @GracePeriod INT;
	SELECT @LendingLenght = LendingLenght, @GracePeriod = GracePeriod
	FROM Member INNER JOIN MemberType ON Member.TypeName = MemberType.TypeName
	WHERE SSN = @SSN;

	DECLARE @ToDate Date;
	Select @ToDate = FromDate from Borrow where SSN = @SSN 
		AND CopyID = @CopyID
		AND ToDate IS NULL
	
	DECLARE @ReturnDate Date;
	SELECT @ReturnDate = DATEADD(DAY, @LendingLenght + @GracePeriod, @ToDate)

	UPDATE Borrow
	SET ToDate = GETDATE()
	FROM Borrow
	where CopyID = @CopyID
		AND SSN = @SSN
		AND ToDate = @ToDate

	IF @ReturnDate > GETDATE()
		BEGIN
			select 0 --returned after the end of grace period
		END
	ELSE
		BEGIN
			select 1 --returned bofore the end of grace period
		END
End
go

--Views
drop view if exists [readAllMaterials]
go
CREATE VIEW readAllMaterials AS
with a as ( 
  SELECT Material.ISBN, Material.Title, Material.Author, Material.Description, copy.LibraryName as Location, copy.TypeName
  FROM Copy inner join Material on Material.isbn = Copy.isbn
  )

SELECT distinct Material.ISBN, Material.Title, Material.Author, Material.Description, copy.LibraryName as Location, copy.TypeName,
(select COUNT(*) from a where copy.TypeName LIKE a.TypeName and a.Location like copy.LibraryName and a.ISBN = copy.ISBN) as Available_Copies
  FROM Copy inner join Material on Material.isbn = Copy.isbn 
go

--Inserts

INSERT INTO Location (PostalCode, City)
VALUES (1000, 'Copenhagen K'),
		(3000, 'Helsingør'),
		(4000, 'Roskilde'),
		(5000, 'Odense C'),
		(9000, 'Aalborg');

SET IDENTITY_INSERT Address ON;
INSERT INTO Address (AddressID, PostalCode, Street, Number)
VALUES (1, 1000, 'test street', 1),
		(2, 3000, 'good street', 33),
		(3, 1000, 'bad street', 3),
		(4, 9000, 'interesting street', 8),
		(5, 5000, 'joke street', 6),
		(6, 3000, 'fake street', 567),
		(7, 9000, 'mock street', 65);
SET IDENTITY_INSERT Address OFF; 

INSERT INTO Library (LibraryName, AddressID)
VALUES ('GTL', 1),
		('Pikalo', 3),
		('HUmm', 7);

INSERT INTO Person (SSN, AddressID, CampusID, Phone, Password)
VALUES (123456781, 1, 1, 20202020, 'test'),
		(123456782, 3, 3, 20202020, 'test'),
		(123456783, 7, 7, 20202020, 'test'),
		(123456784, 4, 4, 20202020, 'test'),
		(123456785, 6, 6, 20202020, 'test'),
		(123456786, 7, 7, 20202020, 'test'),
		(123456787, 3, 3, 20202020, 'test'),
		(123456788, 2, 2, 20202020, 'test'),
		(123456789, 5, 5, 20202020, 'test');

INSERT INTO LibrarianType (TypeName)
VALUES ('chief librarian'),
		('departmental associate librarians'),
		('reference librarians'),
		('check-out staff'),
		('library assistants');

INSERT INTO Librarian (SSN, TypeName)
VALUES (123456781,'chief librarian'),
		(123456782,'departmental associate librarians'),
		(123456783,'reference librarians'),
		(123456784,'check-out staff'),
		(123456785,'library assistants');

SET IDENTITY_INSERT MemberCard ON;
INSERT INTO MemberCard (MemberCardID, Photo, ExpirationDate)
VALUES (1, 0, GETDATE()),
		(2,0, GETDATE()),
		(3,0, GETDATE()),
		(4,0, GETDATE());
SET IDENTITY_INSERT MemberCard OFF;

INSERT INTO MemberType (TypeName, LendingLenght, GracePeriod, NrOfBooks)
VALUES ('Member', 21, 7, 5),
		('Professor', 90, 14, 5);

INSERT INTO Member (SSN, MemberCardID, TypeName)
VALUES (123456786, 1,'Member'),
		(123456787, 2,'Member'),
		(123456788, 3,'Member'),
		(123456789, 4,'Professor');

INSERT INTO MaterialType (TypeName, Lendable)
VALUES ('reference books', 0),
		('rare books', 0),
		('maps', 0),
		('books', 1),
		('needed books', 0);

INSERT INTO Material (ISBN, Title, Description, Author)
VALUES (1,'test book', 'TEST++', 'Hala'),
		(2,'horror book', 'TEST++', 'Pala'),
		(3,'comedy book', 'TEST++', 'KHala'),
		(4,'drama book', 'TEST++', 'ala'),
		(5,'mystery book', 'TEST++', 'asdf'),
		(6,'mystery book', 'TEST++', 'fdsa'),
		(7,'history book', 'TEST++', 'laha');

SET IDENTITY_INSERT Copy ON;
INSERT INTO Copy (CopyID, ISBN, TypeName, LibraryName)
VALUES (1,1,'books','GTL'),
		(2,1,'books','GTL'),
		(3,4,'books','HUmm'),
		(4,6,'maps','GTL'),
		(5,1,'books','Pikalo'),
		(6,3,'books','GTL'),
		(7,1,'needed books','Pikalo'),
		(8,5,'books','Pikalo'),
		(9,1,'reference books','GTL'),
		(10,2,'books','GTL'),
		(11,6,'books','GTL'),
		(12,1,'books','GTL'),
		(13,1,'books','GTL'),
		(14,1,'needed books','Pikalo');
SET IDENTITY_INSERT Copy OFF;

INSERT INTO Borrow (CopyID, SSN, FromDate, ToDate)
VALUES (5,123456786,GETDATE(),null),
		(3,123456788,GETDATE(),null),
		(2,123456786,GETDATE(),GETDATE()),
		(6,123456789,GETDATE(),null),
		(1,123456789,GETDATE(),GETDATE()),
		(4,123456786,GETDATE(),null),
		(7,123456789,GETDATE(),null),
		(3,123456789,GETDATE(),null),
		(9,123456786,GETDATE(),GETDATE()),
		(2,123456789,GETDATE(),null),
		(11,123456789,GETDATE(),null);