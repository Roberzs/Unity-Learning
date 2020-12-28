using System;

/**
 *
 *      单例模式。
 *      
 *      优点：提供了对唯一实例的受控访问，节约资源的多重占用，
 *      缺点：没有接口，不能继承，与单一职责原则冲突，一个类应该只关心内部逻辑，而不关心外面怎么样来实例化。
 *
 */

public class Singleton
{
    // 创建私有变量 内部进行实例化
    private static Singleton _instance = new Singleton();

    // 定义公有方法提供该类的全局唯一访问点
    public static Singleton Instance
    {
        get { return _instance; }
    }

    // 构造方法私有化 不让外部调用构造方法实例化
    private Singleton() { }
}
