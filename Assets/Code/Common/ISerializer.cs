using System;

namespace DoodleJump.Common
{
    public interface ISerializer
    {
        string Serialize(object obj);
        object Deserialize(Type t, string serialized);
    }

    public static class SerializerExtensions
    {
        public static T Deserialize<T>(this ISerializer serializer, string serialized) where T : class
        {
            var obj = serializer.Deserialize(typeof(T), serialized);
            return obj as T;
        }
    }

}