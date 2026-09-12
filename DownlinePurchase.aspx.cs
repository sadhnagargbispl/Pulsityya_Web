using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DownlinePurchase : System.Web.UI.Page
{
    SqlConnection Conn;
    SqlCommand Comm;
    SqlDataAdapter Ad;
    DataTable dt;
    DAL obj;
    clsGeneral objGen = new clsGeneral();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                obj = new DAL();
                Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
                Conn.Open();
                if (!IsPostBack)
                {
                    Label9.Text = "Total PV : ";
                    Label2.Text = "Total BV : ";
                    RbtLegNo.Items[1].Text = "Left";
                    RbtLegNo.Items[2].Text = "Right";
                    Filldate();
                    FillLevel();
                    DdlLevel.SelectedValue = "0";
                    LevelDetail();
                    FillLevel_Ra();
                }
            }
            else
            {
                Response.Redirect("logout.aspx");
            }
        }
        catch (Exception ex)
        {
            LogError(ex);
        }
    }
    private void Filldate()
    {
        try
        {
            DAL objDAL = new DAL();
            string str = "Select Replace(Convert(Varchar,Getdate(),106),' ','-') as CurrentDate";
            DataTable dtData = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];
        }
        catch { }
    }
    protected void FillLevel()
    {
        try
        {
            string str =
               obj.IsoStart + "Select distinct * from (" +
                "Select 0 As MLevel,'-- ALL --' As LevelName " +
                "Union ALL select MLevel,'Level :' + convert(varchar,MLevel) " +
                "from " + obj.dBName + "..R_MemTreeRelation with(nolock) where FormNo='" + Session["FormNo"] + "') Temp order by MLevel" + obj.IsoEnd;
            dt = new DataTable();
            dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];
            DdlLevel.DataSource = dt;
            DdlLevel.DataTextField = "LevelName";
            DdlLevel.DataValueField = "MLevel";
            DdlLevel.DataBind();
        }
        catch (Exception ex)
        {
            LogError(ex);
        }
    }
    protected void LevelDetail()
    {
        string compid = Session["CompID"].ToString();
        try
        {

            string str = "";
            string condition = "";
            if (RbtProduct.SelectedValue == "R")
            {
                if (RbtLegNo.SelectedValue != "0")
                    condition = condition + " and a.LegNo='" + RbtLegNo.SelectedValue + "'";
            }
            else if (RbtLegNo.SelectedValue != "0")
                condition = condition + " and d.LegNo='" + RbtLegNo.SelectedValue + "'";

            string scrname = "";
            string FrmDate = "";
            string ToDate = "";
            FrmDate = TxtFromDate.Text;
            ToDate = TxtToDate.Text;
            if (FrmDate != "")
            {
                try
                {
                    DateTime Dt = Convert.ToDateTime(FrmDate);
                }
                catch (Exception ex)
                {
                    scrname = "<SCRIPT language='javascript'>alert('Check Start Date.. ');" + "</SCRIPT>";
                    this.RegisterStartupScript("MyAlert", scrname);
                    return;
                }
            }
            if (ToDate != "")
            {
                try
                {
                    DateTime Dt = Convert.ToDateTime(ToDate);
                }
                catch (Exception ex)
                {
                    scrname = "<SCRIPT language='javascript'>alert('Check End Date.. ');" + "</SCRIPT>";
                    this.RegisterStartupScript("MyAlert", scrname);
                    return;
                }
            }
            if (FrmDate != "" & ToDate != "")
                condition = condition + " And Cast(Convert(Varchar,b.BillDate,106)as Date)>='" + FrmDate + "' And Cast(Convert(Varchar,b.BillDate,106)as Date)<='" + ToDate + "'";
            if (RbtProduct.SelectedValue == "R")
            {
                str = obj.IsoStart + " exec Sp_DowmLineRepurchaseReport '" + Session["Formno"] + "','" + FrmDate + "','" + ToDate + "' " + obj.IsoEnd;
            }
            else
            {
                str += obj.IsoStart + " Select  a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], ";
                str += " Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name],";
                str += " Replace(Convert(Varchar,b.Rectimestamp,106),' ','-')+' '+CONVERT(varchar(15),CAST(b.Rectimestamp AS TIME),22) as [Bill Date],";
                str += " Repurchincome as BV,pvvalue as PV ";
                str += " from  " + obj.dBName + "..M_MemberMaster as a with(nolock) ";
                str += " Inner Join " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn ";
                str += " Inner join " + obj.dBName + "..RepurchIncome b  on d.FormnoDwn  = b.Formno and b.BillType<>'R' 	 left Join " + obj.dBName + "..M_KitMaster as k with(nolock)on b.Kitid=k.KitId  And  k.RowStatus='Y' ";
                str += " Where  d.Formno='" + Session["Formno"] + "'  " + condition + " order by [Bill Date]  desc   " + obj.IsoEnd;
            }
            dt = new DataTable();
            obj = new DAL();
            dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];
            Session["DirectData1"] = dt;
            GrdDirects.DataSource = dt;
            LblttlRcd.Text = dt.Rows.Count.ToString();
            LblttlRcd1.Text = dt.Rows.Count.ToString();
            GrdDirects.CurrentPageIndex = 0;
            GrdDirects.PageSize = Convert.ToInt32(ddlPazeSize.SelectedValue);
            GrdDirects.DataBind();
            if ((RbtProduct.SelectedValue == "T"))
            {
                divR.Visible = false;
                divT.Visible = true;
                lblleftbv.Visible = true;
                lblbv.Visible = true;
                DataRow aDr1 = dt.NewRow();
                if (Convert.ToInt32(RbtLegNo.SelectedValue) == 1)
                {
                    try
                    {
                        Lblleft.Text = "Left BV : " + dt.Compute("Sum(BV)", "");
                        lblleftbv1.Text = "Left PV : " + dt.Compute("Sum(PV)", "");
                        lblbv1.Text = "Right PV : 0.00";
                        Lblright.Text = "Right BV : 0.00";
                        lblleftbv.Text = "Left BV : " + dt.Compute("Sum(BV)", "");
                        lblbv.Text = "Right BV : 0.00";
                    }
                    catch (Exception ex)
                    {
                    }
                }
                if (Convert.ToInt32(RbtLegNo.SelectedValue) == 2)
                {
                    try
                    {
                        Lblright.Text = "Right BV : " + dt.Compute("Sum(BV)", "");
                        Lblleft.Text = "Left BV : 0.00";
                        lblbv.Text = "Right BV : " + dt.Compute("Sum(BV)", "");
                        lblleftbv.Text = "Left BV : 0.00";
                        lblleftbv1.Text = "Left PV : 0.00";
                        lblbv1.Text = "Right PV : " + dt.Compute("Sum(PV)", "");
                    }
                    catch (Exception ex)
                    {
                    }

                }
                if (Convert.ToInt32(RbtLegNo.SelectedValue) == 0)
                {
                    DataTable dt1 = new DataTable();
                    dt1 = dt;

                    DataView dv = new DataView(dt1);
                    dv.RowFilter = "[Group Name] = 'Left'";
                    dt1 = dv.ToTable();
                    try
                    {
                        if ((dt1.Rows.Count > 0))
                        {
                            DataRow aDr2 = dt1.NewRow();
                            Lblleft.Text = dt1.Compute("Sum(Bv)", "").ToString();
                            lblleftbv.Text = "Left BV : " + dt1.Compute("Sum(BV)", "");
                            lblleftbv1.Text = "Left PV : " + dt1.Compute("Sum(PV)", "");
                        }
                        else
                        {
                            Lblleft.Text = "0.00";
                            lblleftbv.Text = "Left BV : " + "0.00";
                            lblleftbv1.Text = "Left PV : " + "0.00";
                        }
                    }
                    catch (Exception ex)
                    {
                    }


                    dt1 = dt;
                    DataView dv1 = new DataView(dt1);
                    dv1.RowFilter = "[Group Name] = 'Right'";
                    dt1 = dv1.ToTable();
                    try
                    {
                        if ((dt1.Rows.Count > 0))
                        {
                            DataRow aDr3 = dt1.NewRow();

                            Lblright.Text = dt1.Compute("Sum(Bv)", "").ToString();
                            lblbv.Text = "Right BV : " + dt1.Compute("Sum(BV)", "");
                            lblbv1.Text = "Right PV : " + dt1.Compute("Sum(PV)", "");
                        }
                        else
                        {
                            Lblright.Text = "0.00";
                            lblbv.Text = "Right BV : 0.00";
                            lblbv1.Text = "Right PV : 0.00";
                        }

                    }
                    catch (Exception ex)
                    {
                    }

                }
            }
            if ((RbtProduct.SelectedValue == "R"))
            {
                if (Session["CompID"].ToString() == "1108" || (compid == "1007") | (compid == "1010" | compid == "1095" | compid == "1097" | compid == "1100" | compid == "1103" | compid == "1107" | compid == "1109" | compid == "1110"))
                {
                    divR.Visible = false;
                    divT.Visible = true;

                    DataRow aDr1 = dt.NewRow();
                    try
                    {
                        if (Convert.ToInt32(RbtLegNo.SelectedValue) == 1)
                        {
                            try
                            {
                                if (compid == "1107")
                                {
                                    Lblleft.Text = dt.Compute("Sum(EV)", "").ToString();
                                }
                                else
                                {
                                    Lblleft.Text = dt.Compute("Sum(BV)", "").ToString();
                                }

                            }
                            catch (Exception ex)
                            {
                            }

                            Lblright.Text = "0";
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    try
                    {
                        if (Convert.ToInt32(RbtLegNo.SelectedValue) == 2)
                        {
                            if ((dt.Rows.Count > 0))
                            {
                                try
                                {
                                    if (compid == "1107")
                                    {
                                        Lblright.Text = dt.Compute("Sum(EV)", "").ToString();
                                    }
                                    else
                                    {
                                        Lblright.Text = dt.Compute("Sum(BV)", "").ToString();
                                    }

                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            Lblleft.Text = "0";
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                    if (Convert.ToInt32(RbtLegNo.SelectedValue) == 0)
                    {
                        DataTable dt1 = new DataTable();
                        dt1 = dt;

                        DataView dv = new DataView(dt1);
                        dv.RowFilter = "[Group Name] = 'Left'";
                        dt1 = dv.ToTable();
                        if ((dt1.Rows.Count > 0))
                        {
                            DataRow aDr2 = dt1.NewRow();
                            try
                            {
                                if (compid == "1107")
                                {
                                    Lblleft.Text = dt1.Compute("Sum(EV)", "").ToString();
                                }
                                else
                                {
                                    Lblleft.Text = dt1.Compute("Sum(BV)", "").ToString();
                                }

                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else
                        {
                            Lblleft.Text = "0.00";
                            LblLeftRoyalty.Text = "0.00";
                        }




                        dt1 = dt;
                        DataView dv1 = new DataView(dt1);
                        dv1.RowFilter = "[Group Name] = 'Right'";
                        dt1 = dv1.ToTable();
                        if ((dt1.Rows.Count > 0))
                        {
                            DataRow aDr3 = dt1.NewRow();
                            try
                            {
                                if (compid == "1107")
                                {
                                    Lblright.Text = dt1.Compute("Sum(EV)", "").ToString();
                                }
                                else
                                {
                                    Lblright.Text = dt1.Compute("Sum(BV)", "").ToString();
                                }

                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else
                            Lblright.Text = "0.00";
                        if (compid == "1010")
                        {
                            if (RbtProduct.SelectedValue == "R")
                                divRoyalty.Visible = false;
                            else
                                divRoyalty.Visible = true;
                        }
                        else
                            divRoyalty.Visible = false;
                    }
                }
                else if (Session["CompID"].ToString() == "1108" || compid == "1102" | compid == "1107" | compid == "1109" | compid == "1110")
                {
                    divR.Visible = false;
                    divT.Visible = true;
                    lblleftbv.Visible = true;
                    lblbv.Visible = true;
                    DataRow aDr1 = dt.NewRow();
                    if (Convert.ToInt32(RbtLegNo.SelectedValue) == 1)
                    {
                        try
                        {
                            Lblleft.Text = "Left BV : " + dt.Compute("Sum(BV)", "");
                            Lblright.Text = "Right BV : 0.00";
                            lblleftbv.Text = "Left BV : " + dt.Compute("Sum(BV)", "");
                            lblbv.Text = "Right BV : 0.00";
                            lblleftbv1.Text = "Left PV : " + dt.Compute("Sum(PV)", "");
                            lblbv1.Text = "Right PV : 0.00";
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    if (Convert.ToInt32(RbtLegNo.SelectedValue) == 2)
                    {
                        try
                        {
                            Lblright.Text = "Right BV : " + dt.Compute("Sum(BV)", "");
                            Lblleft.Text = "Left BV : 0.00";
                            lblbv.Text = "Right BV : " + dt.Compute("Sum(BV)", "");
                            lblleftbv.Text = "Left BV : 0.00";
                            lblbv1.Text = "Right PV : " + dt.Compute("Sum(PV)", "");
                            lblleftbv1.Text = "Left PV : 0.00";
                            //Lblright.Text = dt.Compute("Sum(Bv)", "").ToString();
                            //Lblleft.Text = "0";
                            //lblbv.Text = dt.Compute("Sum(BV)", "").ToString();
                            //lblbv.Text = "0";
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    if (Convert.ToInt32(RbtLegNo.SelectedValue) == 0)
                    {
                        DataTable dt1 = new DataTable();
                        dt1 = dt;

                        DataView dv = new DataView(dt1);
                        dv.RowFilter = "[Group Name] = 'Left'";
                        dt1 = dv.ToTable();
                        try
                        {
                            if ((dt1.Rows.Count > 0))
                            {
                                DataRow aDr2 = dt1.NewRow();


                                Lblleft.Text = dt1.Compute("Sum(Bv)", "").ToString();
                                lblleftbv.Text = "Left BV : " + dt1.Compute("Sum(BV)", "");
                                lblleftbv1.Text = "Left PV : " + dt1.Compute("Sum(PV)", "");
                            }
                            else
                            {
                                Lblleft.Text = "0.00";
                                lblleftbv.Text = "Left BV : " + "0.00";
                                lblleftbv1.Text = "Left PV : " + "0.00";
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        dt1 = dt;
                        DataView dv1 = new DataView(dt1);
                        dv1.RowFilter = "[Group Name] = 'Right'";
                        dt1 = dv1.ToTable();
                        try
                        {
                            if ((dt1.Rows.Count > 0))
                            {
                                DataRow aDr3 = dt1.NewRow();

                                Lblright.Text = dt1.Compute("Sum(Bv)", "").ToString();
                                lblbv.Text = "Right BV : " + dt1.Compute("Sum(BV)", "");
                                lblbv1.Text = "Right PV : " + dt1.Compute("Sum(PV)", "");
                            }
                            else
                            {
                                lblbv.Text = "Right BV : 0.00";
                                lblbv1.Text = "Right PV : 0.00";
                            }
                        }
                        catch (Exception ex)
                        {
                        }

                    }
                }
                else
                {
                    divR.Visible = true;
                    divT.Visible = false;
                    if ((dt.Rows.Count > 0))
                    {
                        // below commit 22 March 2022
                        try
                        {
                            lblTotalBV.Text = dt.Compute("Sum([Repurchase BV])", "").ToString();
                            lblTotalBV1.Text = dt.Compute("Sum([Repurchase BV])", "").ToString();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else
                        lblTotalBV.Text = "0";
                    lblTotalBV1.Text = "0";
                }
                if (Session["CompID"].ToString() == "1108" || compid == "1109" || compid == "1102" | compid == "1107" | compid == "1109" | compid == "1110")
                {
                    if ((dt.Rows.Count > 0))
                    {
                        // below commit 22 March 2022
                        try
                        {
                            lblTotalBV.Text = dt.Compute("Sum(BV)", "").ToString();
                            lblTotalBV1.Text = dt.Compute("Sum(PV)", "").ToString();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else
                        lblTotalBV.Text = "0";
                    lblTotalBV1.Text = "0";
                    LblttlBv.Text = lblTotalBV.Text.ToString();
                    LblttlBv1.Text = lblTotalBV.Text.ToString();
                }
                else
                {
                    LblttlBv.Text = lblTotalBV.Text.ToString();
                    LblttlBv1.Text = lblTotalBV.Text.ToString();
                }

                if (Session["CompID"].ToString() == "1108" || compid != "1109" || compid != "1102" | compid == "1107" | compid == "1109" | compid == "1110")
                {
                    if (Convert.ToInt32(RbtLegNo.SelectedValue) == 1)
                    {
                        try
                        {
                            Lblleft.Text = dt.Compute("Sum(BV)", "").ToString();
                            Lblright.Text = "0";
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    if (Convert.ToInt32(RbtLegNo.SelectedValue) == 2)
                    {
                        if ((dt.Rows.Count > 0))
                        {
                            try
                            {
                                Lblright.Text = dt.Compute("Sum(BV)", "").ToString();
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        Lblleft.Text = "0";
                    }
                    if (Convert.ToInt32(RbtLegNo.SelectedValue) == 0)
                    {
                        DataTable dt1 = new DataTable();
                        dt1 = dt;

                        DataView dv = new DataView(dt1);
                        dv.RowFilter = "[Group Name] = 'Left'";
                        dt1 = dv.ToTable();
                        if ((dt1.Rows.Count > 0))
                        {
                            DataRow aDr2 = dt1.NewRow();
                            try
                            {
                                Lblleft.Text = dt1.Compute("Sum([Repurchase BV])", "").ToString();
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else
                        {
                            Lblleft.Text = "0.00";
                            LblLeftRoyalty.Text = "0.00";
                        }




                        dt1 = dt;
                        DataView dv1 = new DataView(dt1);
                        dv1.RowFilter = "[Group Name] = 'Right'";
                        dt1 = dv1.ToTable();
                        if ((dt1.Rows.Count > 0))
                        {
                            DataRow aDr3 = dt1.NewRow();
                            // If RbtProduct.SelectedValue = "R" Then
                            try
                            {
                                Lblright.Text = dt1.Compute("Sum([Repurchase BV])", "").ToString();
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else
                            Lblright.Text = "0.00";
                        if (compid == "1010")
                        {
                            if (RbtProduct.SelectedValue == "R")
                                divRoyalty.Visible = false;
                            else
                                divRoyalty.Visible = true;
                        }
                        else
                            divRoyalty.Visible = false;
                    }
                }
            }
            else
            {
                divR.Visible = true;
                divT.Visible = false;
                if ((dt.Rows.Count > 0))
                {
                    // below commit 22 March 2022
                    // lblTotalBV.Text = dt.Compute("Sum([Repurchase BV])", "")
                    try
                    {
                        lblTotalBV.Text = dt.Compute("Sum(BV)", "").ToString();
                        lblTotalBV1.Text = dt.Compute("Sum(PV)", "").ToString();
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                    lblTotalBV.Text = "0";
                    lblTotalBV1.Text = "0";
                }
                  
                LblttlBv.Text = lblTotalBV.Text.ToString();
                LblttlBv1.Text = lblTotalBV1.Text.ToString();
                dt.Columns["BV"].ColumnName = "BV";
                GrdDirects.DataSource = dt;
                GrdDirects.PageSize = Convert.ToInt32(ddlPazeSize.SelectedValue);
                GrdDirects.DataBind();
               divtotal.Visible = true;
            }

        }
        catch (Exception ex)
        {
            
        }
    }
    protected void GrdDirects_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        try
        {
            GrdDirects.CurrentPageIndex = e.NewPageIndex;
            GrdDirects.DataSource = Session["DirectData1"];
            GrdDirects.PageSize = Convert.ToInt32(ddlPazeSize.SelectedValue);
            GrdDirects.DataBind();
        }
        catch (Exception ex)
        {
            LogError(ex);
        }
    }
    protected void DdlLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LevelDetail();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss:fff") +
                          Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
        }
    }
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        LevelDetail();
    }
    protected void ddlPazeSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GrdDirects.DataSource = Session["DirectData1"];
            GrdDirects.PageSize = Convert.ToInt32(ddlPazeSize.SelectedValue);
            GrdDirects.DataBind();
        }
        catch (Exception ex)
        {
            // handle exception if needed
        }
    }
    protected void RbtProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillLevel_Ra();
    }
    protected void FillLevel_Ra()
    {
        if (RbtProduct.SelectedValue == "R")
        {
            if (Session["CompID"].ToString() == "1007" ||
                Session["CompID"].ToString() == "1010" ||
                Session["CompID"].ToString() == "1091" ||
                Session["CompID"].ToString() == "1095" ||
                Session["CompID"].ToString() == "1097")
            {
                LblLevel.Text = "Group Wise";
                DdlLevel.Visible = false;
                RbtLegNo.Visible = true;
            }
            else if (Session["CompID"].ToString() == "1102" || Session["CompID"].ToString() == "1107")
            {
                LblLevel.Text = "Group Wise";
                DdlLevel.Visible = false;
                RbtLegNo.Visible = true;
                lblleftbv.Visible = true;
                lblbv.Visible = true;
            }
            else
            {
                LblLevel.Text = "Level Wise";
                DdlLevel.Visible = true;
                RbtLegNo.Visible = false;
                lblleftbv.Visible = false;
                lblbv.Visible = false;
            }

        }
        else
        {
            LblLevel.Text = "Group Wise";
            DdlLevel.Visible = false;
            RbtLegNo.Visible = true;
            //lblleftbv.Visible = false;
            //lblbv.Visible = false;
        }
    }
    private void LogError(Exception ex)
    {
        string path = HttpContext.Current.Request.Url.AbsoluteUri;
        string text = path + " : " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss:fff") + Environment.NewLine;
        objGen.WriteToFile(text + ex.Message);
    }
    protected void BtnExportToExcel_Click(object sender, EventArgs e)
    {
        string compid = Session["CompID"].ToString();
        try
        {

            string str = "";
            string condition = "";
            if (RbtProduct.SelectedValue == "R")
            {
                if ((compid == "1007") | (compid == "1010") | (compid == "1091") | (compid == "1095") | (compid == "1097" | compid == "1100" | compid == "1103"))
                {
                    if (RbtLegNo.SelectedValue != "0")
                        condition = condition + " and d.LegNo='" + RbtLegNo.SelectedValue + "'";
                }
                else if (Session["CompID"].ToString() == "1108" || (compid == "1109") || (compid == "1102") | (compid == "1107") | (compid == "1110"))
                {
                    if (RbtLegNo.SelectedValue != "0")
                        condition = condition + " and a.LegNo='" + RbtLegNo.SelectedValue + "'";
                }
                else if (Convert.ToInt32(DdlLevel.SelectedValue) > 0)
                    condition = condition + "AND d.MLevel='" + DdlLevel.SelectedValue + "'";
            }
            else if (RbtLegNo.SelectedValue != "0")
                condition = condition + " and d.LegNo='" + RbtLegNo.SelectedValue + "'";

            string scrname = "";
            string FrmDate = "";
            string ToDate = "";
            FrmDate = TxtFromDate.Text;
            ToDate = TxtToDate.Text;
            if (FrmDate != "")
            {
                try
                {
                    DateTime Dt = Convert.ToDateTime(FrmDate);
                }
                catch (Exception ex)
                {
                    scrname = "<SCRIPT language='javascript'>alert('Check Start Date.. ');" + "</SCRIPT>";
                    this.RegisterStartupScript("MyAlert", scrname);
                    return;
                }
            }
            if (ToDate != "")
            {
                try
                {
                    DateTime Dt = Convert.ToDateTime(ToDate);
                }
                catch (Exception ex)
                {
                    scrname = "<SCRIPT language='javascript'>alert('Check End Date.. ');" + "</SCRIPT>";
                    this.RegisterStartupScript("MyAlert", scrname);
                    return;
                }
            }
            if (FrmDate != "" & ToDate != "")
                condition = condition + " And Cast(Convert(Varchar,b.BillDate,106)as Date)>='" + FrmDate + "' And Cast(Convert(Varchar,b.BillDate,106)as Date)<='" + ToDate + "'";

            if (compid == "1010" | compid == "1103")
            {
                if (TxtFromDate.Text != "")
                    FrmDate = TxtFromDate.Text;
                if (TxtToDate.Text != "")
                    ToDate = TxtToDate.Text;
            }
            if (RbtProduct.SelectedValue == "R")
            {
                str = obj.IsoStart + " exec Sp_DowmLineRepurchaseReport '" + Session["Formno"] + "','" + FrmDate + "','" + ToDate + "' " + obj.IsoEnd;
            }
            str += obj.IsoStart + " Select  a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], ";
            str += " Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name],";
            str += " Replace(Convert(Varchar,b.Rectimestamp,106),' ','-')+' '+CONVERT(varchar(15),CAST(b.Rectimestamp AS TIME),22) as [Bill Date],";
            str += " Repurchincome as BV,pvvalue as PV  ";
            str += " from  " + obj.dBName + "..M_MemberMaster as a with(nolock) ";
            str += " Inner Join " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn ";
            str += " Inner join " + obj.dBName + "..RepurchIncome b  on d.FormnoDwn  = b.Formno and b.BillType<>'R' 	 left Join " + obj.dBName + "..M_KitMaster as k with(nolock)on b.Kitid=k.KitId  And  k.RowStatus='Y' ";
            str += " Where  d.Formno='" + Session["Formno"] + "'  " + condition + " order by [Bill Date]  desc   " + obj.IsoEnd;
            obj = new DAL();
            DataTable dtTemp = new DataTable();
            DataGrid dg = new DataGrid();
            dtTemp = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];
            if (Session["CompID"].ToString() == "1007" && dtTemp.Columns.Contains("BV"))
            {
                dtTemp.Columns["BV"].ColumnName = "PV";
            }
            dg.DataSource = dtTemp;
            dg.DataBind();
            ExportToExcel("Downline" + RbtProduct.SelectedItem.Text + ".xls", dg);
        }
        catch (Exception ex)
        {

        }
    }
    private void ExportToExcel(string strFileName, DataGrid dg)
    {
        try
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htw;

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
            Response.Charset = "";

            dg.EnableViewState = false;

            htw = new HtmlTextWriter(sw);
            dg.RenderControl(htw);

            Response.Write(sw.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss:fff") +
                          Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
        }
    }
}
