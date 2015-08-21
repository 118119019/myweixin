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
    <script src="../html/1_files/jquery.min.js"></script>

    <style>
        .news_list {
            overflow: hidden;
            width: 100%;
        }

            .news_list h4 {
                text-align: center;
                border: 1px solid #ddd;
                border-bottom-width: 2px;
                padding: 4px;
                line-height: 1.2;
                background: #0b3aa2;
                background: -webkit-gradient(linear, 0 0,0 bottom, from(#4b83ff), to(#0b3aa2));
                color: #fff;
                font-size: 1.5rem;
            }

            .news_list li {
                border-bottom: 1px solid #ddd;
                width: 100%;
            }

                .news_list li a {
                    display: block;
                    padding: 2% 3%;
                    color: #333;
                    font-size: 1.4rem;
                    overflow: hidden;
                    background: #fff;
                    display: table;
                    table-layout: fixed;
                }

                    .news_list li a span {
                        display: table-cell;
                        vertical-align: middle;
                        width: 100%;
                    }

                    .news_list li a img {
                        width: 50px;
                        height: 50px;
                        display: block;
                        margin-left: 10px;
                    }

                    .news_list li a:active {
                        background: #f9f9f9;
                    }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div class="content">
                <div class="news_list">
                    <h4><%=TitleName %>信息</h4>
                    <ul>
                        <asp:Repeater ID="rptList1" runat="server">
                            <ItemTemplate>
                                <li>
                                    <a href="NewsDetail.aspx?id=<%#Eval("Id") %>"><span><%#Eval("Name") %>
                                    </span>
                                        <img src="../image/icon1.jpg" alt="" /></a>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>


                </div>
            </div>
        </div>
    </form>
</body>
</html>
