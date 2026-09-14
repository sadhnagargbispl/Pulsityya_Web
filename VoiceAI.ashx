<%@ WebHandler Language="VB" Class="VoiceAI" %>

'=================================================================
'  VoiceAI.ashx  -  Mistral proxy for the Admin Voice Assistant
'  Project: Basic-MLM-ADMIN (WebForms, VB.NET)
'
'  Client (js/VoiceAssistant.js) POSTs the following form fields:
'     raw     : the spoken Hinglish sentence
'     mode    : "short" | "full"   (client decides via regex)
'     menus   : newline-joined list of menu names   (live DOM scan)
'     fields  : newline-joined list of field labels  (live DOM scan)
'     buttons : newline-joined list of button texts  (live DOM scan)
'
'  Returns: a single strict JSON object (Mistral output, fences stripped)
'           e.g. {"intent":"navigate","target":"Dashboard"}
'
'  Mistral key is read from web.config appSettings("MistralApiKey").
'  NEVER hardcode the key here.
'=================================================================

Imports System
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Web
Imports System.Configuration
Imports System.Web.Script.Serialization

Public Class VoiceAI : Implements IHttpHandler

    ' Mistral endpoint + model. mistral-small-latest is cheap & fast for intent parsing.
    Private Const MISTRAL_URL As String = "https://api.mistral.ai/v1/chat/completions"
    Private Const MISTRAL_MODEL As String = "mistral-small-latest"

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache)

        Try
            ' --- diagnostic: GET VoiceAI.ashx?diag=1 -> shows if running app sees the key (no leak) ---
            If context.Request.QueryString("diag") = "1" Then
                Dim k As String = Nz(ConfigurationManager.AppSettings("MistralApiKey")).Trim()
                Dim isPlaceholder As Boolean = (k = "" OrElse k = "j2fiL4uy9jHTwb9dJdd3ixezOkQpGr10")
                Dim present As String = IIf(isPlaceholder, "false", "true")
                Dim cfgPath As String = ""
                Try : cfgPath = System.Web.Hosting.HostingEnvironment.MapPath("~/web.config") : Catch : End Try
                context.Response.Write("{""keyPresent"":" & present & _
                    ",""keyLength"":" & k.Length & _
                    ",""startsWith"":" & JsStr(Left(k, 4)) & _
                    ",""model"":" & JsStr(MISTRAL_MODEL) & _
                    ",""webConfig"":" & JsStr(cfgPath) & "}")
                Return
            End If

            ' --- read inputs (form-encoded POST) ---
            Dim raw As String = Trim(Nz(context.Request.Form("raw")))
            Dim mode As String = LCase(Trim(Nz(context.Request.Form("mode"))))
            Dim menus As String = Nz(context.Request.Form("menus"))
            Dim fields As String = Nz(context.Request.Form("fields"))
            Dim buttons As String = Nz(context.Request.Form("buttons"))

            If raw = "" Then
                context.Response.Write("{""intent"":""unknown"",""message"":""Kuch sunai nahi diya, kripya dobara boliye.""}")
                Return
            End If

            Dim apiKey As String = Nz(ConfigurationManager.AppSettings("MistralApiKey")).Trim()
            ' Defensive cleanup for common copy-paste mistakes.
            If apiKey.StartsWith("""") AndAlso apiKey.EndsWith("""") AndAlso apiKey.Length >= 2 Then
                apiKey = apiKey.Substring(1, apiKey.Length - 2).Trim()
            End If
            If apiKey.ToLower().StartsWith("bearer ") Then apiKey = apiKey.Substring(7).Trim()
            'If apiKey = "" OrElse apiKey = "j2fiL4uy9jHTwb9dJdd3ixezOkQpGr10" Then
            '    context.Response.Write("{""intent"":""unknown"",""message"":""MistralApiKey web.config me set nahi hai. Kripya valid key daaliye.""}")
            '    Return
            'End If

            ' --- build request body ---
            Dim sys As String = BuildSystemPrompt(mode, menus, fields, buttons)

            Dim ser As New JavaScriptSerializer()
            ser.MaxJsonLength = Integer.MaxValue

            Dim payload As New Dictionary(Of String, Object)
            payload("model") = MISTRAL_MODEL
            payload("temperature") = 0
            payload("max_tokens") = 400
            Dim messages As New List(Of Object)
            messages.Add(NewMsg("system", sys))
            messages.Add(NewMsg("user", raw))
            payload("messages") = messages
            ' Ask Mistral to return a JSON object (supported by the API).
            Dim rf As New Dictionary(Of String, Object)
            rf("type") = "json_object"
            payload("response_format") = rf

            Dim bodyJson As String = ser.Serialize(payload)

            ' --- TLS 1.2 (required for Mistral over .NET Framework) ---
            ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType) ' Tls12

            Dim req As HttpWebRequest = CType(WebRequest.Create(MISTRAL_URL), HttpWebRequest)
            req.Method = "POST"
            req.ContentType = "application/json"
            req.Accept = "application/json"
            req.Headers("Authorization") = "Bearer " & apiKey
            req.Timeout = 60000

            Dim bytes As Byte() = Encoding.UTF8.GetBytes(bodyJson)
            req.ContentLength = bytes.Length
            Using rs As Stream = req.GetRequestStream()
                rs.Write(bytes, 0, bytes.Length)
            End Using

            Dim respText As String = ""
            Using resp As HttpWebResponse = CType(req.GetResponse(), HttpWebResponse)
                Using sr As New StreamReader(resp.GetResponseStream(), Encoding.UTF8)
                    respText = sr.ReadToEnd()
                End Using
            End Using

            ' --- extract message.content from Mistral response ---
            Dim content As String = ExtractContent(ser, respText)
            content = StripFences(content)

            If content = "" Then
                context.Response.Write("{""intent"":""unknown"",""message"":""AI se koi jawab nahi mila, kripya dobara koshish kijiye.""}")
            Else
                context.Response.Write(content)
            End If

        Catch wex As WebException
            ' surface HTTP errors (e.g. 401 bad key) without leaking the key
            Dim detail As String = "AI request fail hua."
            Try
                If wex.Response IsNot Nothing Then
                    Using sr As New StreamReader(wex.Response.GetResponseStream())
                        detail = sr.ReadToEnd()
                    End Using
                End If
            Catch
            End Try
            context.Response.Write("{""intent"":""unknown"",""message"":" & JsStr("Mistral error: " & Left(detail, 300)) & "}")
        Catch ex As Exception
            context.Response.Write("{""intent"":""unknown"",""message"":" & JsStr("Server error: " & ex.Message) & "}")
        End Try
    End Sub

    '----------------------------------------------------------------
    ' SYSTEM PROMPT BUILDER  (SHORT vs FULL, with live lists injected)
    '----------------------------------------------------------------
    Private Function BuildSystemPrompt(ByVal mode As String, ByVal menus As String, _
                                       ByVal fields As String, ByVal buttons As String) As String
        Dim sb As New StringBuilder()

        sb.AppendLine("You are the command parser for an MLM admin/user web panel.")
        sb.AppendLine("The user speaks in Hinglish (Hindi + English mix).")
        sb.AppendLine("Convert the user's spoken sentence into ONE strict JSON object.")
        sb.AppendLine("Reply ONLY with JSON. No prose, no markdown, no code fences.")
        sb.AppendLine("")
        sb.AppendLine("Allowed ""intent"" values:")
        sb.AppendLine(" - ""navigate"" : open a page/menu.  {""target"":""<menu name>""}")
        sb.AppendLine(" - ""fill""     : put values into form fields.  {""items"":[{""field"":""<field label>"",""value"":""<value>""}]}")
        sb.AppendLine(" - ""submit""   : click ANY action button on the page (Search, Show All, Show Detail,")
        sb.AppendLine("                View All, Advanced Search, Export To Excel, Export To CSV,")
        sb.AppendLine("                Print All Pages, Print Current Page, Approve, ApproveAll, Reject,")
        sb.AppendLine("                RejectAll, Send Sms, Confirm, Save, Paid, Verification, Back).")
        sb.AppendLine("                {""button"":""<button text or empty>""}")
        sb.AppendLine(" - ""read""     : read fields of current page.  {}")
        sb.AppendLine(" - ""clear""    : empty a field or whole form.  {""field"":""<field or 'all'>""}")
        sb.AppendLine(" - ""home""     : go to default/home page.  {}")
        sb.AppendLine(" - ""logout""   : sign out.  {}")
        sb.AppendLine(" - ""help""     : list capabilities.  {}")
        sb.AppendLine(" - ""unknown""  : nothing matched.  {""message"":""<hint>""}")
        sb.AppendLine("")
        sb.AppendLine("Match field / menu / button names to the CLOSEST option from the provided lists.")
        sb.AppendLine("Return the option text EXACTLY as listed so the front-end can locate it.")
        sb.AppendLine("")
        sb.AppendLine("AVAILABLE MENUS: " & OneLine(menus))
        sb.AppendLine("AVAILABLE FIELDS: " & OneLine(fields))
        sb.AppendLine("AVAILABLE BUTTONS: " & OneLine(buttons))

        If mode = "full" Then
            sb.AppendLine("")
            sb.AppendLine("For ""fill"" you MAY return multiple items in one call (user can dictate many fields at once).")
            sb.AppendLine("")
            sb.AppendLine("Examples:")
            sb.AppendLine("user: dashboard kholo  -> {""intent"":""navigate"",""target"":""Dashboard""}")
            sb.AppendLine("user: naam rakesh aur mobile 9001234567 bharo  -> {""intent"":""fill"",""items"":[{""field"":""Name"",""value"":""Rakesh""},{""field"":""Mobile No"",""value"":""9001234567""}]}")
            sb.AppendLine("user: search kar do  -> {""intent"":""submit"",""button"":""Search""}")
            sb.AppendLine("user: excel me export karo  -> {""intent"":""submit"",""button"":""Export To Excel""}")
            sb.AppendLine("user: show all dikhao  -> {""intent"":""submit"",""button"":""Show All""}")
            sb.AppendLine("user: ise approve kar do  -> {""intent"":""submit"",""button"":""Approve""}")
            sb.AppendLine("user: reject karo  -> {""intent"":""submit"",""button"":""Reject""}")
            sb.AppendLine("user: print all pages  -> {""intent"":""submit"",""button"":""Print All Pages""}")
            sb.AppendLine("user: member id 100245 aur start date aaj  -> {""intent"":""fill"",""items"":[{""field"":""MemberId"",""value"":""100245""},{""field"":""Start Date"",""value"":""today""}]}")
            sb.AppendLine("user: form khali karo  -> {""intent"":""clear"",""field"":""all""}")
            sb.AppendLine("user: ghar le chalo  -> {""intent"":""home""}")
        Else
            sb.AppendLine("")
            sb.AppendLine("MODE=short: only classify the intent + single primary target; do not over-extract.")
        End If

        Return sb.ToString()
    End Function

    '---------------- helpers ----------------

    Private Function NewMsg(ByVal role As String, ByVal content As String) As Dictionary(Of String, Object)
        Dim m As New Dictionary(Of String, Object)
        m("role") = role
        m("content") = content
        Return m
    End Function

    ' Pull choices[0].message.content out of the Mistral JSON response.
    Private Function ExtractContent(ByVal ser As JavaScriptSerializer, ByVal respText As String) As String
        Try
            Dim root As Dictionary(Of String, Object) = ser.Deserialize(Of Dictionary(Of String, Object))(respText)
            If root Is Nothing OrElse Not root.ContainsKey("choices") Then Return ""
            ' JavaScriptSerializer maps JSON arrays to Object() / ArrayList -> use IList.
            Dim choices = TryCast(root("choices"), System.Collections.IList)
            If choices Is Nothing OrElse choices.Count = 0 Then Return ""
            Dim first = TryCast(choices(0), Dictionary(Of String, Object))
            If first Is Nothing OrElse Not first.ContainsKey("message") Then Return ""
            Dim msg = TryCast(first("message"), Dictionary(Of String, Object))
            If msg Is Nothing OrElse Not msg.ContainsKey("content") Then Return ""
            Return Nz(msg("content"))
        Catch
            Return ""
        End Try
    End Function

    ' Remove ```json ... ``` fences if the model adds them despite instructions.
    Private Function StripFences(ByVal s As String) As String
        If s Is Nothing Then Return ""
        s = s.Trim()
        If s.StartsWith("```") Then
            Dim nl As Integer = s.IndexOf(ControlChars.Lf)
            If nl >= 0 Then s = s.Substring(nl + 1)
            If s.EndsWith("```") Then s = s.Substring(0, s.Length - 3)
        End If
        Return s.Trim()
    End Function

    Private Function OneLine(ByVal s As String) As String
        If s Is Nothing Then Return ""
        Return s.Replace(ControlChars.Cr, " ").Replace(ControlChars.Lf, ", ").Trim()
    End Function

    Private Function Nz(ByVal o As Object) As String
        If o Is Nothing Then Return ""
        Return o.ToString()
    End Function

    ' Minimal JSON string encoder for our own error messages.
    Private Function JsStr(ByVal s As String) As String
        If s Is Nothing Then s = ""
        Dim sb As New StringBuilder()
        sb.Append(""""c)
        For Each c As Char In s
            Select Case c
                Case """"c : sb.Append("\""")
                Case "\"c : sb.Append("\\")
                Case ControlChars.Cr : sb.Append("\r")
                Case ControlChars.Lf : sb.Append("\n")
                Case ControlChars.Tab : sb.Append("\t")
                Case Else : sb.Append(c)
            End Select
        Next
        sb.Append(""""c)
        Return sb.ToString()
    End Function

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
