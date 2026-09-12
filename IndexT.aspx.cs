using AjaxControlToolkit;
using DocumentFormat.OpenXml.VariantTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class IndexT : System.Web.UI.Page
{
    DAL Obj;
    DataSet Ds;
    clsGeneral objGen = new clsGeneral();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                Session["TransID"] = DateTime.Now.ToString("yyyyMMddhhmmssfff");

                if (!Page.IsPostBack)
                {
                    Referalreq referalreq = new Referalreq();
                    referalreq.islogin = "N";
                    referalreq.reqtype = "referrallink";
                    referalreq.userid = Convert.ToString(Session["IDNo"]);
                    referalreq.passwd = Convert.ToString(Session["MemPassw"]);
                    // Request ko JSON me convert karna
                    string detail = JsonConvert.SerializeObject(referalreq);
                    // API Call
                    string response = CallPostFunction(detail, "https://d9cpanel.bisplindia.in/ProcessAPIWithK.aspx");
                    // Response ko Object me convert karna
                    Referalres referalres = JsonConvert.DeserializeObject<Referalres>(response);
                    if (referalres != null && referalres.response == "OK")
                    {
                        string leftLink = referalres.urlLeft;
                       string rightLink = referalres.urlright;
                        // ---------- Existing profile-card buttons (LEFT link) ----------
                        hfLeftLink.Value = leftLink;
                        registrationlink.HRef = "https://d9.bisplindia.in/Account/SignUp";
                        // ---------- New LEFT card buttons ----------
                        hfLeftLink.Value = leftLink;   // same hidden field, copy button isse uthayega
                        aLeftFacebook.HRef = "https://www.facebook.com/sharer/sharer.php?u=" + leftLink;
                        aLeftTwitter.HRef = "https://twitter.com/intent/tweet?text=Join+Me+%26+Earn+Income&url=" + leftLink;
                        aLeftWhatsapp.HRef = "https://wa.me/?text=" + leftLink;
                        aLeftTelegram.HRef = "https://telegram.me/share/url?url=" + leftLink + "&text=Join+Me+%26+Earn+Income";
                        // ---------- New RIGHT card buttons ----------
                        hfRightLink.Value = rightLink;
                        aRightFacebook.HRef = "https://www.facebook.com/sharer/sharer.php?u=" + rightLink;
                        aRightTwitter.HRef = "https://twitter.com/intent/tweet?text=Join+Me+%26+Earn+Income&url=" + rightLink;
                        aRightWhatsapp.HRef = "https://wa.me/?text=" + rightLink;
                        aRightTelegram.HRef = "https://telegram.me/share/url?url=" + rightLink + "&text=Join+Me+%26+Earn+Income";
                    }
                    
                    DivNews.Attributes["class"] = "col-lg-12";
                    DivMyKYCStatus.Visible = true;
                    divTotalBusiness.Visible = true;
                    DivPurchaseVolume.Visible = false;
                    GetDashboardIncome();
                    GetTotalDirects();
                    LoadTeam();
                    Obj = new DAL();
                    FillKYCStatus();
                    divwithdrawal.Visible = true;
                    BindWithdrawalStatus();
                    //FillPPVStatus();
                }
            }
            else
            {
                Response.Redirect("logout.aspx");
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void BindWithdrawalStatus()
    {
        DataTable dt = GetWithdrawalCounts();

        foreach (DataRow row in dt.Rows)
        {
            row["StatusHtml"] = GetStatusHtml(
                row["Status"].ToString(), row["TotalAmount"].ToString());
        }

        Repeater1.DataSource = dt;
        Repeater1.DataBind();
    }
    private DataTable GetWithdrawalCounts()
    {
        // Saare 3 status hamesha dikhenge, chahe count 0 ho.
        // JOIN trim/case-safe hai taaki DB me extra space ya case diff ho to bhi match ho.
        string sql = @"
SELECT 
    s.Status,
    ISNULL(SUM(w.Netamount), 0) AS TotalAmount
FROM (VALUES ('A'), ('P'), ('R')) AS s(Status)
LEFT JOIN (
    SELECT 
        LTRIM(RTRIM(Status)) AS Status,
        SUM(Netamount) AS Netamount
    FROM fundwithdrawls
    WHERE FormNo = @FormNo
    GROUP BY LTRIM(RTRIM(Status))
) w ON LOWER(w.Status) = LOWER(s.Status)
GROUP BY s.Status
ORDER BY s.Status";

        DataTable dt = new DataTable();

        string cs = HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString();

        using (SqlConnection con = new SqlConnection(cs))
        using (SqlCommand cmd = new SqlCommand(sql, con))
        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            cmd.Parameters.AddWithValue("@FormNo", Session["FormNo"].ToString());

            da.Fill(dt);
        }

        if (!dt.Columns.Contains("StatusHtml"))
            dt.Columns.Add("StatusHtml", typeof(string));



        return dt;
    }
    public static string GetStatusHtml(string status, string count)
    {
        string cssClass, icon, label;

        switch ((status ?? "").Trim().ToUpperInvariant())
        {
            case "A":
                cssClass = "sc-approved"; icon = "fa-check-circle"; label = "APPROVED";
                break;
            case "R":
                cssClass = "sc-rejected"; icon = "fa-times-circle"; label = "REJECTED";
                break;
            case "P":
            default:
                cssClass = "sc-pending"; icon = "fa-clock-o"; label = "PENDING";
                break;
        }

        return string.Format(
            "<div class=\"status-count-box {0}\">" +
            "<div class=\"status-count-label\"><i class=\"fa {1}\"></i>{2}</div>" +
            "<p class=\"status-count-num\">{3}</p></div>",
            cssClass, icon, label, count);
    }
    public string CallPostFunction(string detail, string url)
    {
        try
        {
            // Create a request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(detail);
                streamWriter.Flush();
            }
            // Get the response
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    return result; // Optionally deserialize this JSON string to an object
                }
            }
        }
        catch (Exception ex)
        {
            var message = ex.Message;
        }
        return "";
    }
    public string CalculateTimeDifference(DateTime startDate, DateTime endDate)
    {
        int days = 0;
        int hours = 0;
        int mins = 0;
        int secs = 0;
        string final = string.Empty;

        if (endDate > startDate)
        {
            TimeSpan diff = endDate - startDate;

            days = diff.Days;
            hours = diff.Hours;
            mins = diff.Minutes;
            secs = diff.Seconds;

            final = string.Format("{0} days {1} hours {2} mins {3} secs", days, hours, mins, secs);
        }

        return final;
    }
    private void GetDashboardIncome()
    {
        string compDb = HttpContext.Current.Session["MlmDatabase" +
                          HttpContext.Current.Session["CompID"]].ToString();

        using (SqlConnection con = new SqlConnection(compDb))
        {
            con.Open();



            string checkSP = "";
            if (Session["compid"].ToString() == "1106")
            {
                checkSP = @"
IF NOT EXISTS (SELECT * FROM sys.objects 
               WHERE type = 'P' AND name = 'Sp_DashboardIncome_Total')
BEGIN
    EXEC('
CREATE PROC Sp_DashboardIncome_Total
(
    @formNO VARCHAR(20)
)
AS
BEGIN

DECLARE @sql NVARCHAR(MAX) = ''''
DECLARE @tableName VARCHAR(200)
DECLARE @columnName VARCHAR(200)

DECLARE cur CURSOR FOR
SELECT TableName, ColumnName
FROM M_PayOutColumnSetting
WHERE columnname = ''chqAmt''
ORDER BY Seq

OPEN cur
FETCH NEXT FROM cur INTO @tableName, @columnName

WHILE @@FETCH_STATUS = 0
BEGIN

    SET @sql = @sql + ''

    SELECT SUM(ISNULL('' + QUOTENAME(@columnName) + '',0)) amt
    FROM '' + QUOTENAME(@tableName) + '' a
    INNER JOIN M_MonthSessnMaster b ON a.sessid = b.sessid
    WHERE b.onwebsite = ''''Y'''' AND a.FormNo = '''''' + @formNO + ''''''

    UNION ALL ''

    FETCH NEXT FROM cur INTO @tableName, @columnName

END

CLOSE cur
DEALLOCATE cur

IF RIGHT(@sql,10) = ''UNION ALL ''
    SET @sql = LEFT(@sql, LEN(@sql) - 10)

SET @sql = ''

SELECT ISNULL(SUM(amt),0) TotalIncome
FROM
(
    '' + @sql + ''
) x''

EXEC(@sql)

END
')
END
";
            }
            else
            {
                checkSP = @"
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Sp_DashboardIncome_Total')
            BEGIN
                EXEC('
                    CREATE PROC Sp_DashboardIncome_Total
                    (
                        @formNO VARCHAR(20)
                    )
                    AS
                    BEGIN
                        DECLARE @sql NVARCHAR(MAX) = N'''';
                        DECLARE @tableName VARCHAR(200);
                        DECLARE @columnName VARCHAR(200);

                        DECLARE cur CURSOR FOR
                            SELECT TableName, ColumnName
                            FROM M_PayOutColumnSetting 
                            WHERE IsDash = ''1''
                            ORDER BY Seq;

                        OPEN cur;
                        FETCH NEXT FROM cur INTO @tableName, @columnName;

                        WHILE @@FETCH_STATUS = 0
                        BEGIN
                            SET @sql = @sql + ''
                                SELECT SUM(ISNULL('' + QUOTENAME(@columnName) + '',0)) AS amt
                                FROM '' + QUOTENAME(@tableName) + ''
                                WHERE FormNo = '''''' + @formNO + ''''''
                                UNION ALL
                            '';

                            FETCH NEXT FROM cur INTO @tableName, @columnName;
                        END;

                        CLOSE cur;
                        DEALLOCATE cur;

                        -- REMOVE LAST UNION ALL
                        SET @sql = REVERSE(STUFF(
                            REVERSE(@sql),
                            1,
                            PATINDEX(''%LLA NOINU%'', REVERSE(@sql)) + 8,
                            ''''
                        ));

                        SET @sql = ''
                            SELECT ISNULL(SUM(amt),0) AS TotalIncome
                            FROM ('' + @sql + '') AS x
                        '';

                        EXEC(@sql);
                    END
                ')
            END";
            }

            SqlCommand cmdCheck = new SqlCommand(checkSP, con);
            cmdCheck.ExecuteNonQuery();

            // 2) EXECUTE THE PROCEDURE
            SqlCommand cmd = new SqlCommand("Sp_DashboardIncome_Total", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@formNO", Session["FormNo"]);

            object result = cmd.ExecuteScalar();

            con.Close();

            decimal income = 0;

            if (result != null && result != DBNull.Value)
                income = Convert.ToDecimal(result);
            LblMyIncome.Text = income.ToString("N2");
        }
    }
    public int GetTotalDirects()
    {
        int totalDirects = 0;
        int teamDirect = 0;

        string compDb = HttpContext.Current.Session["MlmDatabase" +
                         HttpContext.Current.Session["CompID"]].ToString();

        using (SqlConnection con = new SqlConnection(compDb))
        {
            con.Open();



            string createOrAlterProc = @"
            IF OBJECT_ID('Sp_TotalDirects', 'P') IS NULL
            BEGIN
                EXEC('
                    CREATE PROCEDURE Sp_TotalDirects
                    (
                        @FormNo INT
                    )
                    AS
                    BEGIN
                        SELECT 
                            SUM(MyDirects) AS MyDirects,
                            SUM(TeamDirect) AS TeamDirect
                        FROM (
                            SELECT COUNT(*) AS MyDirects, 0 AS TeamDirect
                            FROM R_Memtreerelation a WITH(NOLOCK)
                            INNER JOIN M_MemberMaster b WITH(NOLOCK) ON a.FormnoDwn = b.Formno
                            INNER JOIN m_KitMaster c WITH(NOLOCK) ON b.kitid = c.KitId
                            WHERE c.RowStatus = ''Y''
                              AND a.FormNo = @FormNo
                              AND a.Mlevel = 1

                            UNION ALL

                            SELECT 0 AS MyDirects, COUNT(*) AS TeamDirect
                            FROM R_Memtreerelation a WITH(NOLOCK)
                            INNER JOIN M_MemberMaster b WITH(NOLOCK) ON a.FormnoDwn = b.Formno
                            INNER JOIN m_KitMaster c WITH(NOLOCK) ON b.kitid = c.KitId
                            WHERE c.RowStatus = ''Y''
                              AND a.FormNo = @FormNo
                        ) T
                    END
                ')
            END
            ELSE
            BEGIN
                EXEC('
                    ALTER PROCEDURE Sp_TotalDirects
                    (
                        @FormNo INT
                    )
                    AS
                    BEGIN
                        SELECT 
                            SUM(MyDirects) AS MyDirects,
                            SUM(TeamDirect) AS TeamDirect
                        FROM (
                            SELECT COUNT(*) AS MyDirects, 0 AS TeamDirect
                            FROM R_Memtreerelation a WITH(NOLOCK)
                            INNER JOIN M_MemberMaster b WITH(NOLOCK) ON a.FormnoDwn = b.Formno
                            INNER JOIN m_KitMaster c WITH(NOLOCK) ON b.kitid = c.KitId
                            WHERE c.RowStatus = ''Y''
                              AND a.FormNo = @FormNo
                              AND a.Mlevel = 1

                            UNION ALL

                            SELECT 0 AS MyDirects, COUNT(*) AS TeamDirect
                            FROM R_Memtreerelation a WITH(NOLOCK)
                            INNER JOIN M_MemberMaster b WITH(NOLOCK) ON a.FormnoDwn = b.Formno
                            INNER JOIN m_KitMaster c WITH(NOLOCK) ON b.kitid = c.KitId
                            WHERE c.RowStatus = ''Y''
                              AND a.FormNo = @FormNo
                        ) T
                    END
                ')
            END";

            using (SqlCommand cmdProc = new SqlCommand(createOrAlterProc, con))
            {
                cmdProc.ExecuteNonQuery();
            }

            // 2️⃣ CALL PROCEDURE
            using (SqlCommand cmd = new SqlCommand("Sp_TotalDirects", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FormNo", Session["FormNo"]);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        totalDirects = dr["MyDirects"] != DBNull.Value
                            ? Convert.ToInt32(dr["MyDirects"])
                            : 0;

                        teamDirect = dr["TeamDirect"] != DBNull.Value
                            ? Convert.ToInt32(dr["TeamDirect"])
                            : 0;
                    }
                }
            }
        }

        LblMyDirects.Text = totalDirects.ToString();
        LblMyTeam.Text = teamDirect.ToString();

        return totalDirects;
    }
    private void LoadTeam()
    {
        try
        {
            DataSet Ds = new DataSet();
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@FormNo", Session["FormNo"]);

            Ds = SqlHelper.ExecuteDataset(
                    HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                    "sp_LoadTeam",
                    prms);

            // Table 0
            if (Ds.Tables[0].Rows.Count > 0)
            {
                
                LblTotalBusiness.Text = (Convert.ToDecimal(Ds.Tables[0].Rows[0]["DirectUnit"]) +
                     Convert.ToDecimal(Ds.Tables[0].Rows[0]["InDirectUnit"])).ToString();
            }

            // Table 1 (Profile)
            if (Ds.Tables[1].Rows.Count > 0)
            {
                LblMemId.Text = Ds.Tables[1].Rows[0]["Idno"].ToString();
                LblMemName.Text = Ds.Tables[1].Rows[0]["Name"].ToString();
                LblSponsorID.Text = Ds.Tables[1].Rows[0]["sponsorId"].ToString();
                LblSponsorName.Text = Ds.Tables[1].Rows[0]["sponsorName"].ToString();
                LblJoiningDate.Text = Ds.Tables[1].Rows[0]["DOj"].ToString();
            }
            if (Ds.Tables[6].Rows.Count > 0)
            {
                lblRankTitle.Text = Ds.Tables[6].Rows[0]["Rank"].ToString();
            }

            if (Ds.Tables[4].Rows.Count > 0)
            {
                RptNews.DataSource = Ds.Tables[4];
                RptNews.DataBind();
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private void FillKYCStatus()
    {
        try
        {
            DataSet Ds = new DataSet();

            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@FormNo", Session["FormNo"]);
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), "Sp_GetKYCStatus", prms);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                var top10 = Ds.Tables[0].AsEnumerable().Take(10).CopyToDataTable();
                RptKYCStatus.DataSource = top10;
                RptKYCStatus.DataBind();
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
}
public class Referalres
{
    public string response { get; set; }
    public string urlLeft { get; set; }
    public string urlright { get; set; }
}
public class Referalreq
{
    public string islogin { get; set; }
    public string reqtype { get; set; }
    public string userid { get; set; }
    public string passwd { get; set; }
}