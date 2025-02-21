CREATE TABLE Author
(
	Id INT IDENTITY (1,1) PRIMARY KEY,
	Name NVARCHAR (50),
	LifeTime NVARCHAR (10),
	Country NVARCHAR (50)
)

CREATE TABLE Genre_Dictionary 
(
	Id INT IDENTITY (1,1) PRIMARY KEY,
	Name NVARCHAR (50),
	Description NVARCHAR (150)
)

CREATE TABLE MyRole_Dictionary 
(
	Id INT IDENTITY (1,1) PRIMARY KEY,
	Name NVARCHAR (50)
)

CREATE TABLE MyUser
(
	Id INT IDENTITY (1,1) PRIMARY KEY,
	FuulName NVARCHAR (100),
	Email NVARCHAR (50) NOT NULL,
	Password NVARCHAR (25) NOT NULL,
	Role INT FOREIGN KEY REFERENCES MyRole_Dictionary(Id) NOT NULL
)

CREATE TABLE Book 
(
	Id INT IDENTITY (1,1) PRIMARY KEY,
	Author INT FOREIGN KEY REFERENCES Author(Id) NOT NULL,
	Title INT NVARCHAR (50) NOT NULL,
	Genre INT FOREIGN KEY REFERENCES Genre_Dictionary(Id),
	Year DATETIME
)

CREATE TABLE UserBookHistory
(
	UserId INT FOREIGN KEY REFERENCES MyUser(Id),
	BookId INT FOREIGN KEY REFERENCES Book(Id),
	DateTaken DATETIME,
	DateReturned DATETIME
)