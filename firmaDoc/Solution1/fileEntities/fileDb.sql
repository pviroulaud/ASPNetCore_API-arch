create database fileDb;
go
use fileDb;
go
create table userFile(
 id int not null identity(1,1) primary key,
 userId int not null,
 [fileName] nvarchar(max) not null,
 contentType nvarchar(max) not null,
 size int not null,
 cipher bit not null,
 content varbinary(max) not null
);