USE master
GO
IF DB_ID('GTL') IS NOT NULL
ALTER DATABASE GTL SET SINGLE_USER WITH ROLLBACK IMMEDIATE
DROP DATABASE IF EXISTS GTL;
GO

CREATE DATABASE GTL;
GO

USE GTL;
GO

--CREATE

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
	F_Name varchar(50),
	L_Name varchar(50),
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

CREATE TABLE Loan (
    CopyID int FOREIGN KEY REFERENCES Copy(CopyID),
	SSN int FOREIGN KEY REFERENCES Member(SSN),
	FromDate date NOT NULL,
	ToDate date,
	noticeSent bit,
	PRIMARY KEY (CopyID, SSN, FromDate)
);
GO

CREATE NONCLUSTERED INDEX ix_CopyIsbn_ReferenceMaterialIsbn
ON [dbo].[Copy]
(
    [ISBN] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE=OFF, SORT_IN_TEMPDB=OFF);
GO


--Procedures

DROP PROCEDURE IF EXISTS Login
GO
CREATE PROCEDURE Login @SSN int, @Password nvarchar(16)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	BEGIN TRANSACTION
	IF (LEN(@Password) > 0) AND (LEN(@Password) <= 16) AND (LEN(@SSN) = 9) AND EXISTS
		(
			SELECT * 
			FROM Person 
			WHERE SSN = @SSN AND Password = @Password
		)
		BEGIN
			COMMIT
			SELECT 1
		END
	ELSE
		BEGIN
			ROLLBACK
			SELECT 0
		END
END
GO

DROP PROCEDURE IF EXISTS CreateMaterials
GO
CREATE PROCEDURE CreateMaterials @SSN int, @ISBN int, @library varchar(50), @Author nvarchar(50), @Description varchar(max),
									@Title nvarchar(100), @TypeName varchar(50), @Quantity int
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL REPEATABLE READ
	BEGIN TRANSACTION
	IF EXISTS(SELECT * FROM Librarian WHERE Librarian.SSN = @SSN) AND
		EXISTS(SELECT * FROM Library WHERE Library.LibraryName LIKE @library) AND
		EXISTS(SELECT * FROM MaterialType WHERE MaterialType.TypeName LIKE @TypeName)
		BEGIN
		IF NOT EXISTS(SELECT * FROM Material WHERE Material.ISBN = @ISBN)
			BEGIN
			INSERT INTO Material (ISBN, Title, Description, Author)VALUES (@ISBN,@Title, @Description, @Author)
			END
		WHILE @Quantity > 0
			BEGIN
			INSERT INTO Copy (ISBN, TypeName, LibraryName)VALUES (@ISBN, @TypeName,@library)
			SET @Quantity = @Quantity - 1
			END
		COMMIT
		SELECT 1
		END
	ELSE
		BEGIN
		ROLLBACK
		SELECT 0
		END
END
GO

DROP PROCEDURE IF EXISTS DeleteMaterial
GO
CREATE PROCEDURE DeleteMaterial @SSN int, @ISBN int
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	BEGIN TRANSACTION
	IF EXISTS(SELECT * FROM Librarian WHERE Librarian.SSN = @SSN) AND
	   EXISTS(SELECT * FROM Material WHERE Material.ISBN = @ISBN) AND
	   NOT EXISTS(SELECT * FROM Copy WHERE Copy.ISBN = @ISBN)
		BEGIN
		DELETE FROM Material WHERE Material.ISBN = @ISBN
		COMMIT
		SELECT 1
		END
	ELSE
		BEGIN
		ROLLBACK
		SELECT 0
		END
END
GO

DROP PROCEDURE IF EXISTS DeleteCopy
GO
CREATE PROCEDURE DeleteCopy @SSN int, @CopyId int
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	BEGIN TRANSACTION
	IF EXISTS(SELECT * FROM Librarian WHERE Librarian.SSN = @SSN) AND
	   EXISTS(SELECT * FROM Copy WHERE Copy.CopyID = @CopyId)
	   BEGIN
		DELETE FROM Copy WHERE Copy.CopyID = @CopyId
		COMMIT
		SELECT 1
		END
	ELSE
		BEGIN
		ROLLBACK
		SELECT 0
		END
END
GO

DROP PROCEDURE IF EXISTS Returning
GO
CREATE PROCEDURE Returning @CopyId int
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	BEGIN TRANSACTION
	UPDATE Loan
		SET ToDate = GETDATE()
		FROM Loan
		WHERE CopyID = @CopyID
			AND ToDate IS NULL
	Commit
