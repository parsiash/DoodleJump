using System;
using UnityEngine;

namespace DoodleJump.Common
{
    public class LocalObjectStorage : Singleton<LocalObjectStorage>, IObjectStorage
    {
        private ISerializer serializer => JsonNetSerializer.Instance;

        private string GetStorageKey(Type t, string name)
        {
            return $"LocalObject_{t}_{name}";
        }

        public object LoadObject(Type t, string name)
        {
            try
            {
                var storageKey = GetStorageKey(t, name);
                var storedValue = PlayerPrefs.GetString(storageKey, "");

                return serializer.Deserialize(t, storedValue);
            }catch(Exception exp)
            {
                Logger.Instance.LogError($"Exception occured while loading object of type : {t} and name : {name} from local storage : \n {exp.Message} - \n {exp.StackTrace}");
                return null;
            }
        }

        public void SaveObject(Type t, string name, object obj)
        {
            try
            {
                var storageKey = GetStorageKey(t, name);
                var serialized = serializer.Serialize(obj);
                PlayerPrefs.SetString(storageKey, serialized);
                PlayerPrefs.Save();

            }catch(Exception exp)
            {
                Logger.Instance.LogError($"Exception occured while saving object of type : {t} and name : {name} from local storage : \n {exp.Message} - \n {exp.StackTrace}");
            }
        }
    }
}