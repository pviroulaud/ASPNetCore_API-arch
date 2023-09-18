create database uS_userDb;
go
use uS_userDb;
go

create table [user](
id int not null identity(1,1) primary key,
[nick] nvarchar(max) not null,
email nvarchar(max) not null
);

create table permit(
id int not null identity(1,1) primary key,
[name] nvarchar(max) not null
);

create table userPermits(
id int not null identity(1,1) primary key,
userId int not null,
permitId int not null,
[enabled] bit not null,

foreign key (userId) references [user](id),
foreign key (permitId) references permit(id)
);

SET IDENTITY_INSERT [dbo].permit ON 
GO
INSERT [dbo].permit ([id], [name]) VALUES (1, N'Upload Document')
GO
INSERT [dbo].permit ([id], [name]) VALUES (2, N'Download Document')
GO
INSERT [dbo].permit ([id], [name]) VALUES (3, N'Sign Document')
GO
SET IDENTITY_INSERT [dbo].permit OFF
GO