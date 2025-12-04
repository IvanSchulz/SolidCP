/* View UsersDetailed */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[UsersDetailed]
AS
SELECT     U.UserID, U.RoleID, U.StatusID, U.LoginStatusId, U.SubscriberNumber, U.FailedLogins, U.OwnerID, U.Created, U.Changed, U.IsDemo, U.Comments, U.IsPeer, U.Username, U.FirstName, U.LastName, U.Email,
                      U.CompanyName, U.FirstName + ' ' + U.LastName AS FullName, UP.Username AS OwnerUsername, UP.FirstName AS OwnerFirstName,
                      UP.LastName AS OwnerLastName, UP.RoleID AS OwnerRoleID, UP.FirstName + ' ' + UP.LastName AS OwnerFullName, UP.Email AS OwnerEmail, UP.RoleID AS Expr1,
                          (SELECT     COUNT(PackageID) AS Expr1
                            FROM          dbo.Packages AS P
                            WHERE      (UserID = U.UserID)) AS PackagesNumber, U.EcommerceEnabled
FROM         dbo.Users AS U LEFT OUTER JOIN
                      dbo.Users AS UP ON U.OwnerID = UP.UserID
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* Trigger Update_StatusIDchangeDate */
CREATE TRIGGER [dbo].[Update_StatusIDchangeDate]
   ON [dbo].[Packages]
   AFTER UPDATE
AS BEGIN
    UPDATE Packages 
		SET StatusIDchangeDate = GETDATE()

    FROM Packages P 
    INNER JOIN Inserted I ON P.PackageID = I.PackageID
    INNER JOIN Deleted D ON P.PackageID = D.PackageID                  
    WHERE  D.StatusID <> I.StatusID AND I.StatusID > 1 --dont update if nothing change and keep ChangeDate if server back to active  
    --AND P.StatusID <> I.StatusID
END
GO