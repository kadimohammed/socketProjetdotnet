create database SocketsProject
use SocketsProject

drop table Utilisateurs

CREATE TABLE Utilisateurs (
    userID INT PRIMARY KEY IDENTITY(1,1),
    telephone VARCHAR(10) NOT NULL,
    fullname VARCHAR(50) NOT NULL,
    password VARCHAR(100) NOT NULL,
    photo VARBINARY(MAX), -- Colonne pour stocker la photo de l'utilisateur
    infos NVARCHAR(MAX) -- Colonne pour stocker des informations supplémentaires
);

select * from Utilisateurs
INSERT INTO Utilisateurs (telephone, fullname, password, photo, infos)
VALUES ('0111111111', 'John Doe', '1234', 0x0123456789ABCDEF, 'Informations sur John Doe'),
       ('0222222222', 'Jane Smith', '1234', 0xFEDCBA9876543210, 'Informations sur Jane Smith'),
       ('0333333333', 'Alice Johnson', '1234', 0x9876543210ABCDEF, 'Informations sur Alice Johnson');

