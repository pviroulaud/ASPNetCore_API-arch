create database uS_logDb;
go
use uS_logDb;
go
create table errorLog(
id int not null primary key identity(1,1),
userId int,
[location] nvarchar(max),
errorDate datetime not null,
errorCode int not null,
detail nvarchar(max),
params nvarchar(max)
);

create table operation(
id int not null primary key identity(1,1),
[name] nvarchar(max) not null,
[description] nvarchar(max)
);

create table operationLog(
id int not null primary key identity(1,1),
userId int,
operationId int not null,
operationDate datetime not null,
entity nvarchar(max),
[description] nvarchar(max),

foreign key (operationId) references operation(id)
);

SET IDENTITY_INSERT [dbo].operation ON 
GO
INSERT [dbo].operation ([id], [name], [description]) VALUES (1, N'Alta',N'Alta')
GO
INSERT [dbo].operation ([id], [name], [description]) VALUES (2, N'Baja',N'Baja')
GO
INSERT [dbo].operation ([id], [name], [description]) VALUES (3, N'Modificación',N'Modificación')
GO
SET IDENTITY_INSERT [dbo].operation OFF
GO