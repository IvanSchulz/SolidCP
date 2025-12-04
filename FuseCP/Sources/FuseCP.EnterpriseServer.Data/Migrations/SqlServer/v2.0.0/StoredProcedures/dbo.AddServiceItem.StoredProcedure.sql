SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddServiceItem]
    (
    	@ActorID int,
    	@PackageID int,
    	@ServiceID int,
    	@ItemName nvarchar(500),
    	@ItemTypeName nvarchar(200),
    	@ItemID int OUTPUT,
    	@XmlProperties ntext,
    	@CreatedDate datetime
    )
    AS
    BEGIN TRAN

    -- check rights
    IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
    RAISERROR('You are not allowed to access this package', 16, 1)

    -- get GroupID
    DECLARE @GroupID int
    SELECT
    	@GroupID = PROV.GroupID
    FROM Services AS S
    INNER JOIN Providers AS PROV ON S.ProviderID = PROV.ProviderID
    WHERE S.ServiceID = @ServiceID

    DECLARE @ItemTypeID int
    SELECT @ItemTypeID = ItemTypeID FROM ServiceItemTypes
    WHERE TypeName = @ItemTypeName
    AND ((@GroupID IS NULL) OR (@GroupID IS NOT NULL AND GroupID = @GroupID))

    -- Fix to allow plans assigned to serveradmin
    IF (@ItemTypeName = 'FuseCP.Providers.HostedSolution.Organization, FuseCP.Providers.Base')
    BEGIN
    	IF NOT EXISTS (SELECT * FROM ServiceItems WHERE PackageID = 1)
    	BEGIN
    		INSERT INTO ServiceItems (PackageID, ItemTypeID,ServiceID,ItemName,CreatedDate)
    		VALUES(1, @ItemTypeID, @ServiceID, 'System',  @CreatedDate)

    		DECLARE @TempItemID int

    		SET @TempItemID = SCOPE_IDENTITY()
    		INSERT INTO ExchangeOrganizations (ItemID, OrganizationID)
    		VALUES(@TempItemID, 'System')
    	END
    END

    -- add item
    INSERT INTO ServiceItems
    (
    	PackageID,
    	ServiceID,
    	ItemName,
    	ItemTypeID,
    	CreatedDate
    )
    VALUES
    (
    	@PackageID,
    	@ServiceID,
    	@ItemName,
    	@ItemTypeID,
    	@CreatedDate
    )

    SET @ItemID = SCOPE_IDENTITY()

    DECLARE @idoc int
    --Create an internal representation of the XML document.
    EXEC sp_xml_preparedocument @idoc OUTPUT, @XmlProperties

    -- Execute a SELECT statement that uses the OPENXML rowset provider.
    DELETE FROM ServiceItemProperties
    WHERE ItemID = @ItemID

    CREATE TABLE #TempTable(
    	ItemID int,
    	PropertyName nvarchar(50),
    	PropertyValue  nvarchar(max))

    INSERT INTO #TempTable (ItemID, PropertyName, PropertyValue)
    SELECT
    	@ItemID,
    	PropertyName,
    	PropertyValue
    FROM OPENXML(@idoc, '/properties/property',1) WITH 
    (
    	PropertyName nvarchar(50) '@name',
    	PropertyValue nvarchar(max) '@value'
    ) as PV

    -- Move data from temp table to real table
    INSERT INTO ServiceItemProperties
    (
    	ItemID,
    	PropertyName,
    	PropertyValue
    )
    SELECT 
    	ItemID, 
    	PropertyName, 
    	PropertyValue
    FROM #TempTable

    DROP TABLE #TempTable

    -- remove document
    exec sp_xml_removedocument @idoc

    COMMIT TRAN
    RETURN
GO
