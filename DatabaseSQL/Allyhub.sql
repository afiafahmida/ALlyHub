create database Allyhub;

use Allyhub;

create table Users(
    UserId int not null primary key identity(1,1),
    FirstName varchar(50),
    LastName varchar(50),
    UserEmail varchar(50),
    UserPassword varchar(100),
    UserPhone varchar(20),
    UserAddress varchar(100),
	UserType varchar(100)
);
create table Client(
	ClientID int not null primary key identity(1,1),
	CompanyName varchar(50),
	ClientLocation varchar(100),
	UserId int not null,
	foreign key (UserId) references Users (UserId)
);
select * from Client;
select * from Developer;
select* from Users;
insert into Client(ClientID,CompanyName,ClientLocation,UserId)values(1,'ncajn','ancan',1);

create table Developer(
	DeveloperID int not null primary key identity(1,1),
	UserId int not null,
	foreign key (UserId) references Users (UserId),
	PortfolioLink varchar(100),
	LinkedIn varchar(100),
	DevDescription varchar(100),

);
INSERT INTO Project (ProjectTitle, Description, PaymentAmount, ClientID, Level, Duration, SkillSet) VALUES
('Website Development', 'Develop a corporate website', 1000, 1, 3, 4, 'HTML, CSS, JavaScript'),
('UI/UX Development', 'Develop a corporate website design', 1000, 1, 3, 4, 'Figma,AdobeXD'),
('.NET Development', 'Develop a Ecommerce Website', 1000, 1, 3, 4, 'HTML, CSS, JavaScript,.NET'),
('Mobile Application Development', 'Develop a Mobile Application for Habit tracker', 1000, 1, 3, 4, 'HTML, CSS, JavaScript,React Native')
create table DeveloperSkill (
	SkillID int not null primary key identity(1,1),
	SkillName varchar(100),
	DeveloperID int not null,
	foreign key (DeveloperID) references Developer (DeveloperID)
);

create table Certification(
	CertificateID int not null primary key identity(1,1),
	Name varchar(100),
	DeveloperID int not null,
	foreign key (DeveloperID) references Developer (DeveloperID),
	Provider varchar(100),
	CertificationLink varchar(100),
	DateEarned date
);

create table Review(
	ReviewID int not null primary key identity(1,1),
	Rating int not null,
	Description varchar(100),
	Date date,
	GivenBy varchar(100),
	GivenTo varchar(100)
);

CREATE TABLE Project(
    ProjectID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    ProjectTitle VARCHAR(100),
    Description VARCHAR(255),
    PaymentAmount INT,
    ClientID INT NOT NULL,
    FOREIGN KEY (ClientID) REFERENCES Client(ClientID),
    Level INT,
    Duration INT,
    SkillSet VARCHAR(100),
    CompanyName VARCHAR(100) -- New field for company name
);


create table Handshake(
	HandshakeID int not null primary key identity(1,1),
	ProjectID int not null,
	foreign key (ProjectID) references Project (ProjectID),
	DeveloperID int not null,
	foreign key (DeveloperID) references Developer (DeveloperID),
	HandshakeDate date 
);

create table Payment(
	PaymentID int not null primary key identity(1,1),
	HandshakeID int not null,
	foreign key (HandshakeID) references Handshake (HandshakeID),
	Amount int,
	PaymentDate date
);

create table PaymentDetails(
	PaymentID int not null,
	foreign key (PaymentID) references Payment (PaymentID),
	PaymentMethod varchar(100),
	Merchant varchar(100),
	AccountNumber int
);

INSERT INTO Project (ProjectTitle, Description, PaymentAmount, ClientID, Level, Duration, SkillSet, CompanyName) VALUES
('Website Redesign', 'Complete overhaul of the company website.', 5000, 1, 3, 8, 'HTML, CSS, JavaScript', 'Tech Innovators')

select * from Users;