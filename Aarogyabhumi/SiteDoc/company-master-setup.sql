/* ============================================================================
   Drive all company details on the website from m_companymaster
   Run against the database in Web.config (currently: darju9inv)
   Safe to re-run: every step checks before it changes anything.
   Name, address, email, phone and the LOGO (existing column logourl) come from
   columns that already exist; this script only adds the optional extra details.
   STEP 1 + STEP 2 were run on darju9inv on 2026-09-10.
   ============================================================================ */

/* ---------------------------------------------------------------------------
   STEP 1 - add the optional columns the site reads (skipped if they exist).
   Column names must match exactly; the app reads them by these names.
   The logo does NOT need a new column: the site uses the existing logourl.
   --------------------------------------------------------------------------- */
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
   STEP 2 - make Sp_GetCompanydetail return logourl and the new columns.
   The original column list is kept exactly (ContactNo stays cleaned of '?'
   through the alias); the new columns are appended at the end.
   (SELECT * is NOT used: it would bring back the raw ContactNo with '?'.)
   --------------------------------------------------------------------------- */
ALTER PROCEDURE Sp_GetCompanydetail
AS
BEGIN
select CId,CompId,CompName,CompAdd,CompState,CompTinNo,CompSTaxNo,
CompPrefix,REPLACE(ContactNo, '?', '') AS ContactNo,MobileNo,WebSite,ActiveStatus,RecTimeStamp,
LastModified,UserCode,UserId,smsSenderId,smsUserNm,smPass,CompMail,
CompTitle,CompCSTNo,CompPANNo,MailHost,MailPass,CompRegOffAdd,CompTerm,CompanyIDNo,
MsgOnInvoice,WebPortal,IsSendSMS,FTPUserNm,FTPPassw,CompCity,
logourl,CompTagline,CompAboutUs,CompWorkingHours,FreeShipAmount,
FacebookUrl,InstagramUrl,TwitterUrl,YoutubeUrl,LinkedInUrl,CompGSTNo
from M_CompanyMaster
END
GO

/* ---------------------------------------------------------------------------
   STEP 3 - TEMPLATE (commented out so it never runs by accident).
   Put THIS company's real values in place of NULL, then run only this UPDATE.
   Running it with NULLs would clear values that were already filled in.
   A column left NULL / blank simply hides that item on the website.

UPDATE m_companymaster
SET
    CompTagline      = NULL,   -- shown in the page title / about page
    CompAboutUs      = NULL,   -- short about text: footer, home page, about page
    CompWorkingHours = NULL,   -- e.g. 'Mon-Sat : 09:00AM to 08:00PM, Sunday : Close'
    FreeShipAmount   = NULL,   -- e.g. '2500' -> header shows "Free shipping on order above Rs 2500"
    FacebookUrl      = NULL,
    InstagramUrl     = NULL,
    TwitterUrl       = NULL,
    YoutubeUrl       = NULL,
    LinkedInUrl      = NULL,
    CompGSTNo        = NULL;

-- LOGO (header, footer, favicon, share image): full URL, /path, or a file name
-- on the ImageUrl server (Web.config). Empty = company name shown as text.
UPDATE m_companymaster SET logourl = 'your-logo.png';
   --------------------------------------------------------------------------- */

/* ---------------------------------------------------------------------------
   STEP 4 - check what the website receives
   --------------------------------------------------------------------------- */
EXEC Sp_GetCompanydetail;
GO

/* ---------------------------------------------------------------------------
   Original procedure before STEP 2 (to restore, run it as ALTER PROCEDURE):

CREATE procedure  Sp_GetCompanydetail
AS
BEGIN
select CId,CompId,CompName,CompAdd,CompState,CompTinNo,CompSTaxNo,
CompPrefix,REPLACE(ContactNo, '?', '') AS ContactNo,MobileNo,WebSite,ActiveStatus,RecTimeStamp,
LastModified,UserCode,UserId,smsSenderId,smsUserNm,smPass,CompMail,
CompTitle,CompCSTNo,CompPANNo,MailHost,MailPass,CompRegOffAdd,CompTerm,CompanyIDNo,
MsgOnInvoice,WebPortal,IsSendSMS,FTPUserNm,FTPPassw,CompCity from M_CompanyMaster
END
   --------------------------------------------------------------------------- */
