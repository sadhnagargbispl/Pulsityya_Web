/* ============================================================================
   Pulastya web - wishlist (heart on product cards + Wishlist page)
   Database: pulstyainv
   ============================================================================

   DIAGNOSIS
   ---------
   The heart calls /ProductDetail/SaveToWishlist and the Wishlist page calls
   /ViewCart/Wishlist. Both run Sp_SaveShoppingWishlist, which does not exist
   in pulstyainv (there is no wishlist table either):

       Could not find stored procedure 'Sp_SaveShoppingWishlist'.

   SaveToWishlist swallows that error and answers "Something went wrong";
   the Wishlist page has no catch, so it shows the error page.

   WHAT THE WEBSITE SENDS / EXPECTS  (Repository\R_Product.cs)
   ---------------------------------
       @UserID    = member FormNo (Session["FormNo"])
       @ProductID = M_ProductMaster.ProdId

       @Action = 'Save'              toggle - add if missing, remove if present.
                                     One row, column msg = 'Save' or 'Remove'.
                                     No row = the site says "Something went wrong".
       @Action = 'CheckProductwise'  is this product in the member's wishlist?
       @Action = 'CheckUserwise'     the member's wishlist, mapped onto
                                     Entity\E_CartDetails. Column names must match
                                     the property names exactly (case too) and
                                     carry the same type, or the value stays blank.

   imagePath is returned as the file name stored in M_ProductMaster; the
   website builds the full URL from the ProductImageUrl key in Web.config.

   Safe to run more than once.
   ============================================================================ */


/* ---------------------------------------------------------------------------
   STEP 1 - wishlist table.
   Same layout as M_Wishlist in the other shop databases, except Productid is
   varchar(100) to match M_ProductMaster.ProdId.
   --------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.M_Wishlist', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.M_Wishlist
    (
        Aid          numeric(18, 0) IDENTITY(1, 1) NOT NULL CONSTRAINT PK_M_Wishlist PRIMARY KEY,
        Formno       numeric(18, 0) NOT NULL,
        Productid    varchar(100)   NOT NULL,
        ProductName  varchar(500)   NOT NULL CONSTRAINT DF_M_Wishlist_ProductName  DEFAULT (''),
        Qty          numeric(18, 0) NOT NULL CONSTRAINT DF_M_Wishlist_Qty          DEFAULT (1),
        RectimeStamp datetime       NOT NULL CONSTRAINT DF_M_Wishlist_RectimeStamp DEFAULT (GETDATE())
    );

    CREATE UNIQUE INDEX UX_M_Wishlist_Formno_Productid ON dbo.M_Wishlist (Formno, Productid);
END
GO


/* ---------------------------------------------------------------------------
   STEP 2 - the procedure the website calls.
   --------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.Sp_SaveShoppingWishlist', 'P') IS NOT NULL
    DROP PROCEDURE dbo.Sp_SaveShoppingWishlist;
GO

CREATE PROCEDURE dbo.Sp_SaveShoppingWishlist
    @Action    varchar(50),
    @UserID    int,
    @ProductID int
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ProdId varchar(100);
    SET @ProdId = CONVERT(varchar(100), @ProductID);

    -- no member id = session expired; return nothing rather than save against 0
    IF ISNULL(@UserID, 0) <= 0
        RETURN;

    IF @Action = 'Save'
    BEGIN
        IF EXISTS (SELECT 1 FROM dbo.M_Wishlist WHERE Formno = @UserID AND Productid = @ProdId)
        BEGIN
            DELETE FROM dbo.M_Wishlist WHERE Formno = @UserID AND Productid = @ProdId;
            SELECT 'Remove' AS msg;
        END
        ELSE
        BEGIN
            INSERT INTO dbo.M_Wishlist (Formno, Productid, ProductName, Qty, RectimeStamp)
            SELECT @UserID, p.ProdId, p.ProductName, 1, GETDATE()
            FROM   dbo.M_ProductMaster p
            WHERE  p.ProdId = @ProdId;

            IF @@ROWCOUNT > 0
                SELECT 'Save' AS msg;      -- unknown product: no row
        END
        RETURN;
    END

    IF @Action = 'CheckProductwise'
    BEGIN
        SELECT CASE WHEN EXISTS (SELECT 1 FROM dbo.M_Wishlist
                                 WHERE Formno = @UserID AND Productid = @ProdId)
                    THEN 'Y' ELSE 'N' END AS msg;
        RETURN;
    END

    IF @Action = 'CheckUserwise'
    BEGIN
        SELECT CONVERT(int, w.Aid)                       AS WishlistID,
               CONVERT(int, w.Aid)                       AS id,
               p.ProdId                                  AS ProdId,
               p.ProductName                             AS ProdName,
               p.ImagePath                               AS imagePath,
               CONVERT(decimal(18, 2), p.Dp)             AS prodprice,
               CONVERT(decimal(18, 2), p.Dp)             AS Price,
               CONVERT(decimal(18, 2), p.MRP)            AS MRP,
               CONVERT(decimal(18, 2), p.BV)             AS bv,
               CONVERT(int, p.PV)                        AS PV,
               CONVERT(decimal(18, 2), ISNULL(s.Qty, 0)) AS Stockqty
        FROM   dbo.M_Wishlist w
        INNER JOIN dbo.M_ProductMaster p ON p.ProdId = w.Productid
        LEFT JOIN (SELECT ProdId, SUM(ISNULL(Qty, 0)) AS Qty
                   FROM   dbo.IM_CurrentStock
                   WHERE  FCode = 'WR'
                   GROUP BY ProdId) s ON s.ProdId = p.ProdId
        WHERE  w.Formno = @UserID
          AND  p.ActiveStatus = 'Y'
          AND  p.OnWebSite = 'Y'
        ORDER BY w.RectimeStamp DESC;
        RETURN;
    END
END
GO


/* ---------------------------------------------------------------------------
   STEP 3 - optional check. Put a real member FormNo and ProdId, then run
   these one by one (the first adds the product, the last removes it again).
   --------------------------------------------------------------------------- */
-- EXEC dbo.Sp_SaveShoppingWishlist @Action = 'Save',             @UserID = 0, @ProductID = 56;  -- msg = Save
-- EXEC dbo.Sp_SaveShoppingWishlist @Action = 'CheckUserwise',    @UserID = 0, @ProductID = 0;   -- 1 row
-- EXEC dbo.Sp_SaveShoppingWishlist @Action = 'CheckProductwise', @UserID = 0, @ProductID = 56;  -- msg = Y
-- EXEC dbo.Sp_SaveShoppingWishlist @Action = 'Save',             @UserID = 0, @ProductID = 56;  -- msg = Remove
