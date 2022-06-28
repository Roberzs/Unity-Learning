/****************************************************
    文件：CacheSvc.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2020/12/1 23:21:33
	功能：数据库管理类
*****************************************************/

using MySql.Data.MySqlClient;
using PEProtocol;
using System;

public class DBMgr
{
    private static DBMgr instance = null;
    public static DBMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DBMgr();
            }
            return instance;

        }
    }

    private MySqlConnection conn;

    public void Init()
    {
        conn = new MySqlConnection("server=localhost;User Id = root;password= root;Database=darkgod;Charset=utf8");
        conn.Open();

        PECommon.Log("DBMgr Init Done.");

        // 模拟登录
        //QueryPlayerData("huxiaoxiong1", "123456");
    }

    // 查找玩家数据
    public PlayerData QueryPlayerData(string acct, string pass)
    {
        bool isNewAcct = true;      // 用于标志是否是新账号
        PlayerData playerData = null;
        MySqlDataReader reader = null;

        try
        {
            MySqlCommand cmd = new MySqlCommand("select * from account where acct = @acct", conn);
            cmd.Parameters.AddWithValue("acct", acct);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string _pass = reader.GetString("pass");
                if (_pass.Equals(pass))     // 如果查找到数据并且密码正确 获取PlayerData
                {
                    isNewAcct = false;
                    playerData = new PlayerData
                    {
                        id = reader.GetInt32("id"),
                        name = reader.GetString("name"),
                        lv = reader.GetInt32("level"),
                        exp = reader.GetInt32("exp"),
                        power = reader.GetInt32("power"),
                        coin = reader.GetInt32("coin"),
                        diamond = reader.GetInt32("diamond"),
                        crystal = reader.GetInt32("crystal"),
                        // QueryPlayerData()
                        hp = reader.GetInt32("hp"),
                        ad = reader.GetInt32("ad"),
                        ap = reader.GetInt32("ap"),
                        addef = reader.GetInt32("addef"),
                        apdef = reader.GetInt32("apdef"),
                        dodge = reader.GetInt32("dodge"),
                        pierce = reader.GetInt32("pierce"),
                        critical = reader.GetInt32("critical"),

                        guideid = reader.GetInt32("guideid"),
                        time = reader.GetInt64("time"),

                        fuben = reader.GetInt32("fuben"),

                    };
                    // 解析锻造属性
                    string[] strongStrArr = reader.GetString("strong").Split('#');

                    int[] _strongArr = new int[6];
                    for (int i = 0; i < strongStrArr.Length; i++)
                    {
                        if (strongStrArr[i] == "") continue;
                        if (int.TryParse(strongStrArr[i], out int starlv))
                        {
                            _strongArr[i] = starlv;
                        }
                        else
                        {
                            PECommon.Log("Parse Strong Data Error", LogType.Error);
                        }
                    }
                    playerData.strongArr = _strongArr;

                    // 解析每日任务
                    string[] taskStrArr = reader.GetString("task").Split('#');
                    playerData.taskArr = new string[6];
                    for (int i = 0; i < taskStrArr.Length; i++)
                    {
                        if (taskStrArr[i] == "")
                        {
                            continue;
                        }
                        else if (taskStrArr[i].Length >= 5)
                        {
                            playerData.taskArr[i] = taskStrArr[i];
                        }
                        else
                        {
                            throw new Exception("数据异常");
                        }
                    }
                }
                else
                {
                    isNewAcct = false;
                }
            }
        }
        catch (Exception e)
        {
            PECommon.Log("Query PlayData By Acct&Pass Error:" + e, LogType.Error);
        }
        finally
        {
            if (reader != null) reader.Close();

            if (isNewAcct)      // 如果没有查找到这个账号 创建账号并返回PlayerData
            {
                playerData = new PlayerData
                {
                    id = -1,
                    name = "",
                    lv = 1,
                    exp = 0,
                    power = 150,
                    coin = 50000,
                    diamond = 0,
                    crystal = 500,

                    //默认账号数据
                    hp = 2000,
                    ad = 275,
                    ap = 265,
                    addef = 67,
                    apdef = 43,
                    dodge = 7,
                    pierce = 5,
                    critical = 2,

                    guideid = 1001,

                    strongArr = new int[6],
                    time = TimerSvc.Instance.GetNowTime(),

                    taskArr = new string[6],

                    fuben = 10001,

                };
                for (int i = 0; i < playerData.taskArr.Length; i++)
                {
                    playerData.taskArr[i] = (i + 1).ToString() + "|0|0";
                }
                playerData.id = InsertNewAcctData(acct, pass, playerData);
            }
        }
        return playerData;
    }

    // 在数据库中插入新账号的数据并返回ID
    private int InsertNewAcctData(string acct, string pass, PlayerData pd)
    {
        int id = -1;
        try
        {
            MySqlCommand cmd = new MySqlCommand("insert into account set acct=@acct,pass =@pass,name=@name,level=@level,exp=@exp,power=@power,coin=@coin,diamond=@diamond,crystal = @crystal,hp=@hp,ad=@ad,ap=@ap,addef=@addef,apdef=@apdef,dodge=@dodge,pierce=@pierce,critical=@critical,guideid = @guideid, strong = @strong, time = @time, task = @task, fuben = @fuben", conn);
            cmd.Parameters.AddWithValue("acct", acct);
            cmd.Parameters.AddWithValue("pass", pass);
            cmd.Parameters.AddWithValue("name", pd.name);
            cmd.Parameters.AddWithValue("level", pd.lv);
            cmd.Parameters.AddWithValue("exp", pd.exp);
            cmd.Parameters.AddWithValue("power", pd.power);
            cmd.Parameters.AddWithValue("coin", pd.coin);
            cmd.Parameters.AddWithValue("diamond", pd.diamond);
            cmd.Parameters.AddWithValue("crystal", pd.crystal);

            cmd.Parameters.AddWithValue("hp", pd.hp);
            cmd.Parameters.AddWithValue("ad", pd.ad);
            cmd.Parameters.AddWithValue("ap", pd.ap);
            cmd.Parameters.AddWithValue("addef", pd.addef);
            cmd.Parameters.AddWithValue("apdef", pd.apdef);
            cmd.Parameters.AddWithValue("dodge", pd.dodge);
            cmd.Parameters.AddWithValue("pierce", pd.pierce);
            cmd.Parameters.AddWithValue("critical", pd.critical);

            cmd.Parameters.AddWithValue("guideid", pd.guideid);
            cmd.Parameters.AddWithValue("time", pd.time);

            cmd.Parameters.AddWithValue("fuben", pd.fuben);

            string strongInfo = "";
            for (int i = 0; i < pd.strongArr.Length; i++)
            {
                strongInfo += pd.strongArr[i].ToString();
                strongInfo += "#";
            }
            cmd.Parameters.AddWithValue("strong", strongInfo);

            string taskInfo = "";
            for (int i = 0; i < pd.taskArr.Length; i++)
            {
                taskInfo += pd.taskArr[i].ToString();
                taskInfo += "#";
            }
            cmd.Parameters.AddWithValue("task", taskInfo);

            cmd.ExecuteNonQuery();
            id = (int)cmd.LastInsertedId;

        }
        catch (Exception e)
        {
            PECommon.Log("Insert PlayerData Error:" + e, LogType.Error);
        }
        return id;
    }

    // 查找玩家名
    public bool QueryNameData(string name)
    {
        // 查询玩家名 如果存在 返回true
        bool isExist = false;
        MySqlDataReader reader = null;
        try
        {
            MySqlCommand cmd = new MySqlCommand("select * from account where name = @name", conn);
            cmd.Parameters.AddWithValue("name", name);
            reader = cmd.ExecuteReader();
            if (reader.Read()) isExist = true;
        }
        catch (Exception e)
        {
            PECommon.Log("Queue Name State Error:" + e, LogType.Error);
        }
        finally
        {
            if (reader != null) reader.Close();
        }
        return isExist;
    }

    // 更新玩家数据
    public bool UpdatePlayerData(int id, PlayerData playerData)
    {
        try
        {
            MySqlCommand cmd = new MySqlCommand("update account set name=@name,level=@level,exp=@exp,power=@power,coin=@coin,diamond=@diamond,crystal=@crystal,hp=@hp,ad=@ad,ap=@ap,addef=@addef,apdef=@apdef,dodge=@dodge,pierce=@pierce,critical=@critical,guideid = @guideid,strong = @strong,time = @time, task = @task, fuben = @fuben where id =@id", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("name", playerData.name);
            cmd.Parameters.AddWithValue("level", playerData.lv);
            cmd.Parameters.AddWithValue("exp", playerData.exp);
            cmd.Parameters.AddWithValue("power", playerData.power);
            cmd.Parameters.AddWithValue("coin", playerData.coin);
            cmd.Parameters.AddWithValue("diamond", playerData.diamond);
            cmd.Parameters.AddWithValue("crystal", playerData.crystal);

            cmd.Parameters.AddWithValue("hp", playerData.hp);
            cmd.Parameters.AddWithValue("ad", playerData.ad);
            cmd.Parameters.AddWithValue("ap", playerData.ap);
            cmd.Parameters.AddWithValue("addef", playerData.addef);
            cmd.Parameters.AddWithValue("apdef", playerData.apdef);
            cmd.Parameters.AddWithValue("dodge", playerData.dodge);
            cmd.Parameters.AddWithValue("pierce", playerData.pierce);
            cmd.Parameters.AddWithValue("critical", playerData.critical);

            cmd.Parameters.AddWithValue("guideid", playerData.guideid);
            cmd.Parameters.AddWithValue("time", playerData.time);

            cmd.Parameters.AddWithValue("fuben", playerData.fuben);

            string strongInfo = "";
            for (int i = 0; i < playerData.strongArr.Length; i++)
            {
                strongInfo += playerData.strongArr[i].ToString();
                strongInfo += "#";
            }
            cmd.Parameters.AddWithValue("strong", strongInfo);

            string taskInfo = "";
            for (int i = 0; i < playerData.taskArr.Length; i++)
            {
                taskInfo += playerData.taskArr[i].ToString();
                taskInfo += "#";
            }
            cmd.Parameters.AddWithValue("task", taskInfo);

            //TOADD Others
            cmd.ExecuteNonQuery();

        }
        catch (Exception e)
        {
            PECommon.Log("Update PlayerData Error:" + e, LogType.Error);
            return false;
        }
        return true;
    }
}

