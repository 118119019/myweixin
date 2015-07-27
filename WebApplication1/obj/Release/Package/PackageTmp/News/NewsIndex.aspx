<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewsIndex.aspx.cs" Inherits="WebApplication1.News.NewsIndex" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>龙岩就业</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, user-scalable=no, minimal-ui" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="format-detection" content="telephone=no, email=no" />
    <link href="../html/css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div style="margin: auto; text-align: center; width: 100%;">
                <img src="../image/news.jpg" style="height: 300px; width: 100%;" />
            </div>
            <div class="content">
                <div class="tab_list">
                    <table class="set_tab" style="width: 100%;">
                        <thead>
                            <tr>
                                <th><%=TitleName %>信息</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptList1" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><a href="News<%#Eval("Id") %>.html"><%#Eval("Name") %></a></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
