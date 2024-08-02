using System;
using UnityEngine;

/// <summary>
/// Store for subsidiary classes, types 
/// </summary>
public class Source
{

}

/// <summary>
/// class realizing a pattern Singlton for MonoBehaviour classes
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class MonoBehaviourSinglton<T> : MonoBehaviour where T : MonoBehaviourSinglton<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    GameObject gameObject = new GameObject(typeof(T).ToString());
                    instance = gameObject.AddComponent<T>();
                }
            }
            return instance;
        }

        protected set
        {
            if (value == null)
            {
                instance = null;
            }
            else
            {
                if (instance == null)
                {
                    instance = value;
                }
                else if (value != instance)
                {
                    Destroy(value);
                }
            }
        }
    }

    protected virtual void Awake()
    {
        Instance = this as T;
    }
}

[Serializable]
public class SerializableKeyValuePair<K, V>
{
    public K Key;
    public V Value;
}

public struct EnumInterval
{
    public int First, Last;

    public EnumInterval(int x, int y)
    {
        First = x;
        Last = y;
    }
}