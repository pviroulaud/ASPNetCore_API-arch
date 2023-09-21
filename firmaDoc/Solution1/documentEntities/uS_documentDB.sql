create database uS_documentDb;
go
use uS_documentDb;
go
create table document(
 id int not null identity(1,1) primary key,
 title nvarchar(max) not null,
 userId int not null,
 userFileId int,
 userFileStorageDate datetime,
 signedUserFileId int,
 userFileSignDate datetime
);
create table rabbitMqTempFile(
 id int not null identity(1,1) primary key,
 [guid] nvarchar(max) not null,
 [fileName] nvarchar(max) not null,
 contentType nvarchar(max) not null,
 size int not null,
 content varbinary(max) not null
);