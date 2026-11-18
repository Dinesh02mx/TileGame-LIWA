using System;
using System.Collections.Generic;
using System.ComponentModel;

[Serializable]
public class Config
{
    [Description("语言类型  0：未选择   1:中文   2:英文   3:繁体中文   4:俄语")]
    public int Language = 0;

    [Description("窗口显示位置  屏幕左上角   宽度、高度")]
    public int WindowLeft = 0;
    public int WindowTop = 0;
    public int WindowWidth = 0;
    public int WindowHeight = 0;

    [Description("是否显示鼠标光标")]
    public int ShowMouse = 0;

    [Description("窗口显示层级   -1顶层  0 基于顶层之下   1底层  -2:不置顶 ")]
    public int WindowLayer = -1;

    [Description("是否启动软件隐藏任务栏   退出是显示任务栏")]
    public int HideTaskbar = 0;


    [Description("是否测试链接服务器   打包时删掉此参数，设置为非0值为不链接")]
    public int verification = 0;

    [Description("是否显示水印   打包时删掉此参数，设置为非0值为不显示   定制产品自己定义一个值")]
    public int watermark = 0;

    [Description("是否显版权图片   打包时删掉此参数，默认为0不显示    设置为非0值为显示")]
    public int ShowCopyright = 0;

    [Description("打包时删掉此参数")]
    public int regtype = 0;
    [Description("是否关闭检测1 打包时删掉此参数，默认为0不显示 设置为非0值为显示")]
    public int FlowFristCheck = 0;
    [Description("是否关闭检测2 打包时删掉此参数，默认为0不显示 设置为非0值为显示")]
    public int FlowSecondCheck = 0;
    [Description("是否关闭检测3 打包时删掉此参数，默认为0不显示 设置为非0值为显示")]
    public int FlowThreeCheck = 0;
    [Description("是否关闭检测4 打包时删掉此参数，默认为0不显示 设置为非0值为显示")]
    public int FlowFourCheck = 0;

    //public List<int> xml = new List<int> {1, 1}; list使用需要默认初始换一个参数
}