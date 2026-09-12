<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Reply.aspx.cs" Inherits="Reply" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=Session["Title"].ToString ()%></title>
    <link rel="stylesheet" href="assets/css/app.min.css">
    <link rel="stylesheet" href="assets/css/style.css">
    <link rel="stylesheet" href="assets/css/components.css">
    <link rel="stylesheet" href="assets/bundles/jqvmap/dist/jqvmap.min.css">
    <link rel="stylesheet" href="assets/css/custom.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css">
    <style>
        body {
            background: #f4f6fb;
            font-family: 'Segoe UI', Tahoma, sans-serif;
            margin: 0;
            padding: 0;
        }

        .reply-wrapper {
            max-width: 760px;
            margin: 40px auto;
            padding: 0 15px;
        }

        .reply-card {
            background: #fff;
            border-radius: 14px;
            box-shadow: 0 6px 24px rgba(0, 0, 0, 0.08);
            overflow: hidden;
            border: 1px solid #eef0f5;
        }

        .reply-header {
            background: linear-gradient(135deg, #4361ee 0%, #3a0ca3 100%);
            color: #fff;
            padding: 20px 28px;
            display: flex;
            align-items: center;
            gap: 12px;
        }

            .reply-header i {
                font-size: 22px;
            }

            .reply-header h5 {
                margin: 0;
                font-size: 19px;
                font-weight: 600;
                letter-spacing: 0.3px;
            }

        .reply-body {
            padding: 28px 32px 34px;
        }

        .form-group {
            margin-bottom: 22px;
        }

            .form-group label {
                display: block;
                font-size: 13.5px;
                font-weight: 600;
                color: #2b2d42;
                margin-bottom: 8px;
                letter-spacing: 0.2px;
            }

        .red {
            color: #e63946;
            margin-left: 3px;
        }

        .field-box {
            width: 100%;
            border: 1.5px solid #e0e4ec;
            border-radius: 10px;
            padding: 12px 14px;
            font-size: 14px;
            color: #333;
            background: #f9fafc;
            box-sizing: border-box;
            transition: border-color 0.2s, box-shadow 0.2s;
        }

            .field-box:focus {
                outline: none;
                border-color: #4361ee;
                box-shadow: 0 0 0 3px rgba(67, 97, 238, 0.12);
                background: #fff;
            }

        /* Read-only labels ko field jaisa dikhane ke liye */
        span.field-box {
            display: block;
            min-height: 20px;
            background: #eef1f7;
            color: #444;
            font-weight: 500;
        }

        textarea.field-box {
            resize: vertical;
            min-height: 90px;
        }

        .error {
            color: #e63946;
            font-size: 13px;
            font-weight: 500;
            display: block;
            margin-bottom: 10px;
        }

        .info-note {
            font-size: 12.5px;
            color: #8a8f9c;
            margin-top: 4px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="reply-wrapper">
            <div class="reply-card">

                <div class="reply-header">
                    <i class="fa-solid fa-comment-dots"></i>
                    <h5>Complaint Reply</h5>
                </div>

                <div class="reply-body">

                    <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>

                    <div class="form-group">
                        <label>Complaint Type <span class="red">*</span></label>
                        <asp:Label CssClass="field-box" ID="LblCType" runat="server"></asp:Label>
                    </div>

                    <div class="form-group">
                        <label>Complaint <span class="red">*</span></label>
                        <asp:TextBox CssClass="field-box" ID="TxtComplaint" ReadOnly="true" TextMode="MultiLine" Rows="3" runat="server"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>Previous Reply <span class="red">*</span></label>
                        <asp:TextBox CssClass="field-box" ID="TxtPreReply" ReadOnly="true" TextMode="MultiLine" Rows="4" runat="server"></asp:TextBox>
                        <span class="info-note">This is the last reply sent for this complaint.</span>
                    </div>

                </div>
            </div>
        </div>
    </form>
</body>
</html>