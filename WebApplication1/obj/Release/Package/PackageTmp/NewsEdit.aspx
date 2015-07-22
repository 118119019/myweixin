<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewsEdit.aspx.cs" Inherits="WebApplication1.NewsEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="html/1_files/jquery.min.js"></script>
    <script src="ueditor/ueditor.config.js" type="text/javascript"></script>
    <script src="ueditor/ueditor.all.js" type="text/javascript"></script>
    <script src="ueditor/lang/zh-cn/zh-cn.js"></script>
    <script type="text/javascript">
        var Encode = function () {
            $("[id$='hidContent']").val(editor.getContent());
            var value = $("#hidContent").val();
            while (value.indexOf("#") != -1) {
                value = value.replace("#", "%23");
            }
            while (value.indexOf("<") != -1) {
                value = value.replace("<", "&lt;");
            }
            while (value.indexOf(">") != -1) {
                value = value.replace(">", "&gt;");
            }
            $("#hidContent").val(value);

        }
        var CheckValid = function () {
            var title = $.trim($("[id$='txtTitle']").val());
            if (title == "") {
                alert("标题不能为空");
                return false;
            }
            $("[id$='hidContent']").val(editor.getContent());
            var content = $('#hidContent').val();
            if (content == "" || content.length < 50) {
                alert("文章未填或必须超过50个字的内容");
                return false;
            }
            Encode();
            $('#ueditor_textarea_editorValue').val("");

            return true;
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hidContent" runat="server" />
        <div>
            文章名称:
            <asp:TextBox ID="txtTitle" Width="400" MaxLength="100" runat="server"></asp:TextBox>
            (用在链接名称上)
        </div>
        <div id="myEditor" style="margin-top: 20px; width: 1024px; height: 300px; cursor: text;">
        </div>
        <script type="text/javascript">
            var editor = new baidu.editor.ui.Editor({
                toolbars: [[
'fullscreen', 'source', '|', 'undo', 'redo', '|',
'bold', 'italic', 'underline', 'fontborder', 'strikethrough', 'superscript', 'subscript', 'removeformat', 'formatmatch', 'autotypeset', 'blockquote', 'pasteplain', '|', 'forecolor', 'backcolor', 'insertorderedlist', 'insertunorderedlist', 'selectall', 'cleardoc', '|',
'rowspacingtop', 'rowspacingbottom', 'lineheight', '|',
'customstyle', 'paragraph', 'fontfamily', 'fontsize', '|',
'directionalityltr', 'directionalityrtl', 'indent', '|',
'justifyleft', 'justifycenter', 'justifyright', 'justifyjustify', '|',
'imagenone', 'imageleft', 'imageright', 'imagecenter', '|',
'horizontal', '|',
'inserttable', 'deletetable', 'insertparagraphbeforetable', 'insertrow', 'deleterow', 'insertcol', 'deletecol', 'mergecells', 'mergeright', 'mergedown', 'splittocells', 'splittorows', 'splittocols'
                ]]
            , 'filterTxtRules': function () {
                function transP(node) {
                    node.tagName = 'p';
                    node.setStyle();
                }
                return {
                    //直接删除及其字节点内容
                    '-': 'script style object iframe embed input select',
                    'p': { $: {} },
                    'br': { $: {} },
                    'div': { '$': {} },
                    'li': { '$': {} },
                    'caption': transP,
                    'th': transP,
                    'tr': transP,
                    'h1': transP, 'h2': transP, 'h3': transP, 'h4': transP, 'h5': transP, 'h6': transP,
                    'td': function (node) {
                        //没有内容的td直接删掉
                        var txt = !!node.innerText();
                        if (txt) {
                            node.parentNode.insertAfter(UE.uNode.createText(' &nbsp; &nbsp;'), node);
                        }
                        node.parentNode.removeChild(node, node.innerText())
                    }
                }
            }()
                , 'catchRemoteImageEnable': false
            });
            // 'preview','print',
            editor.render("myEditor");
            editor.ready(function () {
                var value = $("#hidContent").val();
                while (value.indexOf("%23") != -1) {
                    value = value.replace("%23", "#");
                }
                while (value.indexOf("&lt;") != -1) {
                    value = value.replace("&lt;", "<");
                }
                while (value.indexOf("&gt;") != -1) {
                    value = value.replace("&gt;", ">");
                }
                //需要ready后执行，否则可能报错
                editor.setContent(value);
                editor.sync("form1");
            });
        </script>
        <asp:Button ID="btnSave" runat="server" OnClientClick="return CheckValid()" Text="保存并返回列表" OnClick="btnSave_Click" />

    </form>
</body>
</html>
