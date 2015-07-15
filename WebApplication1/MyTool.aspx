<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" CodeBehind="MyTool.aspx.cs" Inherits="WebApplication1._Default" %>

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
        <br />
        <div>
            调度相关
             <asp:Button ID="btnStar" runat="server" OnClick="btnStar_Click" Text="重启" />
            <asp:Button ID="btnClose" runat="server" OnClick="btnClose_Click" Text="停止" />
            <br />
            定时推送服务运行状态:
            <asp:Label ID="lbStatus" runat="server"></asp:Label>
            <asp:Button ID="btnQueryService" runat="server" OnClick="btnQueryService_Click" Text="查询推送服务状态" />
        </div>
        <div>
            群发消息接口测试
            <asp:Button ID="btnSendMessage" runat="server" OnClick="btnSendMessage_Click" Text="消息发送" />
        
              <asp:Button ID="btnSendAll" runat="server" OnClick="btnSendAll_Click" Text="全部消息发送" />
        </div>

       
    </form>

</body>
</html>
