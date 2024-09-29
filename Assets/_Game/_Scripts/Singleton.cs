using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    [SerializeField] private bool dontDestroyOverLoad;

    private static T instance;
    public static T Instance => instance;
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this as T;

            if (dontDestroyOverLoad)
            {
                DontDestroyOnLoad(gameObject);
            }

            CustomAwake();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    protected virtual void CustomAwake() { }
}
