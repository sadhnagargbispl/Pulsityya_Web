Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Text
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Drawing.Drawing2D

Public Class clsCaptcha
    Public myHeight As Integer
    Public myWidth As Integer
    Public myText As String
    Public myfamilyNm As String
    Public myImage As Image
    Public Sub clsCaptcha()
        GenerateImage()
    End Sub
    Public Function GenerateImage() As Image
        ''Creates 32-bit Bitmap Image
        Dim ImgBitmap As New Bitmap(myWidth, myHeight, Drawing.Imaging.PixelFormat.Format32bppArgb)

        ''Create a graphics object for drawing.
        Dim g As Graphics
        g = Graphics.FromImage(ImgBitmap)
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        Dim rect As New Rectangle(0, 0, myWidth, myHeight)

        ''Fill Rectangle Background
        Dim hBrush = New HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White)
        g.FillRectangle(hBrush, rect)

        ''Set text font.
        Dim Sz As SizeF
        Dim fntSz As Decimal
        fntSz = 200

        Dim myfnt As Font
        myfnt = New Font(Me.myfamilyNm, 20, FontStyle.Bold)
        ''Adjust the font size until the text fits within the image.
        Do While Sz.Width > rect.Width
            fntSz = fntSz - 1
            myfnt = New Font(Me.myfamilyNm, fntSz, FontStyle.Bold)
            Sz = g.MeasureString(Me.myText, myfnt)
        Loop

        'Do While (Sz.Width > rect.Width)
        '    fntSz = fntSz - 1
        '    myfnt = New Font(Me.myfamilyNm, fntSz, FontStyle.Bold)
        '    Sz = g.MeasureString(Me.myText, myfnt)
        'End While

        ''Set up the text Alignment.
        Dim strFormat As New StringFormat()
        strFormat.Alignment = StringAlignment.Center
        strFormat.LineAlignment = StringAlignment.Center

        ''Create a path using the text and warp it randomly.
        Dim gPath As New GraphicsPath()
        ''gPath.AddString(Me.myText, myfnt.FontFamily, FontStyle.Bold, myfnt.Size, rect, strFormat)
        g.DrawString(myText, myfnt, Brushes.Indigo, 100, 30, strFormat)

        Dim newVar As Decimal
        newVar = 4.0F
        Dim points(3) As PointF
        Dim rndm As New Random()
        '' points(0) = 10.0F
        points(0).X = rndm.Next(rect.Width) / newVar
        points(0).Y = rndm.Next(rect.Height) / newVar

        points(1).X = rndm.Next(rect.Width) / newVar
        points(1).Y = rndm.Next(rect.Height) / newVar

        points(2).X = rndm.Next(rect.Width) / newVar
        points(2).Y = rndm.Next(rect.Height) / newVar

        points(3).X = rndm.Next(rect.Width) / newVar
        points(3).Y = rndm.Next(rect.Height) / newVar

        Dim mtrx = New Matrix
        mtrx.Translate(0.0F, 0.0F)

        gPath.Warp(points, rect, mtrx, WarpMode.Perspective, 0.0F)

        ''Draw the text.
        hBrush = New HatchBrush(HatchStyle.LargeConfetti, Color.LightGray, Color.DarkGray)
        g.FillPath(hBrush, gPath)

        ''Add some random noise.
        Dim m As Int16
        m = Math.Max(rect.Width, rect.Height)

        Dim x, y, w, h, r As Integer
        r = Convert.ToInt32(rect.Width * rect.Height / 30.0F)
        For i As Integer = 1 To r
            x = rndm.Next(rect.Width)
            y = rndm.Next(rect.Height)
            h = rndm.Next(m / 50)
            w = rndm.Next(m / 50)
            g.FillEllipse(hBrush, x, y, w, h)
        Next

        mtrx.Dispose()
        hBrush.Dispose()
        g.Dispose()

        Return ImgBitmap
    End Function
End Class








