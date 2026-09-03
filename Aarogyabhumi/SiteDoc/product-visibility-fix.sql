/* ============================================================================
   Pulastya web - products are not showing on the site
   Database: pulstyainv
   ============================================================================

   DIAGNOSIS
   ---------
   There are 44 products with ActiveStatus = 'Y' and OnWebsite = 'Y', and they
   all join correctly to M_SubcatMaster / M_CatMaster. Nothing reaches the site
   because of two separate things:

   1) M_taxMaster is COMPLETELY EMPTY (0 rows).
      Both product queries use an INNER JOIN on it, so every product is dropped:

        ShowProduct  @Action='ShowProductImage'   (product list page)
            Inner join M_taxMaster d on d.prodCode = a.prodid

        V#specialProdductDP                       (view behind the home page)
            Inner Join M_taxMaster b on b.prodCode = a.Prodid

      -> this alone blanks BOTH the product list AND the home page.

   2) The four home-page flags are 'C' on all 95 products, never 'Y'.
      SearchProduct filters on 'Y':

        @Action='ShowSpecialProduct'   -> Where IsForPC    = 'Y'   (We Recommend)
        @Action='ShowTopSellerProduct' -> Where HotSell    = 'Y'   (Best Selling)
        @Action='ShowFeaturedProduct'  -> Where IsFlexible = 'Y'
        @Action='ShowDealsofWeek'      -> Where spcloffer  = 'Y'

      -> even after fixing (1), the home page sections stay empty until some
         products are flagged 'Y'.

   Fix (1) first, confirm the product list fills up, then do (2).
   ============================================================================ */


/* ---------------------------------------------------------------------------
   STEP 0 - confirm the diagnosis before changing anything
   --------------------------------------------------------------------------- */
SELECT 'products live'          AS Check_, COUNT(*) AS Value FROM M_ProductMaster WHERE ActiveStatus='Y' AND OnWebsite='Y'
UNION ALL SELECT 'tax master rows',        COUNT(*) FROM M_taxMaster
UNION ALL SELECT 'rows the list returns',  COUNT(*) FROM M_ProductMaster a
              INNER JOIN M_taxMaster d ON d.prodCode = a.ProdId
              WHERE a.ActiveStatus='Y' AND a.OnWebsite='Y'
UNION ALL SELECT 'rows the home view has', COUNT(*) FROM [V#specialProdductDP];
GO


/* ---------------------------------------------------------------------------
   STEP 1 - give every live product a tax row.

   >>> SET @Gst TO THE CORRECT RATE BEFORE RUNNING. <<<
   0 is used below only so nothing is invented; if products carry different
   GST rates, insert them per product instead of in one statement.
   --------------------------------------------------------------------------- */
DECLARE @Gst numeric(18,2) = 0;      -- <-- change me (e.g. 5, 12, 18)

INSERT INTO M_taxMaster
      (AId, GenerateBy, ProdCode, ProdName, StateCode, VatTax, STax, CstTax,
       WithCForm, AValue, ActiveStatus, Remarks, Company, Imported, UserId, LastModified)
SELECT ROW_NUMBER() OVER (ORDER BY a.ProdId) + ISNULL((SELECT MAX(AId) FROM M_taxMaster), 0),
       'SYSTEM', CONVERT(varchar(50), a.ProdId), a.ProductName, 0, @Gst, 0, -1,
       0, 0, 'Y', 'seeded for website visibility', '', 'N', 0, CONVERT(varchar(30), GETDATE(), 120)
FROM   M_ProductMaster a
WHERE  a.ActiveStatus = 'Y'
  AND  a.OnWebsite    = 'Y'
  AND  NOT EXISTS (SELECT 1 FROM M_taxMaster t WHERE t.ProdCode = CONVERT(varchar(50), a.ProdId));
GO

-- the product list should now have rows
SELECT COUNT(*) AS list_rows_now FROM M_ProductMaster a
INNER JOIN M_taxMaster d ON d.prodCode = a.ProdId
WHERE a.ActiveStatus='Y' AND a.OnWebsite='Y';
GO


/* ---------------------------------------------------------------------------
   STEP 2 - choose which products appear in each home-page section.
   Replace the ProdId lists with the ones you actually want to feature.
   --------------------------------------------------------------------------- */

-- "We Recommend"
UPDATE M_ProductMaster SET IsForPC = 'Y'
WHERE ProdId IN (56, 57, 58, 59, 60, 61);

-- "Our Best Selling Products"
UPDATE M_ProductMaster SET HotSell = 'Y'
WHERE ProdId IN (62, 63, 64, 65, 70, 71);
GO


/* ---------------------------------------------------------------------------
   STEP 3 - verify what the website will now show
   --------------------------------------------------------------------------- */
EXEC ShowProduct   @Action = 'ShowProductImage';    -- product list page
EXEC SearchProduct @Action = 'ShowSpecialProduct';  -- home: We Recommend
EXEC SearchProduct @Action = 'ShowTopSellerProduct';-- home: Best Selling
GO


/* ---------------------------------------------------------------------------
   NOTE - not blocking, but worth cleaning up one day:
   ShowProduct still builds image URLs with the old host,
       'https://franchise.myhemalika.com/ProductImages/' + ImagePath
   while V#specialProdductDP was already updated to franchise.pulastyaglobals.in.
   The website re-hosts these at runtime from the FranchiseUrl key in Web.config,
   so images work either way - but the SP text is inconsistent with the view.
   --------------------------------------------------------------------------- */
