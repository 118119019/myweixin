<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WelcomeEdit.aspx.cs" Inherits="WebApplication1.WelcomeEdit" %>

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
            <h2>关注消息开头文字</h2>
            <asp:TextBox ID="txtWelcome" Width="400" Height="100" runat="server" TextMode="MultiLine">
            </asp:TextBox>
            <br />
            <asp:Button ID="btnSaveWelcome" runat="server" Text="保存修改关注信息系" OnClick="btnSaveWelcome_Click" />
        </div>
    </form>

</body>
</html>