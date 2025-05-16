/*
Do not use database modifying (ALTER DATABASE), creating (CREATE DATABASE) or switching (USE) statements 
in this file.
*/

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
	ISBN nvarchar(20) unique not null,
	[Name] nvarchar(255) not null,
	[Description] nvarchar(max),
	PublicationDate date,
)
go

create table Author (
	Id int primary key identity,
	[Name] nvarchar(50) not null,
)
go

create table [Location] (
	Id int primary key identity,
	[Name] nvarchar(50) not null,
	[Address] nvarchar(max) not null,
)
go

create table BookAuthor (
	BookId int foreign key references Book(Id),
	AuthorId int foreign key references Author(Id),
	primary key (BookId, AuthorId)
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

create table [User] (
	Id int primary key identity,
	Username nvarchar(100) not null,
	PwdHash nvarchar(255) not null,
	PwdSalt nvarchar(255) not null,
	Email nvarchar(255) not null,
	Phone nvarchar(255)
)
go

create table Reservation (
	Id int primary key identity,
	[Date] datetime default getdate(),
	[Status] int default 0 not null,
	UserId int foreign key references [User](Id),
	BookId int foreign key references Book(Id)
)
go

create table Review (
	Id int primary key identity,
	Rating int check (Rating >= 1 and Rating <= 5) not null,
	[Text] nvarchar(max),
	BookId int foreign key references Book(Id) not null,
	UserId int foreign key references [User](Id) not null
)
go

create table BookLog (
	Id int primary key identity,
	[Timestamp] datetime default getdate() not null,
	[Level] int not null,
	[Message] nvarchar(max) not null,
)
go

insert into Author ([Name]) -- 1
values ('Bogdan Laloviæ')
insert into Author ([Name]) -- 2
values ('Adolf Hitler')
insert into Author ([Name]) -- 3
values ('Friedrich Engels')
insert into Author ([Name]) -- 4
values ('Karl Marx')
go

insert into Book ([Name], ISBN)
values ('Minecraft', '12345678901234567890') -- 1
insert into Book ([Name], ISBN)
values ('Mein kampf', '953-6859-00-9') -- 2
insert into Book ([Name], ISBN)
values ('The Communist Manifesto', '978-0-7178-0241-8') -- 3
go

insert into BookAuthor (BookId, AuthorId) values (1, 1)
insert into BookAuthor (BookId, AuthorId) values (2, 2)
insert into BookAuthor (BookId, AuthorId) values (3, 3)
insert into BookAuthor (BookId, AuthorId) values (3, 4)
go