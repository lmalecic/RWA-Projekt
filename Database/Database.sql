-- MAKNI OVO PRIJE SLANJA
create database BookLibrary
go

use BookLibrary
go

create table Genre (
	Id int primary key identity,
	[Name] nvarchar(50) not null
)
go

create table Book (
	Id int primary key identity,
	Author nvarchar(50) not null,
	[Name] nvarchar(255) not null,
	[Description] nvarchar(max),
	PublicationDate date,
	ISBN nvarchar(20) unique
)
go

create table [Location] (
	Id int primary key identity,
	[Name] nvarchar(50) not null,
	[Address] nvarchar(max) not null unique,
)
go

create table BookGenre (
	BookId int foreign key references Book(Id),
	GenreId int foreign key references Genre(Id),
	primary key (BookId, GenreId)
)
go

create table BookLocation (
	BookId int foreign key references Book(Id),
	LocationId int foreign key references [Location](Id),
	primary key (BookId, LocationId)
)
go


-- Table for Users
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);
GO

-- Table for Reservations (M-N Relationship between Users and Books)
CREATE TABLE Reservations (
    ReservationID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT,
    BookID INT,
    ReservationDate DATETIME DEFAULT GETDATE(),
    ReservationStatus NVARCHAR(50) DEFAULT 'Pending', -- Pending, Approved, Canceled
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (BookID) REFERENCES Books(BookID)
);
GO

-- Table for Reviews (for Rating and Comments by Users on Books)
CREATE TABLE Reviews (
    ReviewID INT PRIMARY KEY IDENTITY(1,1),
    BookID INT,
    UserID INT,
    Rating INT CHECK (Rating >= 1 AND Rating <= 5), -- Rating between 1 and 5
    ReviewText TEXT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (BookID) REFERENCES Books(BookID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);
GO

-- Table for Admin Logs (to track who made changes to Books)
CREATE TABLE AdminLogs (
    LogID INT PRIMARY KEY IDENTITY(1,1),
    AdminUserID INT,  -- Reference to Users table where role = Admin
    ActionType NVARCHAR(50), -- 'Add', 'Edit', 'Delete'
    BookID INT,
    ActionDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (AdminUserID) REFERENCES Users(UserID),
    FOREIGN KEY (BookID) REFERENCES Books(BookID)
);
GO