--queries to insert test user Manuel Herrera into tables
--DECLARE @testId nvarchar(450);
DECLARE @testEmail nvarchar(max) = 'porque42@hotmail.com';
DECLARE @date datetime = getDate();

DECLARE @testId nvarchar(450) = (
	SELECT Id FROM AspNetUsers WHERE Email = @testEmail
);

--insert into account
EXECUTE [dbo].[insert_into_account]
	 @UserId = @testid
	,@FirstName = 'Manuel'
    ,@LastName = 'Herrera'
    ,@DisplayName = 'synthesis22'
    ,@Email = @testEmail
	--password = 'Synthesis1!'
    ,@RegisterDate = @date
	,@CreateDate = @date
GO 

DECLARE @testEmail nvarchar(max) = 'porque42@hotmail.com';;
DECLARE @accountId int = ( SELECT AccountId FROM Accounts WHERE Email = @testEmail );

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
	,'Rubber_Duck.webp'
	,'rubberduck'
	,'DUCK'
	,'250x250'
	,''
	,getdate())

DECLARE @projectId int = ( SELECT ProjectId FROM Projects WHERE Name = 'Rubber_Duck.webp' AND AccountId = @accountId);
DECLARE @date datetime = getDate();

--insert into images
EXECUTE [dbo].[insert_into_images]
	@AccountId = @accountId
	,@ProjectId = @projectId
	,@CreateDate = @date
	,@Url = 'https://pixelartdesigner.blob.core.windows.net/images/Rubber_Duck.webp'
GO

DECLARE @testEmail nvarchar(max) = 'porque42@hotmail.com';;
DECLARE @accountId int = ( SELECT AccountId FROM Accounts WHERE Email = @testEmail );
DECLARE @imageId uniqueIdentifier = ( SELECT ImageId FROM Images WHERE AccountId = @accountId );

SET @imageId = ( SELECT ImageId FROM Images WHERE Url = 'https://pixelartdesigner.blob.core.windows.net/images/weber-cat.webp' );

--test data for Manuel in comments
INSERT INTO Comments
	(AccountId 
	,ImageId 
	,CommentText 
	,CreateDate)
VALUES
	(@accountId 
	,@imageId 
	,'Such college pride. I like it.'
	,getDate())

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

--insert into settings for Manuel
INSERT INTO Settings
	(AccountId
	,DarkMode
	,NotificationsEnabled
	,Biography
	,CreateDate)
VALUES
	(@accountId 
	,1 --dark mode on
	,0 --notifications off
	,'Professor of Natural Science at Rubber Duck University'
	,getDate())

--insert into Pallettes for Manuel
INSERT INTO Pallettes
	(AccountId 
	,HexCodes
	,Name
	,CreateDate)
VALUES
	(@accountId
	,'insert hex code here'
	,'Rubber Duck palette'
	,getDate())

INSERT INTO Pallettes
	(AccountId
	,HexCodes
	,Name
	,CreateDate)
VALUES
	(@accountId
	,'insert hex code here'
	,'Test color palette'
	,getDate())