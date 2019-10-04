<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EHRDefault.aspx.cs"
    Inherits="DrectSoftEHRApplication.Web.DrecSoftEHRDefault" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>结构化临床路径</title>
    <script type="text/javascript" src="./js/Silverlight.js"></script>
    <script type="text/javascript" src="./js/autoInstall.js"></script>
    <link rel="stylesheet" href="./css/EHRDefault.css"/>
</head>
<body>
    <form id="form1" runat="server" style="height: 100%; overflow: hidden;" >
    <div id="silverlightControlHost">
        <object data="data:application/x-silverlight-2," type="application/x-silverlight-2"
            width="100%" height="100%">
            <param name="source" value="ClientBin/Pathway.xap" />
            <param name="onError" value="onSilverlightError" />
            <param name="background" value="white" />
            <param name="minRuntimeVersion" value="5.0.61118.0" />
            <param name="autoUpgrade" value="true" />
            <asp:Literal ID="SLInitParams" runat="server" />
        </object>
    </div>   
    </form>
    <script type="text/javascript" src="./js/EHRDefault.js"></script>
</body>
</html>


