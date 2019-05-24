use GTL;
GO
DELETE FROM Loan;
GO
DELETE FROM Copy;
GO
DELETE FROM Material;
GO
DELETE FROM MaterialType;
GO
DELETE FROM Member;
GO
DELETE FROM MemberType;
GO
DELETE FROM MemberCard;
GO
DELETE FROM Librarian;
GO
DELETE FROM LibrarianType;
GO
DELETE FROM Person;
GO
DELETE FROM Library;
GO
DELETE FROM Address;
GO
DELETE FROM Location;
GO

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

INSERT INTO Person (SSN, AddressID, CampusID, F_Name, L_Name, Phone, Password)
VALUES (123456781, 1, 1, 'Maleah', 'Holt', 20202020, 'test'),
		(123456782, 3, 3, 'Aiden', 'Pena', 20202020, 'test'),
		(123456783, 7, 7, 'Ashly', 'Nelson', 20202020, 'test'),
		(123456784, 4, 4, 'Memphis', 'Pacheco', 20202020, 'test'),
		(123456785, 6, 6, 'Lincoln', 'Rivas', 20202020, 'test'),
		(123456786, 7, 7, 'Rowan', 'Mcknight', 20202020, 'test'),
		(123456787, 3, 3, 'Drew', 'Steele', 20202020, 'test'),
		(123456788, 2, 2, 'Courtney', 'Mann', 20202020, 'test'),
		(123456789, 5, 5, 'Trinity', 'Knight', 20202020, 'test');

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
		(7,'history book', 'TEST++', 'laha'),
		(8,'deletable material', 'TEST++', 'laha');

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
		(14,1,'needed books','Pikalo'),
		(15,1,'books','HUmm'),
		(16,7,'needed books','Pikalo'),
		(17,7,'needed books','Pikalo');
SET IDENTITY_INSERT Copy OFF;

INSERT INTO Loan (CopyID, SSN, FromDate, ToDate)
VALUES (5,123456786,GETDATE(),null),
		(2,123456786,GETDATE(),GETDATE()),
		(6,123456789,GETDATE(),null),
		(1,123456789,GETDATE(),GETDATE()),
		(15,123456789,'11.03.2001',null),
		(4,123456786,GETDATE(),null),
		(7,123456789,GETDATE(),null),
		(3,123456789,GETDATE(),null),
		(9,123456786,GETDATE(),GETDATE()),
		(11,123456789,GETDATE(),null),
		(16,123456787,GETDATE(),null);