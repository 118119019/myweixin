﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication1.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        body {
            margin: 0;
            padding: 0;
        }

        .wrap {
            margin: 150px auto;
            width: 380px;
            overflow: hidden;
            float: none;
            background-color: #000000;
        }

        .loginForm {
            box-shadow: 0 0 2px rgba(0, 0, 0, 0.2), 0 1px 1px rgba(0, 0, 0, 0.2), 0 3px 0 #fff, 0 4px 0 rgba(0, 0, 0, 0.2), 0 6px 0 #fff, 0 7px 0 rgba(0, 0, 0, 0.2);
            position: absolute;
            z-index: 0;
            background-color: #FFF;
            border-radius: 4px;
            height: 240px;
            width: 380px;
            background: -webkit-gradient(linear, left top, left 24, from(#EEE), color-stop(4%, #FFF), to(#EEE));
            background: -moz-linear-gradient(top, #EEE, #FFF 1px, #EEE 24px);
            background: -o-linear-gradient(top, #EEEEEE, #FFFFFF 1px, #EEEEEE 24px);
        }

            .loginForm:before {
                content: '';
                position: absolute;
                z-index: -1;
                border: 1px dashed #CCC;
                top: 5px;
                bottom: 5px;
                left: 5px;
                right: 5px;
                box-shadow: 0 0 0 1px #FFF;
            }

            .loginForm h1 {
                text-shadow: 0 1px 0 rgba(255, 255, 255, .7), 0px 2px 0 rgba(0, 0, 0, .5);
                text-transform: uppercase;
                text-align: center;
                color: #666;
                line-height: 3em;
                margin: 16px 0 20px 0;
                letter-spacing: 4px;
                font: normal 26px/1 Microsoft YaHei, sans-serif;
            }

        fieldset {
            border: none;
            padding: 10px 10px 0;
        }

            fieldset input[type=text] {
                background: 4px 5px no-repeat;
            }

            fieldset input[type=password] {
                background: 4px 5px no-repeat;
            }

            fieldset input[type=text], fieldset input[type=password] {
                width: 100%;
                line-height: 2em;
                font-size: 12px;
                height: 30px;
                border: none;
                padding: 3px 4px 3px 2.2em;
                width: 300px;
            }

            fieldset input[type=submit] {
                text-align: center;
                padding: 2px 20px;
                line-height: 2em;
                border: 1px solid #113DEE;
                border-radius: 3px;
                background: -webkit-gradient(linear, left top, left 24, from(#113DEE), color-stop(0%, #0938F7), to(#113DEE));
                background: -moz-linear-gradient(top, #FF6900, #FF9800 0, #FF6900 24px);
                background: -o-linear-gradient(top, #FF6900, #FF9800 0, #FF6900 24px);
                filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#FF9800', endColorstr='#FF6900');
                -ms-filter: "progid:DXImageTransform.Microsoft.gradient(startColorstr='#FF9800', endColorstr='#FF6900')";
                height: 30px;
                cursor: pointer;
                letter-spacing: 4px;
                margin-left: 10px;
                color: #FFF;
                font-weight: bold;
            }

                fieldset input[type=submit]:hover {
                    background: -webkit-gradient(linear, left top, left 24, from(#FF9800), color-stop(0%, #FF6900), to(#FF9800));
                    background: -moz-linear-gradient(top, #FF9800, #FF6900 0, #FF9800 24px);
                    background: -o-linear-gradient(top, #FF6900, #FF6900 0, #FF9800 24px);
                    filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#FF6900', endColorstr='#FF9800');
                    -ms-filter: "progid:DXImageTransform.Microsoft.gradient(startColorstr='#FF6900', endColorstr='#FF9800')";
                }

        .inputWrap {
            background: -webkit-gradient(linear, left top, left 24, from(#FFFFFF), color-stop(4%, #EEEEEE), to(#FFFFFF));
            background: -moz-linear-gradient(top, #FFFFFF, #EEEEEE 1px, #FFFFFF 24px);
            background: -o-linear-gradient(top, #FFFFFF, #EEEEEE 1px, #FFFFFF 24px);
            border-radius: 3px;
            border: 1px solid #CCC;
            margin: 10px 10px 0;
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#EEEEEE', endColorstr='#FFFFFF');
            -ms-filter: "progid:DXImageTransform.Microsoft.gradient(startColorstr='#EEEEEE', endColorstr='#FFFFFF')";
        }

        fieldset input[type=checkbox] {
            margin-left: 10px;
            vertical-align: middle;
        }

        fieldset a {
            color: blue;
            font-size: 12px;
            margin: 6px 0 0 10px;
            text-decoration: none;
        }

            fieldset a:hover {
                text-decoration: underline;
            }

        fieldset span {
            font-size: 12px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="wrap">
            <section class="loginForm">
                <header>
                    <h1>登录</h1>
                </header>
                <div class="loginForm_content">
                    <fieldset>
                        <div class="inputWrap">
                            <asp:TextBox ID="txtUsername" runat="server" placeholder="账号"></asp:TextBox>
                        </div>
                        <div class="inputWrap">
                            <asp:TextBox type="password" ID="txtPassword" runat="server" placeholder="请输入密码"></asp:TextBox>
                        </div>
                        <div class="inputWrap">
                            <asp:Label ID="lbError" runat="server"></asp:Label>
                        </div>
                    </fieldset>
                    <fieldset>
                        <asp:Button ID="btnLogin" runat="server" Text="登陆" OnClick="btnLogin_Click" />
                    </fieldset>
                </div>
            </section>
        </div>
    </form>
</body>
</html>
