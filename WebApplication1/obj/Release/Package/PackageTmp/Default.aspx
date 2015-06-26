<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1._Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:TextBox ID="txtResult" runat="server" TextMode="MultiLine"></asp:TextBox>
        <br />
        行业
        <asp:TextBox ID="txtIndustry" Text="01" runat="server"></asp:TextBox>
        学历
        <asp:TextBox ID="txtDegree" Text="10" runat="server"></asp:TextBox>
        工作
        <asp:TextBox ID="txtWork" Text="1000000" runat="server"></asp:TextBox>
        登记日期
        <asp:TextBox ID="txtRegDate" Text="" runat="server"></asp:TextBox>
        截止日期
        <asp:TextBox ID="txtEffectDate" Text="" runat="server"></asp:TextBox>
        <asp:Button ID="btnQuery" Text="查询" runat="server" OnClick="btnQuery_Click" />
    </form>
</body>
</html>
