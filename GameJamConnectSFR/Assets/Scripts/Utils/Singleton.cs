using System;
using UnityEngine;


public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;

    private static object staticLock = new object();

    public static T Instance
    {
        get
        {
            lock (staticLock)
            {
                if (null == Singleton<T>.instance)
                {
                    // When component is instanciated, the Awake method is called and the instance field is set.
                    new GameObject(typeof(T).Name, typeof(T));
                }

                return Singleton<T>.instance;
            }
        }
    }

    public static bool TryGetInstance(out T instance)
    {
        instance = Singleton<T>.instance;
        return null != instance;
    }

    public virtual void Awake()
    {
        if (null != Singleton<T>.instance && this != Singleton<T>.instance)
        {
            throw new InvalidOperationException(string.Format("Only one instance of {0} is allowed.\nAn other one is already instanciated: {1}.", typeof(Singleton<T>), Singleton<T>.instance));
        }

        Singleton<T>.instance = (T)this;
    }
}

