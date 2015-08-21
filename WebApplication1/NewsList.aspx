<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewsList.aspx.cs" Inherits="WebApplication1.NewsList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript">
        var OnCheckDelete = function () {
            return confirm("是否将此信息删除?");
        };
    </script>
    <style>
        .cion-bg1 {
            background-color: #f5f4f0;
        }

        /***账单明细***/
        .cion-bg1 {
            background-color: #f5f4f0;
            padding: 15px;
        }

            .cion-bg1 .line {
                padding: 10px 0;
                line-height: 27px;
            }

            .cion-bg1 strong {
                color: #464646;
                font-size: 16px;
                vertical-align: middle;
            }

        .time-text {
            width: 128px;
            height: 25px;
            border: 1px solid #dfdbd2;
            background: #fff url(../images/cion.gif) no-repeat 103px -410px;
            vertical-align: middle;
            text-indent: 8px;
        }

        .text1 {
            width: 188px;
            height: 25px;
            line-height: 25px;
            text-indent: 8px;
            border: 1px solid #dfdbd2;
            background-color: #fff;
            vertical-align: middle;
        }

        .cion-bg1 .line input {
            vertical-align: middle;
        }

        .cion-bg1 .line label {
            line-height: 27px;
        }

        .cion-bg1 .line .radio_box {
            width: 260px;
            display: inline-block;
            vertical-align: middle;
        }

        .cion-bg1 .line .select_box {
            width: 270px;
            display: inline-block;
            vertical-align: middle;
        }

        .hRadio {
            padding-left: 22px;
            display: inline-block;
            background: url(../images/cion.gif) no-repeat 0 -374px;
            vertical-align: middle;
        }

        .hRadio_Checked {
            background: url(../images/cion.gif) no-repeat 0 -350px;
        }

        .cion-bg1 .line .button-1 {
            vertical-align: middle;
            padding: 0 20px;
            font-weight: bold;
        }

        .cion-bg1 .line .selectbtn {
            width: 178px !important;
        }

        .cion-bg1 .selectbtn ul {
            width: 178px !important;
        }

            .cion-bg1 .selectbtn ul li {
                width: 178px !important;
            }

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
        <div class="cion-bg1">
            <div class="add-line" style="display: inline">
                <span class="button-box" style="float: right">
                    <asp:LinkButton class="addnew" ID="btnAdd" runat="server" OnClick="btnAdd_Click">新增</asp:LinkButton>
                </span>
            </div>
        </div>
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
                                    |   <a href="News/NewsDetail.aspx?id=<%#Eval("id") %>" target="_blank">查看</a>|
                                     <asp:LinkButton ID="lkBtnDelReply" CommandName="Del" CommandArgument='<%#Eval("Id")  %>'
                                         OnClientClick="return OnCheckDelete();" runat="server">删除</asp:LinkButton>
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
