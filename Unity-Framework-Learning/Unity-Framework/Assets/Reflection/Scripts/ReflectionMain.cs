/****************************************************
    文件：ReflectionDemo.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/29 10:34:15
    功能：Nothing
*****************************************************/

using System;
using System.Reflection;
using UnityEngine;

namespace ReflectionDemo
{
    // 客户类
    public class Customer
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public Customer() { }

        public bool Validate(Customer customerObj)
        {
            //Code to validate the customer object
            return true;
        }
    }

    public class ReflectionMain : MonoBehaviour
    {
        

        private void Start()
        {
            Type type = typeof(Customer);

            // 反射类名与命名空间
            Debug.Log("Class: " + type.Name);
            Debug.Log("Namespace: " + type.Namespace);

            // 反射所有属性名
            PropertyInfo[] propertyInfos = type.GetProperties();
            Debug.Log("The list of properties of the Customer class are:------------------------");
            foreach (var item in propertyInfos)
            {
                Debug.Log(item.Name);
            }

            // 反射构造函数
            ConstructorInfo[] constructorInfos = type.GetConstructors();
            Debug.Log("The Customer class contains the following Constructors:------------------------");
            foreach (var item in constructorInfos)
            {
                Debug.Log(item);
            }

            // 反射所有Public方法
            MethodInfo[] methodInfos = type.GetMethods();
            Debug.Log("The methods of the Customer class are:------------------------");
            foreach (var item in methodInfos)
            {
                Debug.Log(item.Name);
            }
        }
    }
}

