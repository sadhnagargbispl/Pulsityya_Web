/* ============================================================================
   Pulastya Globals website - company details, logo and home banners
   Database: pulstyainv  (Web.config sqlConn, login usrpulstya)
   Prepared 2026-09-10. Safe to re-run.

   Already in place on pulstyainv (no action needed):
     - optional columns CompLogo, CompTagline, CompAboutUs, CompWorkingHours,
       FreeShipAmount, FacebookUrl, InstagramUrl, TwitterUrl, YoutubeUrl,
       LinkedInUrl, CompGSTNo
     - Sp_GetCompanydetail uses SELECT *, so logourl and the new columns reach
       the website (the site strips '?' from ContactNo itself)
   The website reads: the logo from logourl, home banners from
   tblBannerMaster (IsActive = 1) + TblBannerDetail (ImagePath, SeqNo).
   ImagePath / logourl may be a full URL, a site path starting with ~/ , or a
   path on the ImageUrl upload server (Web.config).
   ============================================================================ */

USE pulstyainv;
GO

/* ---------------------------------------------------------------------------
   STEP 1 - demo banners, demo logo, and the blank / dummy company details
   (values from the Pulastya demo footer). One transaction.
   Name, address, website, CompTitle and MobileNo are NOT touched.
   --------------------------------------------------------------------------- */
SET XACT_ABORT ON;
BEGIN TRAN;

-- demo hero banners (files are part of the site under ~/theme/img),
-- attached to the active banner master 3 (it had no images)
INSERT INTO TblBannerDetail (BannerId, ImagePath, CreatedOn, SeqNo, Url)
SELECT 3, v.ImagePath, CAST(GETDATE() AS date), v.SeqNo, NULL
FROM (VALUES ('~/theme/img/hero-banner-01.jpg', 1),
             ('~/theme/img/hero-banner-02.jpg', 2),
             ('~/theme/img/hero-banner-03.jpg', 3)) v(ImagePath, SeqNo)
WHERE NOT EXISTS (SELECT 1 FROM TblBannerDetail d WHERE d.ImagePath = v.ImagePath);

UPDATE M_CompanyMaster SET
    logourl          = '~/theme/img/logo.png',
    ContactNo        = CASE WHEN ISNULL(ContactNo,'') IN ('', '1234567890') THEN '+91 76686 23064' ELSE ContactNo END,
    CompMail         = CASE WHEN ISNULL(CompMail,'') = '' THEN 'info@pulastyaglobals.com' ELSE CompMail END,
    CompCity         = CASE WHEN ISNULL(CompCity,'') IN ('', 'West Punjabi Bagh') THEN 'New Delhi' ELSE CompCity END,
    CompTagline      = CASE WHEN ISNULL(CompTagline,'') = '' THEN 'A Better Tomorrow Together' ELSE CompTagline END,
    CompAboutUs      = CASE WHEN ISNULL(CompAboutUs,'') = '' THEN 'At Pulastya Globals India, we believe that a healthier lifestyle begins with informed choices and access to reliable products. Our focus is on offering carefully developed health and wellness solutions while continuously working towards improving product quality, customer experience and distributor support.' ELSE CompAboutUs END,
    CompWorkingHours = CASE WHEN ISNULL(CompWorkingHours,'') = '' THEN 'Mon-Sat : 09:00AM to 08:00PM, Sunday : Close' ELSE CompWorkingHours END,
    FreeShipAmount   = CASE WHEN ISNULL(FreeShipAmount,'') = '' THEN '2500' ELSE FreeShipAmount END,
    FacebookUrl      = CASE WHEN ISNULL(FacebookUrl,'') = '' THEN 'https://www.facebook.com/profile.php?id=61554688337187' ELSE FacebookUrl END,
    InstagramUrl     = CASE WHEN ISNULL(InstagramUrl,'') = '' THEN 'https://www.instagram.com/pulastyaglobals_india' ELSE InstagramUrl END,
    TwitterUrl       = CASE WHEN ISNULL(TwitterUrl,'') = '' THEN 'https://x.com/pulastya4614321' ELSE TwitterUrl END,
    YoutubeUrl       = CASE WHEN ISNULL(YoutubeUrl,'') = '' THEN 'https://www.youtube.com/@PulastyaGlobalsIndiaPrivateLtd' ELSE YoutubeUrl END;

COMMIT;
GO

/* ---------------------------------------------------------------------------
   STEP 2 - check what the website will receive
   --------------------------------------------------------------------------- */
EXEC ShowBanner @Action = 'DisplayBanner', @BannerCatId = 1;
SELECT CompName, CompTitle, CompAdd, CompCity, ContactNo, CompMail, WebSite, logourl,
       CompTagline, CompWorkingHours, FreeShipAmount, FacebookUrl, InstagramUrl, TwitterUrl, YoutubeUrl
FROM   M_CompanyMaster;
GO

/* ---------------------------------------------------------------------------
   UNDO for STEP 1 (values before it: ContactNo '1234567890', CompMail empty,
   CompCity 'West Punjabi Bagh', logourl 'https://mlm.bisplindia.in/images/Logo.png',
   all other columns empty):

DELETE FROM TblBannerDetail WHERE ImagePath LIKE '~/theme/img/hero-banner-0%';
UPDATE M_CompanyMaster SET
    logourl = 'https://mlm.bisplindia.in/images/Logo.png', ContactNo = '1234567890', CompMail = '',
    CompCity = 'West Punjabi Bagh', CompTagline = NULL, CompAboutUs = NULL, CompWorkingHours = NULL,
    FreeShipAmount = NULL, FacebookUrl = NULL, InstagramUrl = NULL, TwitterUrl = NULL, YoutubeUrl = NULL;
   --------------------------------------------------------------------------- */

/* ---------------------------------------------------------------------------
   NOTE - darju9inv is Darju's database, NOT this site. It was changed by
   mistake on 2026-09-10 while Web.config still pointed there:
     - 10 empty columns added to M_CompanyMaster (CompTagline ... CompGSTNo)
     - Sp_GetCompanydetail: logourl and those columns appended to its SELECT list
       (all original columns unchanged, same order)
   To put Darju's original procedure back, run this on darju9inv:

USE darju9inv;
GO
ALTER procedure Sp_GetCompanydetail
AS
BEGIN
select CId,CompId,CompName,CompAdd,CompState,CompTinNo,CompSTaxNo,
CompPrefix,REPLACE(ContactNo, '?', '') AS ContactNo,MobileNo,WebSite,ActiveStatus,RecTimeStamp,
LastModified,UserCode,UserId,smsSenderId,smsUserNm,smPass,CompMail,
CompTitle,CompCSTNo,CompPANNo,MailHost,MailPass,CompRegOffAdd,CompTerm,CompanyIDNo,
MsgOnInvoice,WebPortal,IsSendSMS,FTPUserNm,FTPPassw,CompCity from M_CompanyMaster
END
GO
   --------------------------------------------------------------------------- */
