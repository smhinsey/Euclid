CREATE TABLE [dbo].[Forum](
	[Identifier] [uniqueidentifier] NOT NULL,
	[Created] [datetime] NULL,
	[Modified] [datetime] NULL,
	[Name] [nvarchar](125) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[UrlSlug] [nvarchar](125) NOT NULL,
	[UrlHostName] [nvarchar](2000) NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Identifier] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


create table [Person] (
Identifier uniqueidentifier not null,
Created datetime not null,
Modified datetime null,
[FirstName] [nvarchar](255) NOT NULL,
[LastName] [nvarchar](255) NOT NULL,
[Email] [nvarchar](255) NOT NULL,

primary key nonclustered (Identifier)
)

create table [Badge] (
Identifier uniqueidentifier not null,
Created datetime not null,
Modified datetime null,
ImageUrl nvarchar(2000) not null,
Title nvarchar(25) not null,
[Description] nvarchar(500) not null
primary key nonclustered (Identifier)
)

CREATE TABLE [dbo].[User](
	[Identifier] [uniqueidentifier] NOT NULL,
	[PersonIdentifier] [uniqueidentifier] not null references Person(Identifier),
	[ForumIdentifier] [uniqueidentifier] not null references Forum(Identifier),
	[LastLogin] [datetime] NOT NULL,
	[Username] [nvarchar](100) not null,
	[PasswordHash] [nvarchar](255) NOT NULL,
	[PasswordSalt] [nvarchar](255) NOT NULL,
	[Reputation] [int] not null default(0)
PRIMARY KEY NONCLUSTERED 
(
	[Identifier] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

create table [UserBadge] (
	BadgeIdentifier uniqueidentifier not null references Badge(Identifier),
	UserIdentifier uniqueidentifier not null references [User](Identifier),
	primary key nonclustered (BadgeIdentifier, UserIdentifier)
)

CREATE TABLE [dbo].[Organization](
	[Identifier] [uniqueidentifier] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Name] [nvarchar](125) NOT NULL,
	[Modified] [datetime] NULL
PRIMARY KEY NONCLUSTERED 
(
	[Identifier] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

create table [OrganizationBadge] (
	BadgeIdentifier uniqueidentifier not null references Badge(Identifier),
	OrganizationIdentifier uniqueidentifier not null references [Organization](Identifier),
	primary key nonclustered (BadgeIdentifier, OrganizationIdentifier)
)

create table [Tag] (
Identifier uniqueidentifier not null,
OrganizationIdentifier uniqueidentifier not null references Organization(Identifier),
Created datetime not null,
Modified datetime null,
Value nvarchar(25) not null,

primary key nonclustered (Identifier)
)

create table [Category] (
Identifier uniqueidentifier not null,
OrganizationIdentifier uniqueidentifier not null references Organization(Identifier),
ParentIdentifier uniqueidentifier not null references Category(Identifier),
Created datetime not null,
Modified datetime null,
Value nvarchar(25) not null,

primary key nonclustered (Identifier)
)

create table [ForumBadge] (
	BadgeIdentifier uniqueidentifier not null references Badge(Identifier),
	ForumIdentifier uniqueidentifier not null references [Forum](Identifier),
	primary key nonclustered (BadgeIdentifier, ForumIdentifier)
)

create table [ForumCategory] (
	ForumIdentifier uniqueidentifier not null references Forum(Identifier),
	CategoryIdentifier uniqueidentifier not null references Category(Identifier),
	
	primary key nonclustered (ForumIdentifier, CategoryIdentifier)
)

create table [ForumTag] (
	ForumIdentifier uniqueidentifier not null references Forum(Identifier),
	TagIdentifier uniqueidentifier not null references Tag(Identifier),

	primary key nonclustered (ForumIdentifier, TagIdentifier)
)

create table [Post] (
Identifier uniqueidentifier not null,
ForumIdentifier uniqueidentifier not null references Forum(Identifier),
AuthorIdentifier uniqueidentifier not null references [User](Identifier),
Created datetime not null,
Modified datetime null,
Title nvarchar(150) not null,
Body nvarchar(max) not null,
Score float not null default(0),

primary key nonclustered (Identifier)
)


create table [Comment] (
Identifier uniqueidentifier not null,
PostIdentifier uniqueidentifier not null references Post(Identifier),
AuthorIdentifier uniqueidentifier not null references [User](Identifier),
Created datetime not null,
Modified datetime null,
Body nvarchar(max) not null,
Score float not null default(0),

primary key nonclustered (Identifier)
)

create table [PostTag] (
	PostIdentifier uniqueidentifier not null references Post(Identifier),
	TagIdentifier uniqueidentifier not null references Tag(Identifier)
	
	primary key nonclustered (PostIdentifier, TagIdentifier)
)


create table [CommentTag] (
	CommentIdentifier uniqueidentifier not null references Comment(Identifier),
	TagIdentifier uniqueidentifier not null references Tag(Identifier)
	
	primary key nonclustered (CommentIdentifier, TagIdentifier)
)

create table [UserPostVote] (
	PostIdentifier uniqueidentifier not null references Post(Identifier),
	UserIdentifier uniqueidentifier not null references [User](Identifier),
	VoteAmount float not null default(0)
	
	primary key nonclustered (PostIdentifier, UserIdentifier)
)

create table [UserCommentVote] (
	CommentIdentifier uniqueidentifier not null references Comment(Identifier),
	UserIdentifier uniqueidentifier not null references [User](Identifier),
	VoteAmount float not null default(0)
	
	primary key nonclustered (CommentIdentifier, UserIdentifier)
)

CREATE TABLE [dbo].[OrganizationAdministrators](
	[OrganizationIdentifier] [uniqueidentifier] NOT NULL references Organization(Identifier),
	[UserIdentifier] [uniqueidentifier] NOT NULL references [User](Identifier),
PRIMARY KEY NONCLUSTERED 
(
	[OrganizationIdentifier] ASC,
	[UserIdentifier] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[ForumModerators](
	[ForumIdentifier] [uniqueidentifier] NOT NULL references Forum(Identifier),
	[UserIdentifier] [uniqueidentifier] NOT NULL references [User](Identifier),
PRIMARY KEY NONCLUSTERED 
(
	[ForumIdentifier] ASC,
	[UserIdentifier] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[ForumMember](
	[ForumIdentifier] [uniqueidentifier] NOT NULL references Forum(Identifier),
	[UserIdentifier] [uniqueidentifier] NOT NULL references [User](Identifier),
PRIMARY KEY NONCLUSTERED 
(
	[ForumIdentifier] ASC,
	[UserIdentifier] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[ForumAdministrators](
	[ForumIdentifier] [uniqueidentifier] NOT NULL references Forum(Identifier),
	[UserIdentifier] [uniqueidentifier] NOT NULL references [User](Identifier),
PRIMARY KEY NONCLUSTERED 
(
	[ForumIdentifier] ASC,
	[UserIdentifier] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

create table [Phone] (
Identifier uniqueidentifier not null,
Created datetime not null,
Modified datetime null,
TagIdentifier uniqueidentifier null references Tag(Identifier),
PhoneNumber nvarchar(50) not null,

primary key nonclustered (Identifier)
)

create table [Country] (
CountryCode nvarchar(3) not null primary key,
Name nvarchar(75) not null,
)

create table [Address] (
Identifier uniqueidentifier not null,
Created datetime not null,
Modified datetime null,
AddressLine1 nvarchar(150) not null,
AddressLine2 nvarchar(150) null,
City nvarchar(150) not null,
TagIdentifier uniqueidentifier null references Tag(Identifier),
PostalCode nvarchar(20) not null,
CountryCode nvarchar(3) references Country(CountryCode)

primary key nonclustered (Identifier)
)

create table [PersonAddress] (
PersonIdentifier uniqueidentifier not null references Person(Identifier),
AddressIdentifier uniqueidentifier not null references [Address](Identifier),

primary key nonclustered (PersonIdentifier, AddressIdentifier)
)

create table [PersonPhone] (
PersonIdentifier uniqueidentifier not null references Person(Identifier),
PhoneIdentifier uniqueidentifier not null references Phone(Identifier),

primary key nonclustered (PersonIdentifier, PhoneIdentifier)
)

create table [OrganizationContact] (
OrganizationIdentifier uniqueidentifier not null references Organization(Identifier),
PersonIdentifier uniqueidentifier not null references Person(Identifier),
TagIdentifier uniqueidentifier null references Tag(Identifier)

primary key nonclustered (OrganizationIdentifier, PersonIdentifier)
)