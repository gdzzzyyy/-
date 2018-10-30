using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

//算是观察这种的监听接口
public interface ISubscriber
{
    System.Action<object[]> Handler
    {
        set;
    }

    void UnSubscribe(); 

}

//客户端用到的事件名最好都记录下来
public static class EventNames
{
    public const string Gui_test1 = "gui:test1";
}




public static class EventsManager
{
    //所有正在监听的事件列表
    static Dictionary<string, List<Subscriber>> ms_subscribers = new Dictionary<string, List<Subscriber>>();

    //[System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Field)]

    ////??????
    //public class ObserveAttribute : System.Attribute  
    //{

    //}

    //抽象监听主体
    private class Subscriber : ISubscriber
    {
        string m_subscribeKey;
        System.Action<object[]> m_handler;

        public Subscriber(string key)
        {
            m_subscribeKey = key;
        }

        ~Subscriber()
        {
            UnSubscribe();
        }

        public void UnSubscribe()
        {

        }

        public System.Action<object[]> Handler
        {
            set { m_handler = value; }
        }

        public void Notify(params object[] args)
        {
            if (m_handler != null)
                m_handler(args);
        }

    }

    //注册监听对象
    public static ISubscriber Subscribe(string name)
    {
        List<Subscriber> sublist = null;
        if(!ms_subscribers.TryGetValue(name, out sublist))
        {
            sublist = new List<Subscriber>();
            ms_subscribers.Add(name, sublist);
        }

        Subscriber sub = new Subscriber(name);
        sublist.Add(sub);
        return sub;
    }

    //监听对象分发实现方法
    public static void Notify(string name, params object[] args)
    {
        List<Subscriber> sublist = null;
        if(!ms_subscribers.TryGetValue(name, out sublist))
        {
            return;
        }

        Subscriber[] subs = sublist.ToArray();
        foreach(var sub in subs)
        {
            if (!sublist.Contains(sub))
                continue;
            sub.Notify(args);
        }
    }

    //分发主体   
    public abstract class Publisher
    {
        public abstract string Name
        {
            get;
        }

        protected void Notify(string name, params object[] args)
        {
            EventsManager.Notify(Name + ":" + name, args);
        }
    }
}
