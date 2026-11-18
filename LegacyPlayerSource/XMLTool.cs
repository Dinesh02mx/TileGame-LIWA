using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class XMLTool 
{
    private const string  _newlineString = "<!--"; // XML中的字符串，用于表示换行
    private const string  _newlineChange = "\r\n  <!--"; 
    
    /// <summary>
    /// 运行时使更新xml
    /// </summary>
    public static void updateXML(string name,string value)
    {
        if (File.Exists(XmlManager.m_WindowsXmlConfigPath))
        {
            try
            {
                bool haseValue = false;
                XmlDocument xml = new XmlDocument();
                xml.Load(XmlManager.m_WindowsXmlConfigPath);
                XmlNodeList xmlNodeList = xml.SelectSingleNode("Config").ChildNodes;
                foreach (XmlNode MyNode in xmlNodeList)
                {
                    if (MyNode.Name.Equals(name))
                    {
                        MyNode.InnerText = value;
                        haseValue = true;
                        break;
                    }
                }

                if (haseValue)
                {
                    xml.Save(XmlManager.m_WindowsXmlConfigPath);
                }
                else
                {
                    XmlElement xeadd=xml.CreateElement(name);
                    xeadd.InnerText = value;
                    xml.SelectSingleNode("Config").AppendChild(xeadd); //添加到<User>节点中
                    xml.Save(XmlManager.m_WindowsXmlConfigPath);
                }
                AddNewLine(XmlManager.m_WindowsXmlConfigPath);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    /// <summary>
    /// 读取xml值
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    public static void ReadXML<T>(string name, out T value, T defaultvalue, bool addValue = true)
    {
        value = defaultvalue;
        if (File.Exists(XmlManager.m_WindowsXmlConfigPath))
        {
            try
            {
                bool haseValue = false;
                XmlDocument xml = new XmlDocument();
                xml.Load(XmlManager.m_WindowsXmlConfigPath);
                XmlNodeList xmlNodeList = xml.SelectSingleNode("Config").ChildNodes;
                foreach (XmlNode MyNode in xmlNodeList)
                {
                    if (MyNode.Name.Equals(name))
                    {
    
                        value = (T)Convert.ChangeType(MyNode.InnerText, typeof(T));
                        haseValue = true;
                        break;
                    }
                }
                if (!haseValue)
                {
                    if (addValue)
                    {
                        XmlElement xeadd = xml.CreateElement(name);
                        xeadd.InnerText = value.ToString();
                        xml.SelectSingleNode("Config").AppendChild(xeadd); //添加到<User>节点中
                        xml.Save(XmlManager.m_WindowsXmlConfigPath);
                        AddNewLine(XmlManager.m_WindowsXmlConfigPath);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
    
    /// <summary>
    /// 类序列化成xml
    /// </summary>
    /// <param name="path"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool Xmlserialize(string path, System.Object obj)
    {
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = "   ";
                settings.NewLineChars = "\r\n";
                settings.Encoding = Encoding.UTF8;
                settings.OmitXmlDeclaration = true;
                
                using (XmlWriter xw = XmlWriter.Create(fs,settings))
                {
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add(String.Empty, String.Empty);
                    XmlSerializer xs = new XmlSerializer(obj.GetType());
                    xs.Serialize(xw, obj,ns);
                    xw.Close();
                }
                fs.Close();
            }
            AddDescription();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("此类无法转换成xml " + obj.GetType() + "," + e);
        }
        return false;
    }

    public static void AddDescription()
    { 
        FieldInfo[] propertys = typeof(Config).GetFields();
        object[] objs;
        XmlDocument xml = new XmlDocument();
        xml.Load(XmlManager.m_WindowsXmlConfigPath);
        XmlNode xmlNodeList = xml.SelectSingleNode("Config");
        foreach (var p in propertys)
        {
            objs = p.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (objs.Length > 0)
            {
                XmlComment comment= xml.CreateComment(((DescriptionAttribute)objs[0]).Description);
                xmlNodeList.InsertBefore(comment,xmlNodeList.SelectSingleNode(p.Name));
            }
        }
        xml.Save(XmlManager.m_WindowsXmlConfigPath);
        AddNewLine(XmlManager.m_WindowsXmlConfigPath);
        Debug.LogError("创建XMl成功");
    }
    
    public static void AddNewLine(string path)
    {
        // 读取文件内容
        string content = File.ReadAllText(path);
        // 替换字符串
        string modifiedContent = content.Replace(_newlineString, _newlineChange);
        // 写入修改后的内容
        File.WriteAllText(path, modifiedContent);
    }
    
    /// <summary>
    /// 运行时使读取xml
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static T XmlDeserializeRun<T>(string path) where T : class
    {
        T t = default(T);
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            t = (T) xs.Deserialize(fs);
        }
        return t;
    }
}
