using System;
using Newtonsoft.Json;

namespace DoodleJump.Common
{
    public class JsonNetSerializer : Singleton<JsonNetSerializer>, ISerializer
    {
        public object Deserialize(Type type, string serialized)
        {
            return JsonConvert.DeserializeObject(serialized, type);
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}