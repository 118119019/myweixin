<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewsList.aspx.cs" Inherits="WebApplication1.NewsList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
        .table-box2 {
            margin-top: 20px;
            overflow: hidden;
        }

            .table-box2 .table-2 {
                width: 100%;
                text-align: center;
                margin-left: -2px;
            }

                .table-box2 .table-2 th {
                    height: 25px;
                    line-height: 25px;
                    padding: 10px 0;
                    background: #1f4e7a url(../images/cion.gif) no-repeat -56px -505px;
                    color: #fff;
                }

                .table-box2 .table-2 .th-widt1 {
                    width: 56px;
                }

                .table-box2 .table-2 .th-widt2 {
                    width: 85px;
                }

                .table-box2 .table-2 td {
                    padding: 4px 0;
                    border-bottom: 1px solid #dcdcdc;
                    background: #f5f4f0;
                }

                .table-box2 .table-2 .td-odd td {
                    background: #f5f4f0 url(../images/cion.gif) no-repeat -56px -440px;
                }

            .table-box2 .tab-title {
                padding: 30px 8px 25px;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="table-box2">
            <table class="table-2">
                <thead>
                    <tr>
                        <th width="10%">分类</th>
                        <th width="25%">名称</th>
                        <th width="10%">操作</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptList1" OnItemCommand="rptList1_ItemCommand" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%# showTypeName(Eval("Type").ToString()) %>
                                </td>
                                <td>
                                    <%#Eval("Name") %>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lkBtnEdit" CommandName="Edit" CommandArgument='<%#Eval("Id")  %>' runat="server">编辑</asp:LinkButton>
                                    |   <a href="News/News<%#Eval("id") %>.html" target="_blank">查看</a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>

        </div>
    </form>
</body>
</html>
