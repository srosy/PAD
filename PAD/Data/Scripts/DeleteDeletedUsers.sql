--Deletes all records from tables (Pallettes, Favorites, Projects, Settings, and Accounts) where DeleteDate is over a week.
CREATE PROCEDURE [dbo].[clean_up_deleted_accounts]
AS 
	--parameters not needed here
BEGIN
	SELECT a.AccountId INTO #AccountToDelete
	FROM Accounts a INNER JOIN Pallettes pal ON a.AccountId = pal.AccountId
	INNER JOIN Settings s ON a.AccountId = s.AccountId
	INNER JOIN Projects prj ON a.AccountId = prj.AccountId
	INNER JOIN Favorites f ON a.AccountId = f.AccountId
	WHERE a.DeleteDate > (DATEADD(DD, -7, getDate()));

	DELETE FROM [Accounts] WHERE AccountId IN (SELECT AccountId FROM #AccountToDelete);
	DELETE FROM [Settings] WHERE AccountId IN (SELECT AccountId FROM #AccountToDelete);
	DELETE FROM [Pallettes] WHERE AccountId IN (SELECT AccountId FROM #AccountToDelete);
	DELETE FROM [Favorites] WHERE AccountId IN (SELECT AccountId FROM #AccountToDelete);
	DELETE FROM [Projects] WHERE AccountId IN (SELECT AccountId FROM #AccountToDelete);
END
GO