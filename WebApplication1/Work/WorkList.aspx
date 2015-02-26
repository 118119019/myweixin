<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkList.aspx.cs" Inherits="WebApplication1.Work.WorkList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title></title>
    <!-- 新 Bootstrap 核心 CSS 文件 -->
    <link rel="stylesheet" href="http://cdn.bootcss.com/bootstrap/3.3.2/css/bootstrap.min.css" />

    <!-- 可选的Bootstrap主题文件（一般不用引入） -->
    <link rel="stylesheet" href="http://cdn.bootcss.com/bootstrap/3.3.2/css/bootstrap-theme.min.css" />

    <!-- jQuery文件。务必在bootstrap.min.js 之前引入 -->
    <script src="http://cdn.bootcss.com/jquery/1.11.2/jquery.min.js"></script>

    <!-- 最新的 Bootstrap 核心 JavaScript 文件 -->
    <script src="http://cdn.bootcss.com/bootstrap/3.3.2/js/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-xs-12 col-sm-12">
            <div class="row my_panel_title">
                <h2>查询招聘岗位信息</h2>
            </div>

        </div>
        <div class="container-fluid">
            <asp:DropDownList ID="ddlTime" runat="server">
                <asp:ListItem Value="00">发布时间不限</asp:ListItem>
                <asp:ListItem Value="01">一周内</asp:ListItem>
                <asp:ListItem Value="02">百日内</asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:DropDownList ID="ddlWork" runat="server">
                <asp:ListItem Value="0000000">工作岗位--不限</asp:ListItem>
                <asp:ListItem Value="1050000">单位负责人</asp:ListItem>
                <asp:ListItem Value="1050100">部门经理</asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:DropDownList ID="ddlPlace" runat="server">
                <asp:ListItem Value="35080000">所属劳动部门--不限  </asp:ListItem>
                <asp:ListItem Value="35080100">龙岩市劳动就业中心 </asp:ListItem>
                <asp:ListItem Value="35080200">新罗区劳动就业中心</asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:Button ID="btnSearch" runat="server" Text="搜 索" CssClass="button-1" OnClick="btnSearch_Click" />
        </div>
        <div>
            <ul>
                <asp:Repeater ID="rptList" runat="server">
                    <ItemTemplate>
                        <li><a href="http://fjlylm.com/<%#Eval("url") %>"><%# Eval("name") %></a></li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
    </form>
</body>
</html>
