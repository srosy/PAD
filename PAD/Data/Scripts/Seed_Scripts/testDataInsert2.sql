--queries to insert test user James Hawkins into tables
--DECLARE @testId nvarchar(450);
DECLARE @testEmail nvarchar(max) = 'YesImANerd@gmail.com';
DECLARE @date datetime = getDate();

DECLARE @testId nvarchar(450) = (
	SELECT Id FROM AspNetUsers WHERE Email = @testEmail
);

--insert into account
EXECUTE [dbo].[insert_into_account]
	 @UserId = @testid
	,@FirstName = 'Jiwoo'
    ,@LastName = 'Kim'
    ,@DisplayName = 'Webt00nReader4000'
    ,@Email = @testEmail
	--password = 'Eleceed1!'
    ,@RegisterDate = @date
	,@CreateDate = @date
GO 

DECLARE @testEmail nvarchar(max) = 'YesImANerd@gmail.com';;
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
	,'weber-cat.webp'
	,'WildCat Mascot'
	,'Um, I thought it would be cool to have the mascot in pixel art...'
	,''
	,''
	,getdate())

DECLARE @projectId int = ( SELECT ProjectId FROM Projects WHERE Name = 'weber-cat.webp' AND AccountId = @accountId);
DECLARE @date datetime = getDate();

--insert into images
EXECUTE [dbo].[insert_into_images]
	@AccountId = @accountId
	,@ProjectId = @projectId
	,@CreateDate = @date
	,@Url = 'https://pixelartdesigner.blob.core.windows.net/images/weber-cat.webp'
GO

DECLARE @testEmail nvarchar(max) = 'YesImANerd@gmail.com';;
DECLARE @accountId int = ( SELECT AccountId FROM Accounts WHERE Email = @testEmail );
DECLARE @imageId uniqueIdentifier = ( SELECT ImageId FROM Images WHERE AccountId = @accountId );

SET @imageId = ( SELECT ImageId FROM Images WHERE Url = 'https://pixelartdesigner.blob.core.windows.net/images/umbreon.webp' );

--test data for Jiwoo in comments
INSERT INTO Comments
	(AccountId 
	,ImageId 
	,CommentText 
	,CreateDate)
VALUES
	(@accountId 
	,@imageId 
	,'So cute!'
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

--insert into settings for Jiwoo
INSERT INTO Settings
	(AccountId
	,DarkMode
	,NotificationsEnabled
	,Biography
	,CreateDate)
VALUES
	(@accountId 
	,0 --dark mode off
	,1 --notifications on
	,'College student, likes to make friends'
	,getDate())

--insert into Pallettes for Jiwoo
INSERT INTO Pallettes
	(AccountId 
	,HexCodes
	,Name
	,CreateDate)
VALUES
	(@accountId
	,'insert hex code here'
	,'Cats!!!!!!!'
	,getDate())