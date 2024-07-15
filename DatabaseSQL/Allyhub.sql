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
	UserType varchar(100),
	UserPhoto varchar(255)
);
update Users SET UserPhoto='Zenun.jpg' where UserId=2
create table Client(
	ClientID int not null primary key identity(1,1),
	CompanyName varchar(50),
	ClientLocation varchar(100),
	UserId int not null,
	foreign key (UserId) references Users (UserId)
);
select * from Project;
select * from Developer;
select* from Users;
select * from Client
drop table Developer

insert into Developer(UserID , PortfolioLink , LinkedIn , Facebook , DevDescription , AreaofExpertise) VALUES (3 , 'https://github.com/afiafahmida' ,'https://www.linkedin.com/in/afia-fahmida-1b1b65243/','https://www.facebook.com/afiafahmida.dona','Hello! I am Afia Fahmida from Bangladesh, a full stuck developer, a tech enthusiast','ASP .NET Developer')

select u.FirstName , u.LastName, u.UserAddress , u.UserPhone , u.UserEmail , d.DeveloperID, d.UserId ,d.DevDescription , d.AreaofExpertise , d.PortfolioLink , d.LinkedIn ,d.Facebook, d.Country , d.Languagee , d.DOB from Developer d JOIN Users u ON d.UserId = u.UserId where DeveloperID=3

create table Developer(
	DeveloperID int not null primary key identity(1,1),
	UserId int not null,
	foreign key (UserId) references Users (UserId),
	PortfolioLink varchar(255),
	LinkedIn varchar(255),
	Facebook varchar(255),
	DevDescription varchar(1000),
	AreaofExpertise varchar(255),
	Country varchar(255),
	Languagee varchar(255),
	DOB varchar(255)
);

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

select * from Project;
INSERT INTO Project (ProjectTitle,ShortDescription ,Description, PaymentAmount, ClientID, ExpertiseLevel, Duration, SkillSet, CompanyName) VALUES
('Website Redesign', 'Complete overhaul of the company website.', 'Complete overhaul of the company website.' ,5000, 1, 'Beginner', '3-6 Months', 'HTML, CSS, JavaScript', 'Tech Innovators'),
('Mobile App Development', 'Develop a mobile app for health tracking.', 'Complete overhaul of the company website.',10000, 1, 'Expert', '1-2 Months', 'Java, Kotlin, Android', 'Health Solutions'),
('E-commerce Platform', 'Create an e-commerce platform for eco-friendly products.', 'Complete overhaul of the company website.',8000, 1, 'Intermediate', '1-3 Months', 'React, Node.js, MongoDB', 'Eco Builders'),
('Data Analysis Tool', 'Build a data analysis tool for marketing analytics.','Complete overhaul of the company website.' ,7000, 1, 'Beginner', '4-6 Months', 'Python, Pandas, Django', 'Tech Innovators'),
('Customer Portal', 'Develop a customer portal for managing health records.','Complete overhaul of the company website.' ,12000, 1, 'Expert', '3-6 Months', 'Angular, .NET, SQL Server', 'Health Solutions'),
('Green Building Design', 'Design a green building project for urban development.', 'Complete overhaul of the company website.',9000, 1, 'Beginner', '3-6 Months', 'AutoCAD, Revit, BIM', 'Eco Builders');


CREATE TABLE Project(
    ProjectID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    ProjectTitle VARCHAR(100),
	ShortDescription varchar(500),
    Description VARCHAR(2000),
    PaymentAmount INT,
    ClientID INT NOT NULL,
    FOREIGN KEY (ClientID) REFERENCES Client(ClientID),
    ExpertiseLevel varchar(255),
    Duration varchar(255),
    SkillSet VARCHAR(100),
	PostedOn DATE DEFAULT GETDATE(),
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