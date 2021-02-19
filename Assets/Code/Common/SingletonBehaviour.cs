using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DoodleJump.Common
{
    public abstract class SingletonBehaviour : CommonBehaviour
    {

    }

    public class SingletonBehaviour<T> : SingletonBehaviour where T : SingletonBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if(!instance)
                {
                    SetInstance(new GameObject().AddComponent<T>());
                }

                return instance;
            }
        }

        private void Awake()
        {
            var component = this as T;

            if(instance && instance != component)
            {
                DestroyImmediate(gameObject);
            }else
            {
                SetInstance(component);
                Init();
            }
        }

        private static void SetInstance(T argInstance)
        {
            //first delete all other instances (if there are any)
            var instances = Resources.FindObjectsOfTypeAll<T>();
            foreach(var inst in instances)
            {
                if(inst && inst != argInstance)
                {
                    DestroyImmediate(inst.gameObject);
                }
            }

            instance = argInstance;
            instance.gameObject.name = $"Singleton_{typeof(T).Name}";
            GameObject.DontDestroyOnLoad(instance.gameObject);
        }

        public virtual void Init()
        {
            
        }
    }
}
