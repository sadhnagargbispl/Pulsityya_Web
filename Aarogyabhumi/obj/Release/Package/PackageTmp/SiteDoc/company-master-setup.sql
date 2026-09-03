/* ============================================================================
   Pulastya web - drive all company details from m_companymaster
   Run against database:  pulstyainv
   Safe to re-run: every step checks before it changes anything.
   ============================================================================ */

/* ---------------------------------------------------------------------------
   STEP 1 - add the columns the site reads but the table does not have yet.
   Column names must match exactly; the app reads them by these names.
   --------------------------------------------------------------------------- */
IF COL_LENGTH('m_companymaster','CompLogo')         IS NULL ALTER TABLE m_companymaster ADD CompLogo         varchar(255) NULL;
IF COL_LENGTH('m_companymaster','CompTagline')      IS NULL ALTER TABLE m_companymaster ADD CompTagline      varchar(200) NULL;
IF COL_LENGTH('m_companymaster','CompAboutUs')      IS NULL ALTER TABLE m_companymaster ADD CompAboutUs      varchar(1000) NULL;
IF COL_LENGTH('m_companymaster','CompWorkingHours') IS NULL ALTER TABLE m_companymaster ADD CompWorkingHours varchar(200) NULL;
IF COL_LENGTH('m_companymaster','FreeShipAmount')   IS NULL ALTER TABLE m_companymaster ADD FreeShipAmount   varchar(20)  NULL;
IF COL_LENGTH('m_companymaster','FacebookUrl')      IS NULL ALTER TABLE m_companymaster ADD FacebookUrl      varchar(255) NULL;
IF COL_LENGTH('m_companymaster','InstagramUrl')     IS NULL ALTER TABLE m_companymaster ADD InstagramUrl     varchar(255) NULL;
IF COL_LENGTH('m_companymaster','TwitterUrl')       IS NULL ALTER TABLE m_companymaster ADD TwitterUrl       varchar(255) NULL;
IF COL_LENGTH('m_companymaster','YoutubeUrl')       IS NULL ALTER TABLE m_companymaster ADD YoutubeUrl       varchar(255) NULL;
IF COL_LENGTH('m_companymaster','LinkedInUrl')      IS NULL ALTER TABLE m_companymaster ADD LinkedInUrl      varchar(255) NULL;
IF COL_LENGTH('m_companymaster','CompGSTNo')        IS NULL ALTER TABLE m_companymaster ADD CompGSTNo        varchar(50)  NULL;
GO

/* ---------------------------------------------------------------------------
   STEP 2 - THIS IS THE IMPORTANT ONE.
   Sp_GetCompanydetail currently lists its 34 columns one by one, so any new
   column is invisible to the website even after STEP 1. Switching it to
   SELECT * means every column you add from now on flows through automatically
   and you never have to touch this procedure again.
   --------------------------------------------------------------------------- */
ALTER PROCEDURE Sp_GetCompanydetail
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *,
           REPLACE(ContactNo, '?', '') AS ContactNoClean
    FROM   M_CompanyMaster;
END
GO

/* ---------------------------------------------------------------------------
   STEP 3 - fill in the values. Replace the text below with the real details.
   Leave a column NULL / blank and the site keeps its current built-in default,
   so you can fill them in one at a time.
   --------------------------------------------------------------------------- */
UPDATE m_companymaster
SET
    -- already present, shown here so everything is in one place
    CompName         = 'PULASTYA GLOBALS INDIA PRIVATE LIMITED',
    CompAdd          = 'E-17, Ground Floor, Green Park, New Delhi, South West Delhi - 110016',
    CompCity         = 'New Delhi',
    ContactNo        = '+91 76686 23064',          -- currently EMPTY: footer/contact phone is blank without this
    CompMail         = 'info@pulastyaglobals.com', -- currently EMPTY: footer/contact email is blank without this
    WebSite          = 'https://www.pulastyaglobals.in/',

    -- new
    CompLogo         = NULL,   -- full URL, /path, or just a file name uploaded in admin. NULL = use the theme logo
    CompTagline      = 'A Better Tomorrow Together',
    CompAboutUs      = 'At Pulastya Globals India, we believe that a healthier lifestyle begins with informed choices and access to reliable products. Our focus is on offering carefully developed health and wellness solutions while continuously working towards improving product quality, customer experience and distributor support.',
    CompWorkingHours = 'Mon-Sat : 09:00AM to 08:00PM, Sunday : Close',
    FreeShipAmount   = '2500',
    FacebookUrl      = 'https://www.facebook.com/profile.php?id=61554688337187',
    InstagramUrl     = 'https://www.instagram.com/pulastyaglobals_india',
    TwitterUrl       = 'https://x.com/pulastya4614321',
    YoutubeUrl       = 'https://www.youtube.com/@PulastyaGlobalsIndiaPrivateLtd',
    LinkedInUrl      = 'https://www.linkedin.com',
    CompGSTNo        = NULL;
GO

/* ---------------------------------------------------------------------------
   STEP 4 - check what the website will now receive
   --------------------------------------------------------------------------- */
EXEC Sp_GetCompanydetail;
GO
