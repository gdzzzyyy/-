using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public interface ISubscriber
{
    System.Action<object[]> Handler
    {
        set;
    }

    void UnSubscribe();

}

public static class EventNames
{
    public const string Gui_test1 = "gui:test1";
}





public static class EventsManager
{
    static Dictionary<string, List<Subscriber>> ms_subscribers = new Dictionary<string, List<Subscriber>>();

    [System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Field)]

    //??????
    public class ObserveAttribute : System.Attribute  
    {

    }

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

    //观察者
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

    public abstract class Observer : Publisher
    {
        System.Object m_obj;
        List<FieldInfo> m_observeFields = new List<FieldInfo>();
        List<PropertyInfo> m_observeProperties = new List<PropertyInfo>();
        List<object> m_fieldValues = new List<object>();
        List<object> m_propertyValues = new List<object>();

        public Observer(System.Object obj)
        {
            m_obj = obj;
            var fields = m_obj.GetType().GetFields();

            foreach(var f in fields)
            {
                object[] attributes = f.GetCustomAttributes(typeof(ObserveAttribute), false);
                if(attributes.Length > 0)
                {
                    m_observeFields.Add(f);
                    m_fieldValues.Add(f.GetValue(m_obj));
                }
            }

            var properties = m_obj.GetType().GetProperties();
            foreach(var p in properties)
            {
                if (!p.CanRead)
                    continue;

                object[] attributes = p.GetCustomAttributes(typeof(ObserveAttribute), false);
                if(attributes.Length > 0)
                {
                    m_observeProperties.Add(p);
                    m_propertyValues.Add(p.GetValue(m_obj, null));
                }
            }
        }

        public void Update()
        {
            for(int i = 0; i < m_observeFields.Count; i++)
            {
                var m = m_observeFields[i];
                object newValue = m.GetValue(m_obj);
                if(!object.Equals(newValue, m_fieldValues[i]))
                {
                    Notify("valueChanged", m_obj, m.Name);
                    m_fieldValues[i] = newValue;
                }
            }

            for(int i = 0; i < m_observeProperties.Count; i++)
            {
                var p = m_observeProperties[i];
                object newValue = p.GetValue(m_obj, null);
                if (!object.Equals(newValue, m_propertyValues[i]))
                {
                    Notify("valueChanged", m_obj, p.Name);
                    m_propertyValues[i] = newValue;
                }
            }
        }
    }



}
