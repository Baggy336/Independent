using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TankSingleton<T> : NetworkBehaviour
    where T : Component
{
    private static T _Instance;

    // Make sure there is only one instance of tank singleton in the scene
    public static T Instance
    {
        get
        {
            if (_Instance == null)
            {
                var objects = FindObjectsOfType(typeof(T)) as T[];
                if (objects.Length > 0) _Instance = objects[0];
                if (objects.Length > 1)
                {
                    Debug.LogError("There is more than one " + typeof(T).Name + "in this game.");
                }
                if (_Instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = string.Format("_{0}", typeof(T).Name);
                    _Instance = obj.AddComponent<T>();
                }
            }
            return _Instance;
        }
    }
}
