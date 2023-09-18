create database certificateDb;
go
use certificateDb;
go
create table electronicCertificate(
id int not null primary key identity (1,1),
userId int not null,
pfx nvarchar(max) not null,
cer nvarchar(max) not null,
pass nvarchar(max) not null,
creationDate datetime not null,
expiratioDate datetime not null,
valid bit not null
);
go