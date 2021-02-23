using System.Collections.Generic;
using UnityEngine;
using DoodleJump.Common;

namespace DoodleJump.Gameplay
{
    public interface IEntityFactory
    {
        T CreateEntity<T>(string name) where T : Entity;
        void AddPrefab(string name, Entity prefab);
    }

    public static class EntityFactoryExtensions
    {
        public static T CreateEntity<T>(this IEntityFactory entityFactory) where T : Entity
        {
            return entityFactory.CreateEntity<T>(typeof(T).Name);
        }

        public static void AddPrefab<T>(this IEntityFactory entityFactory, T prefab) where T : Entity
        {
            entityFactory.AddPrefab(typeof(T).Name, prefab);
        }
    }
    public class EntityFactory : IEntityFactory
    {
        private IDictionary<string, Entity> _entityPrefabs;
        private Common.ILogger _logger;

        public EntityFactory(Common.ILogger logger)
        {
            _logger = logger;
            _entityPrefabs = new Dictionary<string, Entity>();
        }

        public void AddPrefab(string name, Entity prefab)
        {
            _entityPrefabs[name] = prefab;
        }

        public T CreateEntity<T>(string name) where T : Entity
        {
            if(_entityPrefabs.TryGetValue(name, out var prefab))
            {
                if(prefab is T)
                {
                    return GameObject.Instantiate<T>(prefab as T);
                }else
                {
                    _logger.LogError($"Prefab : {name} is not of type : {typeof(T)}");
                    return null;
                }
            }else
            {
                _logger.LogError($"No prefab found with name : {name}");
                return null;
            }
        }
    }
}
