<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InvoicePage.aspx.vb" Inherits="InvoicePage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Invoice</title>
    <link href="https://fonts.googleapis.com/css2?family=Cormorant+Garamond:ital,wght@0,300;0,400;0,600;1,300;1,400&family=DM+Mono:wght@300;400;500&display=swap"
        rel="stylesheet">
    <style>
        *
        {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
        }
        body
        {
            background: #e8e8e8;
            font-family: 'Segoe UI' , Tahoma, Geneva, Verdana, sans-serif;
            padding: 30px 16px;
            color: #222;
        }
        .page
        {
            max-width: 720px;
            margin: 0 auto;
            background: #fff;
            border-radius: 10px;
            overflow: hidden;
            box-shadow: 0 4px 24px rgba(0,0,0,0.13);
        }
        .inner
        {
            padding: 36px 40px 28px;
        }
        /* ── BACK BUTTON ── */.back-btn-wrap
        {
            margin-bottom: 22px;
        }
        .back-btn
        {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            padding: 8px 20px;
            background: #f5f5f5;
            color: #333;
            border: 1px solid #ddd;
            border-radius: 6px;
            text-decoration: none;
            font-size: 14px;
            cursor: pointer;
            transition: background 0.2s, color 0.2s, border-color 0.2s;
        }
        .back-btn:hover
        {
            background: #222;
            color: #fff;
            border-color: #222;
        }
        /* ── HEADER ── */header
        {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 20px;
        }
        .brand
        {
            display: flex;
            align-items: center;
            gap: 12px;
        }
        .brand-mark
        {
            display: flex;
            align-items: center;
            gap: 10px;
        }
        .logo-box
        {
            width: 52px;
            height: 52px;
            border-radius: 8px;
            overflow: hidden;
            display: flex;
            align-items: center;
            justify-content: center;
            background: #f5f0e8;
            border: 1px solid #e0d0b0;
        }
        .logo-box img
        {
            width: 100%;
            height: 100%;
            object-fit: contain;
        }
        .company-name
        {
            font-size: 18px;
            font-weight: 700;
            color: #1a1a1a;
            letter-spacing: 0.3px;
        }
        .invoice-title-block .word-invoice
        {
            font-size: 30px;
            font-weight: 800;
            color: #c8a84b;
            letter-spacing: 2px;
            text-transform: uppercase;
        }
        /* ── DIVIDER ── */.rule
        {
            height: 2px;
            background: linear-gradient(90deg, #c8a84b 0%, #f0d080 60%, #e8e8e8 100%);
            margin-bottom: 22px;
            border-radius: 2px;
        }
        /* ── META ── */.meta-grid
        {
            display: flex;
            gap: 40px;
            margin-bottom: 24px;
        }
        .meta-label
        {
            font-size: 11px;
            text-transform: uppercase;
            letter-spacing: 1px;
            color: #999;
            margin-bottom: 4px;
        }
        .meta-value
        {
            font-size: 15px;
            color: #222;
        }
        /* ── PARTIES ── */.parties
        {
            display: flex;
            gap: 0;
            margin-bottom: 28px;
            position: relative;
        }
        .divider-vertical
        {
            width: 2px;
            background: #c8a84b;
            border-radius: 2px;
            margin: 0 28px;
            min-height: 80px;
        }
        .party-label
        {
            font-size: 11px;
            text-transform: uppercase;
            letter-spacing: 1px;
            color: #999;
            margin-bottom: 6px;
        }
        .party-name
        {
            font-size: 16px;
            font-weight: 700;
            color: #1a1a1a;
            margin-bottom: 6px;
        }
        .party-details
        {
            font-size: 13px;
            color: #555;
            line-height: 1.7;
        }
        /* ── LINE ITEMS ── */.items-section
        {
            margin-bottom: 0;
        }
        .items-header
        {
            display: grid;
            grid-template-columns: 1fr 80px 110px 110px;
            background: #1a1a1a;
            color: #c8a84b;
            padding: 10px 14px;
            border-radius: 6px 6px 0 0;
            font-size: 12px;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 0.8px;
        }
        .th.center
        {
            text-align: center;
        }
        .th.right
        {
            text-align: right;
        }
        .item-row
        {
            display: grid;
            grid-template-columns: 1fr 80px 110px 110px;
            padding: 14px 14px;
            border-bottom: 1px solid #f0f0f0;
            align-items: center;
            background: #fafafa;
        }
        .item-name
        {
            font-size: 15px;
            font-weight: 600;
            color: #1a1a1a;
            margin-bottom: 3px;
        }
        .item-desc
        {
            font-size: 12px;
            color: #888;
        }
        .item-qty
        {
            text-align: center;
            font-size: 15px;
            color: #333;
        }
        .item-rate
        {
            text-align: right;
            font-size: 15px;
            color: #333;
        }
        .item-amount
        {
            text-align: right;
            font-size: 15px;
            font-weight: 600;
            color: #1a1a1a;
        }
        /* ── TOTALS ── */.totals-block
        {
            display: flex;
            justify-content: flex-end;
            margin-top: 0;
            padding: 18px 14px 10px;
            background: #fafafa;
            border-radius: 0 0 6px 6px;
            border-top: 2px solid #e8e8e8;
        }
        .totals-table
        {
            width: 300px;
        }
        .totals-row
        {
            display: flex;
            justify-content: space-between;
            padding: 5px 0;
            font-size: 14px;
            color: #555;
            border-bottom: 1px dashed #ececec;
        }
        .totals-row:last-child
        {
            border-bottom: none;
        }
        .totals-row .label
        {
            font-weight: 500;
        }
        .totals-row .value
        {
            font-weight: 600;
            color: #333;
        }
        .total-final
        {
            margin-top: 8px;
            padding-top: 10px !important;
            border-top: 2px solid #c8a84b !important;
            border-bottom: none !important;
        }
        .total-final .label, .total-final .value
        {
            font-size: 16px;
            font-weight: 800;
            color: #1a1a1a;
        }
        /* ── FOOTER ── */.footer
        {
            background: #1a1a1a;
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 14px 40px;
            flex-wrap: wrap;
            gap: 8px;
        }
        .footer-text
        {
            font-size: 11px;
            color: #aaa;
            letter-spacing: 1px;
            text-transform: uppercase;
        }
        .footer-gold
        {
            font-size: 11px;
            color: #c8a84b;
            font-style: italic;
        }
        @media print
        {
            body
            {
                background: #fff;
                padding: 0;
            }
            .page
            {
                box-shadow: none;
                border-radius: 0;
            }
            .back-btn-wrap
            {
                display: none;
            }
        }
    </style>
</head>
<body>
    <form id="Form1" runat="server">
    <div class="page">
        <div class="inner">
            <!-- BACK BUTTON -->
            <div class="back-btn-wrap">
                <a href="ProductrequestDetail.aspx" class="back-btn">&#8592; Back</a>
            </div>
            <!-- HEADER -->
            <header>
                    <div class="brand">
                        <div class="brand-mark">
                            <div class="logo-box">
                                <img src='' alt="Company Logo" />
                            </div>
                            <span class="company-name">Overnet Trading Pvt. Ltd.</span>
                        </div>
                    </div>
                    <div class="invoice-title-block">
                        <div class="word-invoice">Invoice</div>
                    </div>
                </header>
            <!-- DIVIDER -->
            <div class="rule">
            </div>
            <!-- META -->
            <div class="meta-grid">
                <div class="meta-cell">
                    <div class="meta-label">
                        Bill Date</div>
                    <div class="meta-value">
                        <strong>
                            <asp:Literal ID="litIssueDate" runat="server" Text="" /></strong></div>
                </div>
                <div class="meta-cell">
                    <div class="meta-label">
                        Bill No.</div>
                    <div class="meta-value">
                        <strong>
                            <asp:Literal ID="LblOderNo" runat="server" Text="" /></strong></div>
                </div>
            </div>
            <!-- PARTIES -->
            <div class="parties">
                <div>
                    <div class="party-label">
                        From</div>
                    <div class="party-name">
                        Overnet Trading Pvt. Ltd.</div>
                    <div class="party-details">
                        Plot No. 34, 1st Floor, Sewak Park,<br />
                        Near Dwarka Mode Metro Station,<br />
                        Metro Pillar No. 773, Uttam Nagar,<br />
                        New Delhi 110059
                    </div>
                </div>
                <div class="divider-vertical">
                </div>
                <div>
                    <div class="party-label">
                        Bill To</div>
                    <div class="party-name">
                        <asp:Literal ID="litToName" runat="server" Text="" /></div>
                    <div class="party-details">
                        <asp:Literal ID="litToAddress" runat="server" Text="" />
                    </div>
                </div>
            </div>
            <!-- LINE ITEMS -->
            <div class="items-section">
                <div class="items-header">
                    <div class="th">
                        Description</div>
                    <div class="th center">
                        Qty</div>
                    <div class="th right">
                        Unit Rate</div>
                    <div class="th right">
                        Amount</div>
                </div>
                <div class="item-row">
                    <div>
                        <div class="item-name">
                            Gift Voucher</div>
                    </div>
                    <div class="item-qty">
                        15</div>
                    <div class="item-rate">
                        100.00</div>
                    <div class="item-amount">
                        1,500.00</div>
                </div>
            </div>
            <!-- TOTALS -->
            <div class="totals-block">
                <div class="totals-table">
                    <div class="totals-row">
                        <span class="label">Subtotal</span> <span class="value">1,500.00</span>
                    </div>
                    <div class="totals-row">
                        <span class="label">GST (0%)</span> <span class="value">0.00</span>
                    </div>
                    <div class="totals-row">
                        <span class="label">SGST (0%)</span> <span class="value">0.00</span>
                    </div>
                    <div class="totals-row">
                        <span class="label">CGST (0%)</span> <span class="value">0.00</span>
                    </div>
                    <div class="totals-row total-final">
                        <span class="label">Total Bill Amount</span> <span class="value">1,500.00</span>
                    </div>
                </div>
            </div>
        </div>
        <!-- /inner -->
        <!-- FOOTER -->
        <div class="footer">
            <span class="footer-text">THIS IS COMPUTER GENERATED INVOICE</span> <span class="footer-gold">
                This gift voucher can be redeemed as per the applicable Terms and Conditions.</span>
        </div>
    </div>
    
    </form>
</body>
</html>
