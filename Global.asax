<%@ Application Language="VB" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="System.Data.SqlClient" %>


<script RunAt="server">

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup     
        '' HttpContext.Current.Session("InvDatabase" & Session("CompID")) = "PadmInv"
        'Application("Connect") = "Server=164.132.18.172;UID=ubgshp;PWD=S#09B!81ee$;Database=BigShopee;Pooling=False;Connect Timeout=500000000;"
        ''Application("Connect") = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Application("sConnect") = "Data Source=103.193.74.91,1533;Initial Catalog=MLMMenuMaster;Integrated Security=false;UID=Mlmmenu;PWD=menu@$#123;Pooling=False;Connect Timeout=900000000"
        
        ''Application("CompID") = "1003"
    
       
    End Sub
    
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub
        
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started     
       
        ''getData()
        
       
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
    
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub
    
   
    
   
        
    
    
    
    
    
    'Sub GetMenu()
    '    Try
    '        Dim dt As DataTable = New DataTable
    '        Dim ds As DataSet = New DataSet
    '        Dim str As String = " Select a.MenuId as MenuId, a.MenuName as MenuName,a.ParentId as ParentId, a.OnSelect as OnSelect from "
    '        str &= " M_CompWiseWebMenuMaster a Where a.ActiveStatus = 'Y' And a.RowStatus ='Y' And a.CompanyID = '" & Application("CompID") & "' order by a.Hierar,a.MenuId"
    '        ds = SqlHelper.ExecuteDataset(Application("sConnect"), CommandType.Text, str)
    '        dt = ds.Tables(0)
    '        HttpContext.Current.Session("Menu") = dt

    '    Catch ex As Exception

    '    End Try
    'End Sub
       
       
</script>

