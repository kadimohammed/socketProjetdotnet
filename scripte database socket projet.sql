create database SocketsProject
use SocketsProject

/*
drop database SocketsProject1
drop table Utilisateurs
drop table Contacts
*/

CREATE TABLE Utilisateurs (
    userID INT PRIMARY KEY IDENTITY(1,1),
    telephone VARCHAR(10) NOT NULL,
    fullname VARCHAR(50) NOT NULL,
    password VARCHAR(100) NOT NULL,
    photo VARBINARY(MAX), -- Colonne pour stocker la photo de l'utilisateur
    infos NVARCHAR(MAX) -- Colonne pour stocker des informations supplémentaires
);


CREATE TABLE Contacts (
    contactID INT PRIMARY KEY identity(1,1),
    userID INT,
    contactUserID INT,
    FOREIGN KEY (userID) REFERENCES Utilisateurs(userID),
    FOREIGN KEY (contactUserID) REFERENCES Utilisateurs(userID)
);




CREATE TABLE Conversations (
    conversationID INT PRIMARY KEY,
    conversationName NVARCHAR(100),
    createDate DATETIME DEFAULT GETDATE()
);

-- Création de la table Messages
CREATE TABLE Messages (
    messageID INT PRIMARY KEY,
    content NVARCHAR(MAX) NOT NULL,
    userID INT,
    conversationID INT,
    sendDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (userID) REFERENCES Utilisateurs(userID),
    FOREIGN KEY (conversationID) REFERENCES Conversations(conversationID)
);



-- Création de la table Groupes
CREATE TABLE Groupes (
    groupID INT PRIMARY KEY,
    groupName NVARCHAR(100),
    groupDescription NVARCHAR(255)
);

-- Création de la table Group_Members
CREATE TABLE Group_Members (
    groupID INT,
    userID INT,
    PRIMARY KEY (groupID, userID),
    FOREIGN KEY (groupID) REFERENCES Groupes(groupID),
    FOREIGN KEY (userID) REFERENCES Utilisateurs(userID)
);


insert into Contacts values(1,2),(1,3),(1,4);


delete __EFMigrationsHistory
select * from Utilisateurs
select * from Contacts


select * from Utilisateurs
INSERT INTO Utilisateurs (telephone, fullname, password, photo, infos)
VALUES ('1', 'John Doe', '1', 0x0123456789ABCDEF, 'Informations sur John Doe'),
       ('0222222222', 'Jane Smith', '1234', 0xFEDCBA9876543210, 'Informations sur Jane Smith'),
       ('0333333333', 'Alice Johnson', '1234', 0x9876543210ABCDEF, 'Informations sur Alice Johnson');

