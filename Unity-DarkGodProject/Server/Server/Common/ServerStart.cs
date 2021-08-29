using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class ServerStart
    {
        static void Main(string[] args)
        {
            /**
             *  ServerRoot 初始化所有层级
             *  网络服务层初始化打开网络服务 监听网络会话 在收到客户端会话时 将会话信息添加进队列
             *  网络服务层当会话队列有消息时 根据消息类别 将消息传递到相应业务系统进行处理
             */
            ServerRoot.Instance.Init();
            while(true) {
                ServerRoot.Instance.Update();
                Thread.Sleep(20);
            };
        }
    }
}
