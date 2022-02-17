/****************************************************
	文件：DataEditor.cs
	作者：Zhang
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml;
using OfficeOpenXml;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;

public class DataEditor
{
    private static string XMLPath = "Assets/GameData/Data/Xml/";
    private static string BinaryPath = "Assets/GameData/Data/Binary/";
    private static string ScriptPath = "Assets/Script/Data/";

    [MenuItem("Assets/Class转Xml")]
    public static void AssetClassToXml()
    {
        UnityEngine.Object[] objs = Selection.objects;
        for (int i = 0; i < objs.Length; i++)
        {
            EditorUtility.DisplayProgressBar("文件夹下Class转Xml", $"正在扫描{objs[i].name}...", 1.0f / objs.Length + i);
            ClassToXml(objs[i].name);
        }
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();

    }

    [MenuItem("Assets/Xml转Binary")]
    public static void AssetXmlToBinary()
    {
        UnityEngine.Object[] objs = Selection.objects;
        for (int i = 0; i < objs.Length; i++)
        {
            EditorUtility.DisplayProgressBar("文件夹下Xml转二进制", $"正在扫描{objs[i].name}...", 1.0f / objs.Length + i);
            XmlToBinary(objs[i].name);
        }
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();

    }

    [MenuItem("Tools/Xml/全部Xml转Binary")]
    public static void AllXmlToBinary()
    {
        string path = Application.dataPath.Replace("Assets", "") + XMLPath;
        string[] filesPath = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
        for (int i = 0; i < filesPath.Length; i++)
        {
            EditorUtility.DisplayProgressBar("查找Xml", $"正在扫描{filesPath[i]}...", 1.0f / filesPath.Length + i);
            if (filesPath[i].EndsWith(".xml"))
            {
                string tmpPath = filesPath[i].Substring(filesPath[i].LastIndexOf(@"/") + 1);
                tmpPath = tmpPath.Replace(".xml", "");
                XmlToBinary(tmpPath);
            }
        }
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
    }


    [MenuItem("Tools/Xml/Xml转Excel")]
    public static void XmlToExcel()
    {
        string fileName = "MonsterData";
        string regPath = Application.dataPath + $"/../Data/Reg/{fileName}.xml";
        if (!File.Exists(regPath))
        {
            Debug.LogError($"不存在xml配置文件:{fileName}");
            return;
        }
        Dictionary<string, SheetData> sheetDataDic = new Dictionary<string, SheetData>();
        
        
        string className = string.Empty;
        string xmlName = string.Empty;
        string excelName = string.Empty;
        Dictionary<string, SheetClass> allSheetClassDic = ReadRegFromDict(regPath, ref excelName, ref xmlName, ref className);

        object data = GetObjFromXml(className);
        

        List<SheetClass> outSheetList = new List<SheetClass>();
        foreach (SheetClass item in allSheetClassDic.Values)
        {
            if (item.Depth == 1)
            {
                outSheetList.Add(item);
            }
        }

        for (int i = 0; i < outSheetList.Count; i++)
        {
            ReadData(data, outSheetList[i], allSheetClassDic, sheetDataDic);
        }

        string xlsxPath = Application.dataPath.Replace("Assets", "Data/Excel/") + excelName;
        if (FileIsUsed(xlsxPath))
        {
            Debug.LogError($"目标文件可能被占用,无法修改:{xlsxPath}");
            return;
        }
        try
        {
            FileInfo xlsxFile = new FileInfo(xlsxPath);
            if (xlsxFile.Exists)
            {
                xlsxFile.Delete();
                xlsxFile = new FileInfo(xlsxPath);
            }
            using(ExcelPackage package = new ExcelPackage(xlsxFile))
            {
                foreach (string str in sheetDataDic.Keys)
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(str);
                    worksheet.Cells.AutoFitColumns();   // 宽度自适应
                    SheetData sheetData = sheetDataDic[str];
                    for (int i = 0; i < sheetData.AllName.Count; i++)
                    {
                        ExcelRange range = worksheet.Cells[1, i + 1];
                        range.Value = sheetData.AllName[i];
                    }
                    for (int i = 0; i < sheetData.AllData.Count; i++)
                    {
                        RowData rowData = sheetData.AllData[i];
                        for (int j = 0; j < rowData.RowDataDic.Count; j++)
                        {
                            ExcelRange range = worksheet.Cells[i + 2, j + 1];
                            range.Value = rowData.RowDataDic[sheetData.AllName[j]];
                        }
                    }
                }
                package.Save();
            }
        }
        catch(Exception e)
        {
            Debug.LogError(e);
            return;
        }
        Debug.Log($"生成{xlsxPath}成功");
    }

    private static Dictionary<string, SheetClass> ReadRegFromDict(string regPath, ref string excelName, ref string xmlName, ref string className)
    {
        XmlDocument xml = new XmlDocument();
        XmlReader reader = XmlReader.Create(regPath);
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.IgnoreComments = true; // 忽略xml文件中的注释
        xml.Load(reader);

        XmlNode xn = xml.SelectSingleNode("data");
        XmlElement xe = (XmlElement)xn;
        className = xe.GetAttribute("name");
        xmlName = xe.GetAttribute("to");
        excelName = xe.GetAttribute("from");
        
        // 存储所有变量的表
        Dictionary<string, SheetClass> allSheetClassDic = new Dictionary<string, SheetClass>();
        ReadXmlNode(xe, allSheetClassDic, 0);
        reader.Close();
        return allSheetClassDic;
    }

    private static object GetObjFromXml(string className)
    {
        Type type = null;
        foreach (var item in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type tmpType = item.GetType(className);
            if (tmpType != null)
            {
                type = tmpType;
                break;
            }
        }
        if (type != null)
        {

            string xmlPath = XMLPath + className + ".xml";
            return BinarySerializeOption.XmlDeserializeEditor(xmlPath, type);
        }
        return null;
    }

    /// <summary>
    /// 判断目标文件是否被占用
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static bool FileIsUsed(string path)
    {
        bool result = false;
        if (File.Exists(path))
        {
            FileStream fileStream = null;
            try
            {
                fileStream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                result = true;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }
        return result;
    }


    /// <summary>
    /// 递归读取类中的数据
    /// </summary>
    /// <param name="data"></param>
    /// <param name="sheetClass"></param>
    /// <param name="allSheetClassDic"></param>
    /// <param name="sheetDataDic"></param>
    private static void ReadData(object data, SheetClass sheetClass, Dictionary<string, SheetClass> allSheetClassDic, Dictionary<string, SheetData> sheetDataDic)
    {
        List<VarClass> varList = sheetClass.VarList;
        VarClass varClass = sheetClass.ParentVar;
        object dataList = GetMemberValue(data, varClass.Name);

        int listCnt = System.Convert.ToInt32(dataList.GetType().InvokeMember("get_Count", BindingFlags.Default | BindingFlags.InvokeMethod, null, dataList, null));

        SheetData sheetData = new SheetData();
        for (int i = 0; i < varList.Count; i++)
        {
            if (!string.IsNullOrEmpty(varList[i].Col))
            {
                sheetData.AllName.Add(varList[i].Col);
                sheetData.AllType.Add(varList[i].Type);
            }   
        }
        for (int i = 0; i < listCnt; i++)
        {
            object item = dataList.GetType().InvokeMember("get_Item", BindingFlags.Default | BindingFlags.InvokeMethod, null, dataList, new object[] { i });

            RowData rowData = new RowData();
            for (int j = 0; j < varList.Count; j++)
            {
                if (varList[j].Type == "list")
                {
                    SheetClass tmpSheetClass = allSheetClassDic[varList[j].ListSheetName];
                    ReadData(item, tmpSheetClass, allSheetClassDic, sheetDataDic);
                }
                else
                {
                    object value = GetMemberValue(item, varList[j].Name);
                    if (varList[j].Col != null)
                    {
                        rowData.RowDataDic.Add(varList[j].Col, value.ToString());
                    }
                    else
                    {
                        Debug.LogError($"{varList[j].Name}反射值为空");
                    }
                }
            }

            string key = varClass.ListName + "&" + varClass.ListSheetName;
            //Debug.Log(varClass.ListSheetName);
            if (sheetDataDic.ContainsKey(key))
            {
                sheetDataDic[key].AllData.Add(rowData);
            }
            else
            {
                sheetData.AllData.Add(rowData);
                sheetDataDic.Add(key, sheetData);
            }
        }
        
    }

    /// <summary>
    /// 读取xml节点配置
    /// </summary>
    /// <param name="xmlElement">节点</param>
    private static void ReadXmlNode(XmlElement xmlElement, Dictionary<string, SheetClass> allSheetClassDic, int depth)
    {
        depth++;
        foreach (XmlNode node in xmlElement.ChildNodes)
        {
            XmlElement xe = (XmlElement)node;
            if (xe.GetAttribute("type") == "list")
            {
                XmlElement listEle = (XmlElement)node.FirstChild;
                VarClass parentVar = new VarClass()
                {
                    Name = xe.GetAttribute("name"),
                    Type = xe.GetAttribute("type"),
                    Col = xe.GetAttribute("col"),
                    DefultValue = xe.GetAttribute("defaultValue"),
                    Foregin = xe.GetAttribute("foregin"),
                    SplitStr = xe.GetAttribute("split")
                };
                if (parentVar.Type == "list")
                {
                    parentVar.ListName = ((XmlElement)xe.FirstChild).GetAttribute("name");
                    parentVar.ListSheetName = ((XmlElement)xe.FirstChild).GetAttribute("sheetname");
                }
                SheetClass sheetClass = new SheetClass()
                {
                    Name = listEle.GetAttribute("name"),
                    SheetName = listEle.GetAttribute("sheetname"),
                    SplitStr = listEle.GetAttribute("split"),
                    MainKey = listEle.GetAttribute("mainKey"),
                    ParentVar = parentVar,
                    Depth = depth,
                };
                if (!string.IsNullOrEmpty(sheetClass.SheetName))
                {
                    if (!allSheetClassDic.ContainsKey(sheetClass.SheetName))
                    {
                        // 获取该类下所有的变量
                        foreach (XmlNode item in listEle.ChildNodes)
                        {
                            XmlElement itemXe = (XmlElement)item;

                            VarClass varClass = new VarClass()
                            {
                                Name = itemXe.GetAttribute("name"),
                                Type = itemXe.GetAttribute("type"),
                                Col = itemXe.GetAttribute("col"),
                                DefultValue = itemXe.GetAttribute("defaultValue"),
                                Foregin = itemXe.GetAttribute("foregin"),
                                SplitStr = itemXe.GetAttribute("split")
                            };
                            if (varClass.Type == "list")
                            {
                                varClass.ListName = ((XmlElement)item.FirstChild).GetAttribute("name");
                                varClass.ListSheetName = ((XmlElement)item.FirstChild).GetAttribute("sheetname");
                            }
                            sheetClass.VarList.Add(varClass);
                        }
                        allSheetClassDic.Add(sheetClass.SheetName, sheetClass);
                    }
                }
                ReadXmlNode(listEle, allSheetClassDic, depth);
            }
        }
    }

    private static object GetObjFormXml(string className, string xmlPath)
    {
        return null;
    }

    /// <summary>
    /// 设置反射属性的值
    /// </summary>
    /// <param name="info"></param>
    /// <param name="var"></param>
    /// <param name="value"></param>
    /// <param name="type"></param>
    private static void SetPropertyValue(PropertyInfo info, object var, string value, string type)
    {
        object val = (object)value;
        switch (type)
        {
            case "int":
                val = System.Convert.ToInt32(val);
                break;
            case "bool":
                val = System.Convert.ToBoolean(val);
                break;
            case "string":
                val = System.Convert.ToString(val);
                break;
            case "float":
                val = System.Convert.ToSingle(val);
                break;
            case "enum":
                val = TypeDescriptor.GetConverter(info.PropertyType).ConvertFromInvariantString(val.ToString());
                break;
        }
        info.SetValue(var, val);
    }

    /// <summary>
    /// 创建一个反射类的List
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static object CreatePropertyList(Type type)
    {
        Type listType = typeof(List<>);
        // 确认List内部的数据类型
        Type specType = listType.MakeGenericType(new System.Type[] { type });
        // 创建List
        return Activator.CreateInstance(specType, new object[] { });
    }

    /// <summary>
    /// 通过反射创建类的实例
    /// </summary>
    /// <param name="name">类名</param>
    /// <returns></returns>
    private static object CreateClass(string name)
    {
        object obj = null;
        Type type = null;
        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type tmpType = asm.GetType(name);
            if (tmpType != null)
            {
                type = tmpType;
                break;
            }
        }
        if (type != null)
        {
            obj = Activator.CreateInstance(type);
        }

        return obj;
    }

    private static void XmlToBinary(string name)
    {
        if (string.IsNullOrEmpty(name))
            return;
        try
        {
            Type type = null;
            foreach (var item in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type tmpType = item.GetType(name);
                if (tmpType != null)
                {
                    type = tmpType;
                    break;
                }
            }
            if (type != null)
            {

                string xmlPath = XMLPath + name + ".xml";
                string binaryPath = BinaryPath + name + ".binary";
                var obj = BinarySerializeOption.XmlDeserializeEditor(xmlPath, type);
                BinarySerializeOption.BinarySerilize(binaryPath, obj);
                Debug.Log(name + "Xml转二进制成功, 路径:" + binaryPath);

            }
            
        }
        catch
        {
            Debug.LogError(name + "Xml转二进制失败");
        }

    }

    private static void ClassToXml(string name)
    {
        if (string.IsNullOrEmpty(name))
            return;
        try
        {
            Type type = null;
            foreach (var item in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type tmpType = item.GetType(name);
                if (tmpType != null)
                {
                    type = tmpType;
                    break;
                }
            }
            if (type != null)
            {
                var tmpScript = Activator.CreateInstance(type);
                if (tmpScript is ExcelBase)
                {
                    (tmpScript as ExcelBase).Construction();

                    BinarySerializeOption.XmlSerialize(XMLPath + name + ".xml", tmpScript);

                    Debug.Log(name + "类转Xml成功, 路径:" + XMLPath + name + ".xml");
                }

            }
        }
        catch
        {
            Debug.LogError(name + "类转Xml失败");
        }

    }

    /// <summary>
    /// 反射类中变量的具体值
    /// </summary>
    /// <param name="obj">类</param>
    /// <param name="memberName">变量名</param>
    /// <param name="bindingFlags">BindingFlags</param>
    /// <returns></returns>
    private static object GetMemberValue(object obj, string memberName, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static)
    {
        Type type = obj.GetType();
        MemberInfo[] members = type.GetMember(memberName, bindingFlags);
        while(members == null || members.Length == 0)
        {
            type = type.BaseType;
            if (type == null)
                return null;
            members = type.GetMember(memberName, bindingFlags);
        }
        object resultObj = null;
        switch (members[0].MemberType)
        {
            case MemberTypes.Field:
                resultObj = type.GetField(memberName, bindingFlags).GetValue(obj);
                break;
            case MemberTypes.Property:
                resultObj = type.GetProperty(memberName, bindingFlags).GetValue(obj);
                break;
        }
        return resultObj;
    }
}

public class SheetClass
{
    /// <summary>
    /// 所属父级的var变量
    /// </summary>
    public VarClass ParentVar { get; set; }
    /// <summary>
    /// 深度
    /// </summary>
    public int Depth { get; set; }
    /// <summary>
    /// 对应类名
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 类对应的Sheet表名
    /// </summary>
    public string SheetName { get; set; }
    /// <summary>
    /// 主键
    /// </summary>
    public string MainKey { get; set; }
    /// <summary>
    /// 分隔符
    /// </summary>
    public string SplitStr { get; set; }
    /// <summary>
    /// 所包含的所有变量
    /// </summary>
    public List<VarClass> VarList = new List<VarClass>();
}

public class VarClass
{
    /// <summary>
    /// 原类变量名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 变量类型
    /// </summary>
    public string Type { get; set; }
    /// <summary>
    /// 变量所对应的Excel中的列
    /// </summary>
    public string Col { get; set; }
    /// <summary>
    /// 变量默认值
    /// </summary>
    public string DefultValue { get; set; }
    /// <summary>
    /// 变量如果是List类型，外联部分列
    /// </summary>
    public string Foregin { get; set; }
    /// <summary>
    /// 分隔符
    /// </summary>
    public string SplitStr { get; set; }
    /// <summary>
    /// 如果自身是List，存储对应的List类名
    /// </summary>
    public string ListName { get; set; }
    /// <summary>
    /// 如果自身是List，存储对应的sheet名
    /// </summary>
    public string ListSheetName { get; set; }

}

public class SheetData
{
    public List<string> AllName = new List<string>();
    public List<string> AllType = new List<string>();
    public List<RowData> AllData = new List<RowData>();
}

public class RowData
{
    public Dictionary<string, string> RowDataDic = new Dictionary<string, string>();
}
