--queries to insert test user James Hawkins into tables
--DECLARE @testId nvarchar(450);
/*
DECLARE @testEmail1 nvarchar(max) = 'JamesPleiadesHawkins@yahoo.com';
DECLARE @date datetime = getDate();

DECLARE @testId nvarchar(450) = (
	SELECT Id FROM AspNetUsers WHERE Email = @testEmail1  
);

--insert into account
EXECUTE insert_into_account 
	 @UserId = @testId
	,@FirstName = 'James'
	,@LastName = 'Hawkins'
	,@DisplayName = 'TreasurePlanetJim'
	,@Email = @testEmail1
	,@RegisterDate = @date
	,@CreateDate = @date
	--password = 'TreasurePlanet1!'
GO*/

DECLARE @testEmail1 nvarchar(max) = 'JamesPleiadesHawkins@yahoo.com';
DECLARE @accountId int = ( SELECT AccountId FROM Accounts WHERE Email = @testEmail1 );

--insert into images
INSERT INTO [dbo].[Images]
	(AccountId
	,CreateDate
	,Url)
VALUES 
	(@AccountId
	,getDate()
	,'https://pixelartdesigner.blob.core.windows.net/images/umbreon.webp')

--insert into projects
INSERT INTO [dbo].[Projects]
	(AccountId 
	,Name 
	,DisplayTitle 
	,Description 
	,GridSize
	,Data
	,CreateDate)
VALUES
	(@accountId
	,'umbreon.webp'
	,'Chibi Umbreon'
	,'Umbreon in pixel art'
	,'43x48'
	,''
	,getdate())

DECLARE @imageId uniqueIdentifier = ( SELECT ImageId FROM Images WHERE AccountId = @accountId );

--insert into Ratings
INSERT INTO [dbo].[Ratings] 
	(AccountId
	,ImageId
	,CreateDate
	,Rating)
VALUES
	(@accountId
	,@imageId
	,getDate()
	,1)

--test data for James in comments
INSERT INTO Comments
	(AccountId 
	,ImageId 
	,CommentText 
	,CreateDate)
VALUES
	(@accountId 
	,@imageId 
	,'lol I commented on my own picture'
	,getDate())

--insert into settings for James
INSERT INTO Settings
	(AccountId
	,DarkMode
	,NotificationsEnabled
	,Biography
	,CreateDate)
VALUES
	(@accountId 
	,1 --dark mode on
	,1 --notifications on
	,'Likes to solar sail'
	,getDate())

--insert into Pallettes for James
INSERT INTO Pallettes
	(AccountId 
	,HexCodes
	,Name
	,CreateDate)
VALUES
	(@accountId
	,'insert hex code here'
	,'HexPallette Solar Sail'
	,getDate())