using System;

namespace DoodleJump.Common
{
    public interface IObjectStorage
    {
        void SaveObject(Type t, string name, object obj);
        object LoadObject(Type t, string name);
    }

    public static class ObjectStorageExtensions
    {
        public static T LoadObject<T>(this IObjectStorage storage, string name = null) where T : class
        {
            if(string.IsNullOrEmpty(name))
            {
                name = typeof(T).Name;
            }

            var obj = storage.LoadObject(typeof(T), name);
            return obj as T;
        }

        public static void SaveObject<T>(this IObjectStorage storage, T obj, string name = null) where T : class
        {
            if(string.IsNullOrEmpty(name))
            {
                name = typeof(T).Name;
            }

            storage.SaveObject(typeof(T), name, obj);
        }
    }
}