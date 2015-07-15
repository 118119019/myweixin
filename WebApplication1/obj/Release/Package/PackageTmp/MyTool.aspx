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
        </div>

        <div>
            <p>
                <img width='100%' src='https://mmbiz.qlogo.cn/mmbiz/iaXDmvibibwTLXGYrdN08iabrTlgiaRDb9uvHAhGeOLibAZdiaF5BiapfMD08dvHIXKUvHnqvcIWK9XrD1G9pamRhEMWibA/0' />
            </p>
            <p>
                <br />
            </p>
            <fieldset style='border: 0px; margin: 0.5em 0px; padding: 0px; box-sizing: border-box;' class='wxqq-borderTopColor'>
                <section style='margin-left: 1%; border: 1px solid rgb(0, 187, 236); border-top-left-radius: 0px; border-top-right-radius: 5px; border-bottom-right-radius: 5px; border-bottom-left-radius: 0px; font-size: 1em; font-family: inherit; font-weight: inherit; text-align: inherit; text-decoration: inherit; color: rgb(52, 54, 60); background-color: rgb(255, 255, 255); box-sizing: border-box;'
                    class='wxqq-borderTopColor wxqq-borderRightColor wxqq-borderBottomColor wxqq-borderLeftColor'>
                    <section style='margin-top: 5%; float: left; margin-right: 8px; margin-left: -8px; font-size: 0.8em; font-family: inherit; font-style: inherit; font-weight: inherit; text-decoration: inherit; color: rgb(255, 255, 255); background-color: transparent; border-color: rgb(0,187,236); box-sizing: border-box;'
                        class='wxqq-borderTopColor wxqq-borderRightColor wxqq-borderBottomColor wxqq-borderLeftColor'>
                        <span style='display: inline-block; padding: 0.3em 0.5em; border-top-left-radius: 0px; border-top-right-radius: 0.5em; border-bottom-right-radius: 0.5em; border-bottom-left-radius: 0px; font-size: 1em; background-color: rgb(0,187,236); font-family: inherit; box-sizing: border-box;'
                            class='wxqq-bg'>
                            <section class='wxqq-borderTopColor' style='box-sizing: border-box;'>
                                福建丹海床垫有限公司名称
                            </section>
                        </span>
                        <section style='width: 0px; border-right-width: 4px; border-right-style: solid; border-right-color: rgb(0,187,236); border-top-width: 4px; border-top-style: solid; border-top-color: rgb(0,187,236); border-left-width: 4px !important; border-left-style: solid !important; border-left-color: transparent !important; border-bottom-width: 4px !important; border-bottom-style: solid !important; border-bottom-color: transparent !important; box-sizing: border-box;'
                            class='wxqq-borderTopColor wxqq-borderRightColor wxqq-borderBottomColor wxqq-borderLeftColor'>
                            公司简介相关
                        </section>
                    </section>
                    <section style='margin-top: 5%; padding: 0px 8px; font-size: 1.5em; font-family: inherit; font-weight: inherit; text-align: inherit; text-decoration: inherit; box-sizing: border-box;'
                        class='wxqq-borderTopColor'>
                        <section class='wxqq-borderTopColor' style='box-sizing: border-box;'>

                            <br />

                        </section>

                    </section>

                    <section style='clear: both; box-sizing: border-box;' class='wxqq-borderTopColor'></section>

                    <section style='padding: 8px; box-sizing: border-box; "' class='wxqq-borderTopColor'>

                        <p>
                            编辑文字的时候，提倡大家复制素材到微信公众平台素材管理里面进行编辑，本编士大夫士大夫是的发生发生的发生的顶顶顶顶顶大大大

                        </p>

                        <p>

                            <br />

                        </p>

                        <p>
                            职位详情

                        </p>

                        <blockquote class='wxqq-borderTopColor wxqq-borderRightColor wxqq-borderBottomColor wxqq-borderLeftColor'
                            style='margin: 0px; padding: 15px; border: 3px dashed rgb(0, 187, 236); border-top-left-radius: 10px; border-top-right-radius: 10px; border-bottom-right-radius: 10px; border-bottom-left-radius: 10px;'>

                            <p>

                                <span style='font-family: inherit; font-size: 1em; font-weight: inherit; text-align: inherit; text-decoration: inherit;'>招聘职位: &nbsp; &nbsp;部门经理 　 &nbsp; &nbsp;</span><br />

                            </p>

                            <p>
                                工作方式: &nbsp; &nbsp;全职 　&nbsp; &nbsp; &nbsp;

                            </p>

                            <p>
                                最低月薪: &nbsp; &nbsp;4500　 &nbsp; &nbsp;

                            </p>

                            <p>
                                招聘人数: &nbsp; &nbsp;2人，其中：男0人，女0人，不限2人　 &nbsp; &nbsp;招聘对象: &nbsp; &nbsp;

                            </p>

                            <p>
                                年龄： &nbsp; &nbsp;不限 　 &nbsp; &nbsp;身高: &nbsp; &nbsp;视力: &nbsp; &nbsp;户口要求: &nbsp; &nbsp;不限 　 &nbsp; &nbsp;

                            </p>

                            <p>
                                文化程度: &nbsp; &nbsp;大专 　 &nbsp; &nbsp;技术等级: &nbsp; &nbsp;无/未说明 　 &nbsp; &nbsp;职业资格证书: &nbsp;&nbsp;

                            </p>

                            <p>
                                要求专业: &nbsp; &nbsp;用工形式: &nbsp; &nbsp;联系方式: &nbsp; &nbsp;

                            </p>

                            <p>
                                登记日期: &nbsp; &nbsp;2015-6-8　 &nbsp; &nbsp;有效日期: &nbsp; &nbsp;2015-7-8　 &nbsp; &nbsp;

                            </p>

                            <p>
                                其它要求: &nbsp; &nbsp;具有良好的沟通能力及销售技巧

                            </p>

                        </blockquote>
                        <p>
                            联系方式
                        </p>
                        <blockquote class='wxqq-borderTopColor wxqq-borderRightColor wxqq-borderBottomColor wxqq-borderLeftColor'
                            style='margin: 0px; padding: 15px; border: 3px dashed rgb(0, 187, 236); border-top-left-radius: 10px; border-top-right-radius: 10px; border-bottom-right-radius: 10px; border-bottom-left-radius: 10px;'>
                            <p class='ue_t'>
                                可在这输入内容， 微信编辑器 - 微信图文排版,微信图文编辑器,微信公众号编辑器，微信编辑首选。

                            </p>
                        </blockquote>
                        <p>
                            <br />
                        </p>
                        <p>
                            <br />
                        </p>
                    </section>
                </section>
            </fieldset>
            <p>
                <br />
            </p>
            <p>
                <img width='100%' src='https://mmbiz.qlogo.cn/mmbiz/iaXDmvibibwTLUAqg2cUWlqgcoLtmRHicBkdPW9GUw6kA9UBX4m1jibPj9tgFQicbnxYrs3kNE1cxu4pqfYTsby1x2cw/0' />
            </p>
        </div>
    </form>

</body>
</html>
