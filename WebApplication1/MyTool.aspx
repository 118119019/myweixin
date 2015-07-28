<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" CodeBehind="MyTool.aspx.cs" Inherits="WebApplication1.MyTool" %>

<!DOCTYPE html>

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
        <asp:TextBox ID="txtResult" runat="server" TextMode="MultiLine"></asp:TextBox>
        <br />
        <div class="mydiv">
            <h2>推送消息管理</h2>
            <label>消息条数</label>
            <asp:DropDownList ID="ddlSendCount" runat="server">
                <asp:ListItem Text="3条" Value="3"></asp:ListItem>
                <asp:ListItem Text="4条" Value="4"></asp:ListItem>
                <asp:ListItem Text="5条" Value="5"></asp:ListItem>
                <asp:ListItem Text="6条" Value="6"></asp:ListItem>
            </asp:DropDownList>
            <br />
            /// 请注意：<br />
            /// 选择完消息条数之后请点保存条数：<br />
            <asp:Button runat="server" ID="btnSaveSendCount" OnClick="btnSaveSendCount_Click" Text="保存每次推送消息条数" />
            <br />
            <br />
            /// 请注意：<br />
            /// 1、该接口暂时仅提供给已微信认证的服务号：<br />
            /// 2、虽然开发者使用高级群发接口的每日调用限制为100次，但是用户每月只能接收4条，请小心测试：<br />
            /// 3、无论在公众平台网站上，还是使用接口群发，用户每月只能接收4条群发消息，多于4条的群发将对该用户发送失败。
            <br />
            <asp:Button ID="btnSendAll" runat="server" OnClick="btnSendAll_Click" Text="服务号 图文消息 推送" />
            <br />
            <br />
            <br />
            <asp:Button ID="btnShortSendAll" runat="server" OnClick="btnShortSendAll_Click" Text="订阅号 图文消息 推送" />
        </div>
        <div class="mydiv">
            <h2>定时任务配置</h2>
            <asp:DropDownList ID="ddlWeek" runat="server">
                <asp:ListItem Text="周一" Value="MON"></asp:ListItem>
                <asp:ListItem Text="周二" Value="TUE"></asp:ListItem>
                <asp:ListItem Text="周三" Value="WED"></asp:ListItem>
                <asp:ListItem Text="周四" Value="THU"></asp:ListItem>
                <asp:ListItem Text="周五" Value="FRI"></asp:ListItem>
                <asp:ListItem Text="周六" Value="SAT"></asp:ListItem>
                <asp:ListItem Text="周日" Value="SUN"></asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="ddlHour" runat="server">
            </asp:DropDownList>
            <asp:DropDownList ID="ddlMinute" runat="server">
            </asp:DropDownList>
            <asp:Button ID="btnSaveQuartzCfg" OnClick="btnSaveQuartzCfg_Click" runat="server" Text="保存定时任务配置" />
            <br />
            <label style="color: red;">保存完定时任务配置之后 请手动重新启动 定时程序</label>
        </div>




        <div style="display: none;">
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
            <div style="display: none;">
                调度相关
             <asp:Button ID="btnStar" runat="server" OnClick="btnStar_Click" Text="重启" />
                <asp:Button ID="btnClose" runat="server" OnClick="btnClose_Click" Text="停止" />
                <br />
                定时推送服务运行状态:
            <asp:Label ID="lbStatus" runat="server"></asp:Label>
                <asp:Button ID="btnQueryService" runat="server" OnClick="btnQueryService_Click" Text="查询推送服务状态" />
            </div>
        </div>



    </form>

</body>
</html>
