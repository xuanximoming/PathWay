var isOK = Silverlight.isInstalled("4.0.50524.0");
if (!isOK) {
    var msgw, msgh, bordercolor;
    msgw = 450; //提示窗口的宽度
    msgh = 150; //提示窗口的高度
    titleheight = 25 //提示窗口标题高度
    bordercolor = "#336699"; //提示窗口的边框颜色
    titlecolor = "#99CCFF"; //提示窗口的标题颜色

    var sWidth, sHeight;
    sWidth = screen.width; //浏览器工作区域内页面宽度
    sHeight = screen.height; //屏幕高度（垂直分辨率）


    //背景层（大小与窗口有效区域相同，即当弹出对话框时，背景显示为放射状透明灰色）
    var bgObj = document.createElement("div"); //创建一个div对象（背景层）
    //定义div属性，即相当于
    //<div id="bgDiv" style="position:absolute; top:0; background-color:#777; filter:progid:DXImagesTransform.Microsoft.Alpha(style=3,opacity=25,finishOpacity=75); opacity:0.6; left:0; width:918px; height:768px; z-index:10000;"></div>
    bgObj.setAttribute('id', 'bgDiv');
    bgObj.style.position = "absolute";
    bgObj.style.top = "0";
    bgObj.style.background = "#777";
    bgObj.style.filter = "progid:DXImageTransform.Microsoft.Alpha(style=2,opacity=25,finishOpacity=75";
    bgObj.style.opacity = "0.6";
    bgObj.style.left = "0";
    bgObj.style.width = sWidth + "px";
    bgObj.style.height = sHeight * 2 + "px";
    bgObj.style.zIndex = "10000";
    document.body.appendChild(bgObj); //在body内添加该div对象


    var msgObj = document.createElement("div")//创建一个div对象（提示框层）
    //定义div属性，即相当于
    //<div id="msgDiv" align="center" style="background-color:white; border:1px solid #336699; position:absolute; left:50%; top:50%; font:12px/1.6em Verdana,Geneva,Arial,Helvetica,sans-serif; margin-left:-225px; margin-top:npx; width:400px; height:100px; text-align:center; line-height:25px; z-index:100001;"></div>
    msgObj.setAttribute("id", "msgDiv");
    msgObj.setAttribute("align", "center");
    msgObj.style.background = "white";
    msgObj.style.border = "1px solid " + bordercolor;
    msgObj.style.position = "absolute";
    msgObj.style.left = "50%";
    msgObj.style.top = "20%";
    msgObj.style.font = "12px/1.6em Verdana, Geneva, Arial, Helvetica, sans-serif";
    msgObj.style.marginLeft = "-225px";
    msgObj.style.marginTop = -75 + document.documentElement.scrollTop + "px";
    msgObj.style.width = msgw + "px";
    msgObj.style.height = msgh + "px";
    msgObj.style.padding = "0px";
    msgObj.style.textAlign = "center";
    msgObj.style.lineHeight = "25px";
    msgObj.style.zIndex = "10001";

    var title = document.createElement("h4"); //创建一个h4对象（提示框标题栏）

    //定义h4的属性，即相当于
    //<h4 id="msgTitle" align="right" style="margin:0; padding:3px; background-color:#336699; filter:progid:DXImageTransform.Microsoft.Alpha(startX=20, startY=20, finishX=100, finishY=100,style=1,opacity=75,finishOpacity=100); opacity:0.75; border:1px solid #336699; height:18px; font:12px Verdana,Geneva,Arial,Helvetica,sans-serif; color:white; cursor:pointer;" onclick="">关闭</h4>
    title.setAttribute("id", "msgTitle");
    title.setAttribute("align", "right");
    title.style.margin = "0";
    title.style.padding = "3px";
    title.style.background = bordercolor;

    title.style.filter = "progid:DXImageTransform.Microsoft.Alpha(startX=20, startY=20, finishX=100, finishY=100,style=1,opacity=75,finishOpacity=100);";
    title.style.opacity = "0.75";
    title.style.border = "1px solid " + bordercolor;
    title.style.height = "18px";

    title.style.font = "12px Verdana, Geneva, Arial, Helvetica, sans-serif";

    title.style.color = "white";
    title.style.cursor = "pointer";
    title.innerHTML = "关闭";


    function removeObj() {//点击标题栏触发的事件
        document.body.removeChild(bgObj); //删除背景层Div
        document.getElementById("msgDiv").removeChild(title); //删除提示框的标题栏
        document.body.removeChild(msgObj); //删除提示框层
    }
    title.onclick = removeObj;


    document.body.appendChild(msgObj); //在body内添加提示框div对象msgObj
    document.getElementById("msgDiv").appendChild(title); //在提示框div中添加标题栏对象title

    var dv = document.createElement("div");
    dv.setAttribute("id", "slInstall");
    dv.setAttribute("style", "font-family:Arial; font-size:14px; ");
    dv.innerHTML = "<br />安装Microsoft Silverlight后，才能正常浏览该页面。<br /><span id=\"msgSpan\">系统正在下载或者安装此程序。或者 <a href=\"\" id=\"downLink\">点击此处</a> 手动安装。</span><br />安装完成后请重新启动浏览器。";
    document.getElementById("msgDiv").appendChild(dv); //在提示框div中添加提示信息对象txt

    //输出exe
    var downHost = "http://172.16.8.29:8099/";
    var osInfo = window.navigator.userAgent;
    var exeName = "";
    if (osInfo.indexOf("Windows NT") >= 0) exeName = "Silverlight5forwindows.exe";
    else if (osInfo.indexOf("PPC Mac OS X") >= 0) exeName = "Silverlight4formac.dmg";
    else if (osInfo.indexOf("Intel Mac OS X") >= 0) exeName = "Silverlight4formac.dmg";
    document.getElementById("downLink").href = downHost + exeName;


    /*判断浏览器*/
    if (osInfo.indexOf("MSIE") == -1) {
        document.getElementById("msgSpan").innerHTML = "您的浏览器不支持自动安装Silverlight，请 <a href=\"\" style=\"text-decoration:underline\" id=\"downLink\">点击此处</a> 手动安装。";

    }
    else {
        //输出exe安装包
        run_exe = "<OBJECT ID=\"RUNIT\" WIDTH=0 HEIGHT=0 TYPE=\"application/x-oleobject\""
        run_exe += "CODEBASE=\"{0}\">"
        run_exe += "</OBJECT>"
        run_exe = run_exe.replace("{0}", downHost + exeName);
        document.open();
        document.clear();
        document.writeln(run_exe);
        document.close();
    }

}
 