END
GO

DROP PROCEDURE IF EXISTS NoticeFilling
GO
CREATE PROCEDURE NoticeFilling
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	BEGIN TRANSACTION
	UPDATE Loan
	SET noticeSent = 0
	FROM Loan 
		INNER JOIN Member ON Loan.SSN = Member.SSN
		INNER JOIN MemberType ON Member.TypeName = MemberType.TypeName
	WHERE ToDate IS NULL 
		AND Loan.noticeSent IS NULL 
		AND CONVERT(date, getdate()) >= DATEADD(DAY, MemberType.LendingLenght + MemberType.GracePeriod, Loan.FromDate);
	COMMIT
END
GO

DROP PROCEDURE IF EXISTS AverageLoanTime
GO
CREATE PROCEDURE AverageLoanTime
AS
BEGIN
	WITH a AS ( 
	SELECT FromDate, ToDate FROM Loan where ToDate is NOT null
	  )

	SELECT CAST((SELECT SUM(DATEDIFF(DAY, FromDate, ToDate)) FROM a)/(SELECT COUNT(*) FROM a) AS DECIMAL(10, 2)) AS AverageLoaningTime
END
GO

DROP PROCEDURE IF EXISTS TopLoaningLibrary
GO
CREATE PROCEDURE TopLoaningLibrary
AS
BEGIN
	SELECT COUNT(Copy.CopyID) AS loaned_count, copy.LibraryName AS Location FROM Loan
	JOIN Copy ON Copy.CopyID = Loan.CopyID WHERE copy.LibraryName NOT LIKE 'GTL'
	GROUP BY LibraryName
END
GO
--Triggers

CREATE OR ALTER TRIGGER Lending
ON Loan
FOR INSERT
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
	BEGIN TRANSACTION
	DECLARE @CopyID INT;
	DECLARE @SSN INT;
	DECLARE @FromDate Date;
	SELECT @CopyID = CopyID, @SSN = SSN, @FromDate = FromDate FROM INSERTED

	DECLARE @NrOfBooks INT;
	SELECT @NrOfBooks = NrOfBooks
	FROM Member INNER JOIN MemberType ON Member.TypeName = MemberType.TypeName
	WHERE SSN = @SSN;

    IF (SELECT COUNT(*) FROM Loan WHERE SSN = @SSN AND ToDate IS NULL) > @NrOfBooks OR --user limit not exceeded
		(SELECT COUNT(*) FROM Loan WHERE CopyID = @CopyID AND ToDate IS NULL) > 1 --book is available
		BEGIN
			ROLLBACK
		END
	ELSE
		BEGIN
			UPDATE Loan
			SET FromDate = GETDATE(), ToDate = NULL
			FROM Loan
			WHERE CopyID = @CopyID
				AND SSN = @SSN
				AND FromDate = @FromDate
			COMMIT
		END
End
GO

--Views

DROP VIEW IF EXISTS readAllMaterials
GO
CREATE VIEW readAllMaterials AS
	WITH a AS ( 
	  SELECT Material.ISBN, Material.Title, Material.Author, Material.Description, copy.LibraryName as Location, copy.TypeName
	  FROM Copy LEFT JOIN Loan ON Copy.CopyID = Loan.CopyID
	   INNER JOIN Material ON Material.isbn = Copy.isbn
	   WHERE Loan.CopyID IS NULL OR (Loan.ToDate IS NOT NULL and Loan.CopyID IS NOT NULL)
	  )

	SELECT DISTINCT Material.ISBN, Material.Title, Material.Author, Material.Description, copy.LibraryName AS Location, copy.TypeName,
	(SELECT COUNT(*) FROM a WHERE copy.TypeName LIKE a.TypeName AND a.Location LIKE copy.LibraryName AND a.ISBN = copy.ISBN) AS Available_Copies
	  FROM Copy INNER JOIN Material ON Material.isbn = Copy.isbn
GO

DROP VIEW IF EXISTS topLoanedBooks
GO
CREATE VIEW topLoanedBooks AS
	WITH a AS ( 
	SELECT ISBN FROM Loan
	JOIN Copy ON Copy.CopyID = Loan.CopyID
	  )

	SELECT DISTINCT top(10) a.ISBN, Material.Title, Material.Author, Material.Description,
	(SELECT COUNT(*) FROM a WHERE a.ISBN = copy.ISBN) AS loaned_count
	  FROM Copy INNER JOIN Material ON Material.isbn = Copy.isbn
	  JOIN a ON a.ISBN = Copy.ISBN
GO 