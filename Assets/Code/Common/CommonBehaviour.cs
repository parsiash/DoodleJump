using System;
using System.Collections.Generic;
using UnityEngine;

namespace DoodleJump.Common
{
    /// <summary>
    /// Base class of all in-game mono behaviours.
    /// Consists several reusable functionalities.
    /// </summary>
    public class CommonBehaviour : MonoBehaviour
    {
        private Dictionary<string, Component> _cachedComoponents;
        private Dictionary<string, Component> cachedComoponents
        {
            get
            {
                _cachedComoponents = _cachedComoponents ?? new Dictionary<string, Component>();
                return _cachedComoponents;
            }
        }

        private Dictionary<string, Component> _cachedComoponentsInChildren;
        private Dictionary<string, Component> cachedComoponentsInChildren
        {
            get
            {
                _cachedComoponentsInChildren = _cachedComoponentsInChildren ?? new Dictionary<string, Component>();
                return _cachedComoponentsInChildren;
            }
        }

        private string GetComponentTypeName<T>() where T : Component
        {
            return GetComponentTypeName(typeof(T));
        }

        private string GetComponentTypeName(Type type)
        {
            return type.Name;
        }

        public T GetCachedComponent<T>() where T : Component
        {
            var name = GetComponentTypeName<T>();

            if(cachedComoponents.TryGetValue(name, out var component))
            {
                return component as T;
            }

            component = GetComponent<T>();
            if(component)
            {
                cachedComoponents[name] = component;
            }

            return component as T;
        }        

        public T GetCachedComponentInChildren<T>(bool includeInActive = true) where T : Component
        {
            var name = GetComponentTypeName<T>();

            if(cachedComoponentsInChildren.TryGetValue(name, out var component))
            {
                return component as T;
            }

            component = GetComponentInChildren<T>(includeInActive);
            if(component)
            {
                cachedComoponentsInChildren[name] = component;
            }

            return component as T;
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public virtual void Destroy()
        {
            GameObject.Destroy(gameObject);
        }

        public T AddChildWithComponent<T>(string name = "") where T : Component
        {
            if(string.IsNullOrEmpty(name))
            {
                name = typeof(T).Name;
            }

            var go = new GameObject(name);
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale  = Vector3.one;

            return go.AddComponent<T>();
        }
    }
}