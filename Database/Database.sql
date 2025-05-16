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






/**************
 *   GENRAE   *
 **************/

insert into Author ([Name]) -- 1
values ('Bogdan Lalović')
insert into Author ([Name]) -- 2
values ('Adolf Hitler')
insert into Author ([Name]) -- 3
values ('Friedrich Engels')
insert into Author ([Name]) -- 4
values ('Karl Marx')
insert into Author ([Name]) -- 5
values ('Ivana Brlić-Mažuranić')
insert into Author ([Name]) values ('Andrzej Sapkowski')
go

insert into Book ([Name], ISBN) -- 1
values ('Minecraft', '12345678901234567890')
insert into Book ([Name], ISBN) -- 2
values ('Mein kampf', '953-6859-00-9')
insert into Book ([Name], ISBN) -- 3
values ('The Communist Manifesto', '978-0-7178-0241-8')
insert into Book ([Name], ISBN) -- 4
values ('Čudnovate zgode šegrta Hlapića', '978-953-6499-33-5')
insert into Book ([Name], ISBN) -- 5
values ('Priče iz davnine', '953-196-797-0')
insert into Book ([Name], ISBN)
values ('The Witcher: The Last Wish', '978-0-575-08244-1')
insert into Book ([Name], ISBN)
values ('The Witcher: Sword of Destiny', '978-1-4732-1153-7')
insert into Book ([Name], ISBN)
values ('The Witcher: Blood of Elves', '978-0-575-08484-1')
insert into Book ([Name], ISBN)
values ('The Witcher: Time of Contempt', '978-0-575-09094-1')
insert into Book ([Name], ISBN)
values ('The Witcher: Baptism of Fire', '978-0-575-09097-2')
insert into Book ([Name], ISBN)
values ('The Witcher: The Tower of the Swallow', '978-0-316-27371-8')
insert into Book ([Name], ISBN)
values ('The Witcher: The Lady of the Lake', '978-0-316-27383-1')
insert into Book ([Name], ISBN)
values ('The Witcher: Season of Storms', '9780316441629')
insert into Book ([Name], ISBN)
values ('The Witcher: Crossroads of Ravens', '9788375782073')
go

insert into BookAuthor (BookId, AuthorId) values (1, 1)
insert into BookAuthor (BookId, AuthorId) values (2, 2)
insert into BookAuthor (BookId, AuthorId) values (3, 3)
insert into BookAuthor (BookId, AuthorId) values (3, 4)
insert into BookAuthor (BookId, AuthorId) values (4, 5)
insert into BookAuthor (BookId, AuthorId) values (5, 5)
insert into BookAuthor (BookId, AuthorId) values (6, 6)
insert into BookAuthor (BookId, AuthorId) values (7, 6)
insert into BookAuthor (BookId, AuthorId) values (8, 6)
insert into BookAuthor (BookId, AuthorId) values (9, 6)
insert into BookAuthor (BookId, AuthorId) values (10, 6)
insert into BookAuthor (BookId, AuthorId) values (11, 6)
insert into BookAuthor (BookId, AuthorId) values (12, 6)
insert into BookAuthor (BookId, AuthorId) values (13, 6)
insert into BookAuthor (BookId, AuthorId) values (14, 6)
go

insert into Genre([Name])
values ('Fantasy')
insert into Genre([Name])
values ('Fairy tale')
insert into Genre([Name])
values ('Children''s literature')
insert into Genre([Name])
values ('Survival')
insert into Genre([Name])
values ('Sandbox')
insert into Genre([Name])
values ('Autobiography')
insert into Genre([Name])
values ('Political manifesto')
insert into Genre([Name])
values ('Political philosophy')
insert into Genre([Name])
values ('Philosophy')
insert into Genre([Name])
values ('Novel')

insert into BookGenre(BookId, GenreId)
values (1, 4)
insert into BookGenre(BookId, GenreId)
values (1, 5)
insert into BookGenre(BookId, GenreId)
values (2, 6)
insert into BookGenre(BookId, GenreId)
values (2, 7)
insert into BookGenre(BookId, GenreId)
values (2, 8)
insert into BookGenre(BookId, GenreId)
values (3, 9)
insert into BookGenre(BookId, GenreId)
values (4, 10)
insert into BookGenre(BookId, GenreId)
values (5, 2)
insert into BookGenre(BookId, GenreId)
values (5, 3)
insert into BookGenre(BookId, GenreId)
values (6, 1)
insert into BookGenre(BookId, GenreId)
values (7, 1)
insert into BookGenre(BookId, GenreId)
values (8, 1)
insert into BookGenre(BookId, GenreId)
values (9, 1)
insert into BookGenre(BookId, GenreId)
values (10, 1)
insert into BookGenre(BookId, GenreId)
values (11, 1)
insert into BookGenre(BookId, GenreId)
values (12, 1)
insert into BookGenre(BookId, GenreId)
values (13, 1)
insert into BookGenre(BookId, GenreId)
values (14, 1)

-- Preview
select b.[Name] 'Book', a.[Name] 'Author', g.[Name] 'Genre' from BookAuthor
inner join Book b on b.Id = BookId
inner join Author a on a.Id = AuthorId
left join BookGenre bg on b.Id = bg.BookId
left join Genre g on bg.GenreId = g.Id
