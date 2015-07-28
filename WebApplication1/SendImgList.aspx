<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendImgList.aspx.cs" Inherits="WebApplication1.SendImgList" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
        .mydiv {
            border: 1px solid #ddd;
            margin-bottom: 10px;
            margin-right: 0;
            margin-left: 0;
            background-color: #fff;
            border-width: 1px;
            border-radius: 4px 4px 0 0;
            -webkit-box-shadow: none;
            box-shadow: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:TextBox ID="txtResult" Width="500" runat="server" TextMode="MultiLine"></asp:TextBox>
        <br />
        <div class="mydiv">
            <h1>推送相关图片列表 （推荐480*300）</h1>
            <ul>
                <li style="margin-bottom: 20px;">第一条图片                   
                    <asp:Image ID="img0" runat="server" />
                    <asp:FileUpload ID="uploadImgUrl0" runat="server" />
                    <asp:Label ID="labImgError0" runat="server"></asp:Label>
                </li>
                <li style="margin-bottom: 20px;">第二条图片
                    <asp:Image ID="img1" runat="server" />
                    <asp:FileUpload ID="uploadImgUrl1" runat="server" />
                    <asp:Label ID="labImgError1" runat="server"></asp:Label>
                </li>
                <li style="margin-bottom: 20px;">第三条图片                  
                    <asp:Image ID="img2" runat="server" />
                    <asp:FileUpload ID="uploadImgUrl2" runat="server" />
                    <asp:Label ID="labImgError2" runat="server"></asp:Label>
                </li>
                <li style="margin-bottom: 20px;">第四条图片                  
                    <asp:Image ID="img3" runat="server" />
                    <asp:FileUpload ID="uploadImgUrl3" runat="server" />
                    <asp:Label ID="labImgError3" runat="server"></asp:Label>
                </li>
                <li style="margin-bottom: 20px;">第五条图片                  
                    <asp:Image ID="img4" runat="server" />
                    <asp:FileUpload ID="uploadImgUrl4" runat="server" />
                    <asp:Label ID="labImgError4" runat="server"></asp:Label>
                </li>
                <li style="margin-bottom: 20px;">第六条图片                  
                    <asp:Image ID="img5" runat="server" />
                    <asp:FileUpload ID="uploadImgUrl5" runat="server" />
                    <asp:Label ID="labImgError5" runat="server"></asp:Label>
                </li>
                <li>
                    <asp:Button ID="btnSaveImg" runat="server" OnClick="btnSaveImg_Click" Text="上传图片到微信服务端" />
                </li>
            </ul>
        </div>
    </form>

</body>
</html>
