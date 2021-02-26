using System.Collections.Generic;
using UnityEngine;
using DoodleJump.Common;
using System;

namespace DoodleJump.Gameplay
{
    public interface IEntityFactory
    {
        IWorld World { get; set; }
        T CreateEntity<T>(string name) where T : Entity;
        void AddPrefab(string name, Entity prefab);
    }

    public static class EntityFactoryExtensions
    {
        public static T CreateEntity<T>(this IEntityFactory entityFactory) where T : Entity
        {
            return entityFactory.CreateEntity<T>(typeof(T).Name);
        }

        public static T CreateEntity<T>(this IEntityFactory entityFactory, Vector2 position, float zIndex = 0f, float scale = 1f) where T : Entity
        {
            var entity = entityFactory.CreateEntity<T>(typeof(T).Name);
            if(entity)
            {
                entity.Position = position;
                entity.ZIndex = 0f; 
            }

            return entity;
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

        public IWorld World { get; set; }

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
                    var entity = GameObject.Instantiate<T>(prefab as T);
                    World.AddEntity(entity);

                    return entity;
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
