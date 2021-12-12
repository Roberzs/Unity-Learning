/****************************************************
    文件：ResSvc.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2020/9/8 22:33:35
	功能：资源加载类
*****************************************************/

using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResSvc : MonoBehaviour 
{
    public static ResSvc Instance = null;

    private Action prgCB = null; 

    public void InitSvc()
    {
        Instance = this;
        InitRDNameCfg(PathDefine.RDNameCfg);
        InitMapCfg(PathDefine.MapCfg);
        InitGuideCfg(PathDefine.GuideCfg);
        InitStrongCfg(PathDefine.StrongCfg);
        InitTaskRewardCfg(PathDefine.TaskRewardCfg);
        PECommon.Log("Init ResSvc");
    }

    public void AsyncLoadScene(string sceneName, Action loaded)
    {
        GameRoot.Instance.loadingWnd.SetWndState(true);

        AsyncOperation sceneAsync =  SceneManager.LoadSceneAsync(sceneName);
        prgCB = () =>
        {
            float val = sceneAsync.progress;
            GameRoot.Instance.loadingWnd.SetProgress(val);
            if (val == 1)
            {
                sceneAsync = null;
                prgCB = null;
                GameRoot.Instance.loadingWnd.SetWndState(false);
                loaded?.Invoke();
            }
        };
    }

    private void Update()
    {
        prgCB?.Invoke();
    }

    private Dictionary<string, AudioClip> adDict = new Dictionary<string, AudioClip>();             // 存放音频路径
    public AudioClip LoadAudio(string path, bool cache = false)
    {
        AudioClip au = null;
        if (!adDict.TryGetValue(path, out au))
        {
            au = Resources.Load<AudioClip>(path);
            if (cache)
            {
                adDict.Add(path, au);
            }
        }
        return au;
    }

    private Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();
    public GameObject LoadPrefab(string path, bool cache = false)
    {
        GameObject prefab = null;
        if (!goDic.TryGetValue(path, out prefab))
        {
            prefab = Resources.Load<GameObject>(path);
            if (cache)
            {
                goDic.Add(path, prefab);
            }
        }
        GameObject go = null;
        if (prefab != null)
        {
            go = Instantiate(prefab);
        }
        else
        {
            PECommon.Log("资源加载失败,路径:" + path, LogType.Error);
        }
        return go;
    }

    private Dictionary<string, Sprite> spDict = new Dictionary<string, Sprite>();
    public Sprite LoadSprite(string path, bool cache = false)
    {
        Sprite sp = null;
        if (!spDict.TryGetValue(path, out sp))
        {
            sp = Resources.Load<Sprite>(path);
            if (cache)
            {
                spDict.Add(path, sp);
            }
        }
        return sp;
    }

    #region 初始化配置文件

    #region 随机名字
    private List<string> surnameList = new List<string>();
    private List<string> maneList = new List<string>();
    private List<string> womanList = new List<string>();
    public void InitRDNameCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml file:" + path + "not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;

            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement ele = nodeList[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null) continue;

                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                foreach (XmlElement e in nodeList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "surname":
                            surnameList.Add(e.InnerText);
                            break;
                        case "man":
                            maneList.Add(e.InnerText);
                            break;
                        case "woman":
                            womanList.Add(e.InnerText);
                            break;
                    }
                }
            }
        }
    }

    public string GetRdNameData(bool isMan = true)
    {
        System.Random cd = new System.Random();
        string rdName = surnameList[PETools.RdInt(0, surnameList.Count - 1, cd)];
        if (isMan)
            rdName += maneList[PETools.RdInt(0, maneList.Count - 1, cd)];
        else
            rdName += womanList[PETools.RdInt(0, womanList.Count - 1, cd)];
        return rdName;
    }
    #endregion

    #region 地图

    private Dictionary<int, MapCfg> mapCfgDataDic = new Dictionary<int, MapCfg>();

    public void InitMapCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml file:" + path + "not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

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
                        case "mapName":
                            mc.mapName = e.InnerText;
                            break;
                        case "sceneName":
                            mc.sceneName = e.InnerText;
                            break;
                        case "power":
                            mc.power = int.Parse(e.InnerText);
                            break;
                        case "mainCamPos":
                            {
                                string[] valArr = e.InnerText.Split(',');
                                mc.mainCamPos = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                            }
                            break;
                        case "mainCamRote":
                            {
                                string[] valArr = e.InnerText.Split(',');
                                mc.mainCamRote = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                            }
                            break;
                        case "playerBornPos":
                            {
                                string[] valArr = e.InnerText.Split(',');
                                mc.playerBornPos = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                            }
                            break;
                        case "playerBornRote":
                            {
                                string[] valArr = e.InnerText.Split(',');
                                mc.playerBornRote = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                            }
                            break;
                    }
                }

                mapCfgDataDic.Add(ID, mc);
            }
        }
    }

    public MapCfg GetMapCfgData(int id)
    {
        MapCfg data;
        if(mapCfgDataDic.TryGetValue(id, out data))
        {
            return data;
        }
        return null;
    }

    #endregion

    #region 任务引导配置文件

    private Dictionary<int, AutoGuideCfg> guideTaskDic = new Dictionary<int, AutoGuideCfg>();

    private void InitGuideCfg (string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml file:" + path + "not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;

            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement ele = nodeList[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null) continue;
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                AutoGuideCfg ac = new AutoGuideCfg
                {
                    ID = ID
                };
                foreach (XmlElement e in nodeList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "npcID":
                            ac.npcID = int.Parse(e.InnerText);
                            break;
                        case "dilogArr":
                            ac.dilogArr = e.InnerText;
                            break;
                        case "actID":
                            ac.actID = int.Parse(e.InnerText);
                            break;
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
    }
    public AutoGuideCfg GetAutoGuideData(int id)
    {
        AutoGuideCfg agc = null;
        if (guideTaskDic.TryGetValue(id, out agc))
        {
            return agc;
        }
        return null;
    }

    #endregion

    #region 锻造

    private Dictionary<int, Dictionary<int, StrongCfg>> strongDic = new Dictionary<int, Dictionary<int, StrongCfg>>();

    private void InitStrongCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml file:" + path + "not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

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
                else {
                    dic = new Dictionary<int, StrongCfg>();
                    dic.Add(sc.starlv, sc);
                    strongDic.Add(sc.pos, dic);
                }
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

    public int GetPropAddValPreLv(int pos, int starlv, int type)
    {
        Dictionary<int, StrongCfg> posDic = null;
        int val = 0;
        if (strongDic.TryGetValue(pos, out posDic))
        {
            for (int i = 0; i <= starlv; i++)
            {
                StrongCfg sc;
                if (posDic.TryGetValue(i, out sc))
                {
                    switch (type)
                    {
                        case 1:
                            val += sc.addhp;
                            break;
                        case 2:
                            val += sc.addhurt;
                            break;
                        case 3:
                            val += sc.adddef;
                            break;
                    }

                }
            }
        }
        return val;
    }

    #endregion

    #region 任务配置文件

    private Dictionary<int, TaskRewardCfg> TaskRewardDic = new Dictionary<int, TaskRewardCfg>();

    private void InitTaskRewardCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml file:" + path + "not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

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
                        case "taskName":
                            trc.taskName = e.InnerText;
                            break;
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
    }
    public TaskRewardCfg GeTaskRewardData(int id)
    {
        TaskRewardCfg trc = null;
        if (TaskRewardDic.TryGetValue(id, out trc))
        {
            return trc;
        }
        return null;
    }

    #endregion

    #endregion
}