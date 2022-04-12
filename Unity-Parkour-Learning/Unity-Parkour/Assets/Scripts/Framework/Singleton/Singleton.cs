/****************************************************
    文件：Singleton.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/8 14:37:10
    功能：Nothing
*****************************************************/

namespace FrameworkDesign
{
    public class Singleton<T> : ISingleton where T : Singleton<T>
    {
        private static T mInstance;

        private static readonly object syslock = new object(); 

        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    lock (syslock)
                    {
                        if (mInstance == null)
                        {
                            mInstance = SingletonCreator.CreateSingleton<T>();
                        }
                    }
                }
                return mInstance;
            }
        }

        

        public virtual void OnSingletonInit()
        {
            
        }

        public virtual void Dispose()
        {

        }
    }
}

