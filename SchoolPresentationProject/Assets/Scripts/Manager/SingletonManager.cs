using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager : MonoBehaviour
{
    private static SingletonManager instance;

    private readonly Dictionary<string, Component> components = new();

    public static void RegisterSingleton<T>(T singleton) where T : Component
    {
        if(instance == null) instance = new GameObject("<<< SingletonManager >>>").AddComponent<SingletonManager>();
        if (!instance.components.ContainsKey(typeof(T).FullName))
        {
            instance.components.Add(typeof(T).FullName, singleton);
        }
        else
        {
            Destroy(singleton.gameObject);
            Debug.LogWarning("Already exist same type name");
        }
    }

    public static T GetSingleton<T>() where T : Component
    {
        return instance.components[typeof(T).FullName] as T;
    }
}
