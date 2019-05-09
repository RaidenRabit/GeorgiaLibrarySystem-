USE master
DROP DATABASE GTL;
GO

CREATE DATABASE GTL;
Go

use GTL;

CREATE TABLE Address (
    AddressID int PRIMARY KEY IDENTITY,
    City varchar(100) NOT NULL,
    PostalCode int NOT NULL,
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