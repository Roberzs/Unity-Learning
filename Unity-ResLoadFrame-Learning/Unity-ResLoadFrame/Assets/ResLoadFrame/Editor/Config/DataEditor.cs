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

    [MenuItem("Tools/全部Xml转Binary")]
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

    [MenuItem("Tools/测试/XML解析测试")]
    public static void TestReadXml()
    {
        XmlReader reader = null;
        try
        {
            string xmlPath = Application.dataPath + "/../Data/Reg/MonsterData.xml";
            XmlDocument xml = new XmlDocument();
            reader = XmlReader.Create(xmlPath);
            xml.Load(reader);
            XmlNode xn = xml.SelectSingleNode("data");
            XmlElement xe = (XmlElement)xn;
            string className = xe.GetAttribute("name");
            string xmlName = xe.GetAttribute("to");
            string excelName = xe.GetAttribute("from");
            reader.Close();
            Debug.Log($"ClassName:{className}, XMLName:{xmlName}, ExcelName:{excelName}");
        }
        catch(Exception e)
        {
            if (reader != null)
            {
                reader.Close();
            }
            Debug.LogError(e);
        }
    }

    [MenuItem("Tools/测试/Excel写入测试")]
    public static void TestWriteExcel()
    {
        string xlsxPath = Application.dataPath + "/../Data/Excel/GMonster.xlsx";
        FileInfo xlsxFile = new FileInfo(xlsxPath);
        if (xlsxFile.Exists)
        {
            xlsxFile.Delete();
            xlsxFile = new FileInfo(xlsxPath);
        }
        using (ExcelPackage package = new ExcelPackage(xlsxFile))
        {
            // 表单页
            ExcelWorksheet workSheet = package.Workbook.Worksheets.Add("怪物配置表");
            // 表单页默认宽高
            workSheet.DefaultColWidth = 10;
            workSheet.DefaultRowHeight = 30;
            
            // 单元格
            ExcelRange range = workSheet.Cells[1, 1];
            range.Value = "测试好多好多\n好多好多好多数据";
            // 自适应行长度
            range.AutoFitColumns();
            // 自动换行
            range.Style.WrapText = true;
           
            package.Save();
        }
    }

    [MenuItem("Tools/测试/已有类反射测试")]
    public static void TestReflection_0()
    {
        TestInfo testInfo = new TestInfo()
        {
            Id = 1001,
            TestStrList = new List<string>()
            {
                "ListData_1",
                "ListData_2"
            },
            TestClassDataList = new List<TestData>()
            {
                new TestData()
                {
                    Id = 201,
                },
                new TestData()
                {
                    Id = 202,
                }
            }
        };
        // 1. 反射一般属性字段
        object id = GetMemberValue(testInfo, "Id");
        Debug.Log($"测试反射int - Id:{id}");
        // 2. 反射List属性字段
        object list = GetMemberValue(testInfo, "TestStrList");
        int listCnt = System.Convert.ToInt32(list.GetType().InvokeMember("get_Count", BindingFlags.Default | BindingFlags.InvokeMethod, null, list, null));
        string listContent = "";
        for (int i = 0; i < listCnt; i++)
        {
            object item = list.GetType().InvokeMember("get_Item", BindingFlags.Default | BindingFlags.InvokeMethod, null, list, new object[] { i });
            listContent = listContent + i + ". " + item + "  ";
        }
        Debug.Log($"测试反射List - TestStrList: 共{listCnt}条数据, 内容: {listContent}");
        // 3. 反射Class List
        list = GetMemberValue(testInfo, "TestClassDataList");
        listCnt = System.Convert.ToInt32(list.GetType().InvokeMember("get_Count", BindingFlags.Default | BindingFlags.InvokeMethod, null, list, null));
        Debug.Log("-------------");
        Debug.Log($"测试反射Class List - TestClassDataList: 共{listCnt}条数据");
        for (int i = 0; i < listCnt; i++)
        {
            object item = list.GetType().InvokeMember("get_Item", BindingFlags.Default | BindingFlags.InvokeMethod, null, list, new object[] { i });

            object tmpId = GetMemberValue(item, "Id");

            Debug.Log($"{i}.{tmpId}");
        }

    }

    [MenuItem("Tools/测试/已有数据反射测试")]
    public static void TestReflection_1()
    {
        object obj = CreateClass("TestInfo");
        if (obj is object)
        {
            PropertyInfo info = obj.GetType().GetProperty("Id");
            SetPropertyValue(info, obj, "401", "int");
            // 反射枚举
            //PropertyInfo enumInfo = obj.GetType().GetProperty("TestEnum");
            //object enumInfoValue = TypeDescriptor.GetConverter(enumInfo.PropertyType).ConvertFromInvariantString("VAL");
            //enumInfo.SetValue(obj, enumInfoValue);

            
            // 创建List
            object list = CreatePropertyList(typeof(string));
            // 调用List的Add方法添加数据
            list.GetType().InvokeMember("Add", BindingFlags.Default | BindingFlags.InvokeMethod, null, list, new object[] { "填入的测试数据1" });
            obj.GetType().GetProperty("TestStrList").SetValue(obj, list);

            // List<Class>
            object list_2 = CreatePropertyList(typeof(TestData));
            // - 创建设置Class属性
            object obj_2 = CreateClass("TestData");
            PropertyInfo info_2 = obj_2.GetType().GetProperty("Id");
            SetPropertyValue(info_2, obj_2, "10086", "int");
            // - 添加到List
            list_2.GetType().InvokeMember("Add", BindingFlags.Default | BindingFlags.InvokeMethod, null, list_2, new object[] { obj_2 });
            // - 添加到obj
            obj.GetType().GetProperty("TestClassDataList").SetValue(obj, list_2);

            TestInfo testInfo = obj as TestInfo;
            Debug.Log($"通过已有数据反射类 TestClassDataList[0].Id: {testInfo.TestClassDataList[0].Id}");
        }
        
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
        XmlDocument xml = new XmlDocument();
        XmlReader reader = XmlReader.Create(regPath);
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.IgnoreComments = true; // 忽略xml文件中的注释
        xml.Load(reader);

        XmlNode xn = xml.SelectSingleNode("data");
        XmlElement xe = (XmlElement)xn;
        string className = xe.GetAttribute("name");
        string xmlName = xe.GetAttribute("to");
        string excelName = xe.GetAttribute("from");
        reader.Close();
        // 存储所有变量的表
        Dictionary<string, SheetClass> allSheetClassDic = new Dictionary<string, SheetClass>();
        Dictionary<string, SheetData> sheetDataDic = new Dictionary<string, SheetData>();
        ReadXmlNode(xe, allSheetClassDic, 0);

        object data = null;
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
            data = BinarySerializeOption.XmlDeserializeEditor(xmlPath, type);
        }

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
    }

    private static void ReadData(object data, SheetClass sheetClass, Dictionary<string, SheetClass> allSheetClassDic, Dictionary<string, SheetData> sheetDataDic)
    {
        List<VarClass> varList = sheetClass.VarList;
        VarClass varClass = sheetClass.ParentVar;
        object dataList = GetMemberValue(data, varClass.Name);

        int listCnt = System.Convert.ToInt32(dataList.GetType().InvokeMember("get_Count", BindingFlags.Default | BindingFlags.InvokeMethod, null, dataList, null));
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

public class TestInfo
{
    public int Id { get; set; }
    public List<string> TestStrList { get; set; }
    public List<TestData> TestClassDataList { get; set; }
}

public class TestData
{
    public int Id { get; set; }
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
