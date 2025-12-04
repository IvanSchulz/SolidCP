SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
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