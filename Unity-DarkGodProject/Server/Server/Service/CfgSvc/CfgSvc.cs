/****************************************************
    文件：CfgSvc.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/4/11 14:29:42
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using System.Xml;

public class CfgSvc
{
    private static CfgSvc instance = null;
    public static CfgSvc Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CfgSvc();
            }
            return instance;
        }
    }

    private CacheSvc cacheSvc = null;

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;

        InitGuideCfg();
        InitStrongCfg();
        InitTaskRewardCfg();
        PECommon.Log("CfgSvc Init Done.");
    }

    #region 任务引导配置文件

    private Dictionary<int, GuideCfg> guideTaskDic = new Dictionary<int, GuideCfg>();

    private void InitGuideCfg()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"E:\Unity\Unity-Learning\Unity-DarkGodProject\DarkGodClient\Assets\Resources\ResCfgs\guide.xml");

        XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;

        for (int i = 0; i < nodeList.Count; i++)
        {
            XmlElement ele = nodeList[i] as XmlElement;

            if (ele.GetAttributeNode("ID") == null) continue;
            int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
            GuideCfg ac = new GuideCfg
            {
                ID = ID
            };
            foreach (XmlElement e in nodeList[i].ChildNodes)
            {
                switch (e.Name)
                {
                    case "coin":
                        ac.coin = int.Parse(e.InnerText);
                        break;
                    case "exp":
                        ac.exp = int.Parse(e.InnerText);
                        break;

                }
            }

            guideTaskDic.Add(ID, ac);
        }
    }
    public GuideCfg GetAutoGuideData(int id)
    {
        GuideCfg agc = null;
        if (guideTaskDic.TryGetValue(id, out agc))
        {
            return agc;
        }
        return null;
    }

    #endregion

    #region 任务配置文件

    private Dictionary<int, TaskRewardCfg> TaskRewardDic = new Dictionary<int, TaskRewardCfg>();

    private void InitTaskRewardCfg()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"E:\Unity\Unity-Learning\Unity-DarkGodProject\DarkGodClient\Assets\Resources\ResCfgs\taskreward.xml");

        XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;

        for (int i = 0; i < nodeList.Count; i++)
        {
            XmlElement ele = nodeList[i] as XmlElement;

            if (ele.GetAttributeNode("ID") == null) continue;
            int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
            TaskRewardCfg trc = new TaskRewardCfg
            {
                ID = ID
            };
            foreach (XmlElement e in nodeList[i].ChildNodes)
            {
                switch (e.Name)
                {
                    case "count":
                        trc.count = int.Parse(e.InnerText);
                        break;
                    case "coin":
                        trc.coin = int.Parse(e.InnerText);
                        break;
                    case "exp":
                        trc.exp = int.Parse(e.InnerText);
                        break;

                }
            }

            TaskRewardDic.Add(ID, trc);
        }
    }
    public TaskRewardCfg GetTaskRewardData(int id)
    {
        TaskRewardCfg trc = null;
        if (TaskRewardDic.TryGetValue(id, out trc))
        {
            return trc;
        }
        return null;
    }

    #endregion

    #region 强化配置文件

    private Dictionary<int, Dictionary<int, StrongCfg>> strongDic = new Dictionary<int, Dictionary<int, StrongCfg>>();

    private void InitStrongCfg()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"E:\Unity\Unity-Learning\Unity-DarkGodProject\DarkGodClient\Assets\Resources\ResCfgs\strong.xml");

        XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;

        for (int i = 0; i < nodeList.Count; i++)
        {
            XmlElement ele = nodeList[i] as XmlElement;

            if (ele.GetAttributeNode("ID") == null) continue;
            int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
            StrongCfg sc = new StrongCfg
            {
                ID = ID
            };
            foreach (XmlElement e in nodeList[i].ChildNodes)
            {
                switch (e.Name)
                {
                    case "pos":
                        sc.pos = int.Parse(e.InnerText);
                        break;
                    case "starlv":
                        sc.starlv = int.Parse(e.InnerText);
                        break;
                    case "addhp":
                        sc.addhp = int.Parse(e.InnerText);
                        break;
                    case "addhurt":
                        sc.addhurt = int.Parse(e.InnerText);
                        break;
                    case "adddef":
                        sc.adddef = int.Parse(e.InnerText);
                        break;
                    case "minlv":
                        sc.minlv = int.Parse(e.InnerText);
                        break;
                    case "coin":
                        sc.coin = int.Parse(e.InnerText);
                        break;
                    case "crystal":
                        sc.crystal = int.Parse(e.InnerText);
                        break;
                }
            }

            Dictionary<int, StrongCfg> dic = null;
            if (strongDic.TryGetValue(sc.pos, out dic))
            {
                //Debug.Log("pos：" + sc.pos + "  starlv： " + sc.starlv);
                dic.Add(sc.starlv, sc);
            }
            else
            {
                dic = new Dictionary<int, StrongCfg>();
                dic.Add(sc.starlv, sc);
                strongDic.Add(sc.pos, dic);
            }
        }

    }

    public StrongCfg GetStrongData(int pos, int starlv)
    {
        StrongCfg sc = null;
        Dictionary<int, StrongCfg> dic = null;
        if (strongDic.TryGetValue(pos, out dic))
        {
            if (dic.TryGetValue(starlv, out sc))
            {
                return sc;
            }
        }
        return null;
    }

    #endregion

    #region 地图配置文件
    private Dictionary<int, MapCfg> mapDic = new Dictionary<int, MapCfg>();

    public void InitMapCfg(string path)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"E:\Unity\Unity-Learning\Unity-DarkGodProject\DarkGodClient\Assets\Resources\ResCfgs\map.xml");

        XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;

        for (int i = 0; i < nodeList.Count; i++)
        {
            XmlElement ele = nodeList[i] as XmlElement;

            if (ele.GetAttributeNode("ID") == null) continue;
            int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
            MapCfg mc = new MapCfg
            {
                ID = ID
            };
            foreach (XmlElement e in nodeList[i].ChildNodes)
            {
                switch (e.Name)
                {
                    case "power":
                        mc.power = int.Parse(e.InnerText);
                        break;
                }
            }

            mapDic.Add(ID, mc);
        }

    }

    public MapCfg GetMapData(int id)
    {
        MapCfg mc = null;
        if (mapDic.TryGetValue(id, out mc))
        {
            return mc;
        }
        return null;
    }
    #endregion
}

public class TaskRewardCfg : BaseData<TaskRewardCfg>
{
    public string taskName;
    public int count;
    public int exp;
    public int coin;
}

public class TaskRewardData : BaseData<TaskRewardData>
{
    public int prgs;
    public bool taked;
}

public class StrongCfg : BaseData<StrongCfg>
{
    public int pos;
    public int starlv;
    public int addhp;
    public int addhurt;
    public int adddef;
    public int minlv;
    public int coin;
    public int crystal;
}

public class GuideCfg : BaseData<GuideCfg>
{
    public int coin;
    public int exp;
}

public class BaseData<T>
{
    public int ID;
}

public class MapCfg : BaseData<MapCfg>
{
    public int power;
}