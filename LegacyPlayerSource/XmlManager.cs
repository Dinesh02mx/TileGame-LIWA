using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 脚本无需挂载 直接调用
/// XML数据获取示例 XmlManager.GetInstance().Language 其中Config需要对应m_WindowsXmlConfigPath目录下的
/// XML文件成员变量 如此目录无Xml文件则在此文件下复制本脚本目录下的Config.xml文件即可，自行配置对应成员和config.cs对应
/// 可以使用"Build/创建默认Xml配置文件"菜单栏一键创建xml
/// XML数据更新示例   XmlManager.updateXML("WindowWidth",“1920”); 第一个变量威对应xml成员名称，第二个为对应值
/// XML读取数据示例   XmlManager.ReadXml("WindowWidth"); 第一个变量威对应xml成员名称 返回值为读取值
/// </summary>
public static class XmlManager
{
    private static Config _instance;
    public static string m_WindowsXmlConfigPath = Application.dataPath + "/../res/Config.xml";
    
    public static Config GetInstance()
    {
        if (_instance == null)
        {
            _instance = XMLTool.XmlDeserializeRun<Config>(m_WindowsXmlConfigPath);
            return _instance;
        }
        return _instance;
    }

    /// <summary>
    /// 更新节点name值为value 没有那么重新创建一个节点name
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    public static void updateXML<T>(string name, T value)
    {
        XMLTool.updateXML(name,value.ToString());
    }
    
    /// <summary>
    /// 读取name的值，没有为默认值
    /// </summary>
    /// <param name="name"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ReadXml<T>(string name)
    {
        T value;
        XMLTool.ReadXML<T>(name, out value, default(T));
        return value;
    }
    /// <summary>
    /// 读取name的值，没有为默认值
    /// </summary>
    /// <param name="name"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ReadXml<T>(string name, T defaultValue, bool addValue = true)
    {
        T defaultValuereturn;
        XMLTool.ReadXML<T>(name, out defaultValuereturn, defaultValue, addValue);
        return defaultValuereturn;
    }
}

