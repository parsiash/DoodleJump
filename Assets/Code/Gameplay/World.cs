using System;
using System.Collections.Generic;
using DoodleJump.Common;
using UnityEngine;

namespace DoodleJump.Gameplay
{
    public enum WorldState
    {
        Initial,
        Simulating,
        Final
    }

    public interface IWorld
    {
        WorldState State { get; }
        float LeftEdgeX { get; }
        float RightEdgeX { get; }
        int Score { get; }
        int JumpedPlatformCount { get; }
        CharacterController Character { get; }

        void OnStart();
        void OnUpdate(float dt);
        void Reset();
        void OnPlatformJumpFirstTime(Platform platform);
        void OnLose();

        //entities
        IEnumerable<IEntity> Entities { get; }
        void AddEntity(IEntity entity);
        void RemoveEntity(IEntity entity);

        //systems
        IEntityFactory EntityFactory { get; }
        IChunkSystem ChunkSystem { get; }
        DoodleJump.Common.ILogger Logger { get; }


        //events
        void AddEventListener(IWorldEventListener eventListener);
    }

    public interface IWorldEventListener
    {
    }

    public class World : IWorld
    {
        public WorldState State { get; private set; }

        public float LeftEdgeX => -4f;
        public float RightEdgeX => 4f;

        private CharacterController _character;
        public CharacterController Character => _character;

        public Common.ILogger Logger => Common.Logger.Instance;

        private IEntityFactory _entityFactory;
        public IEntityFactory EntityFactory => _entityFactory;

        private IChunkSystem _chunkSystem;
        public IChunkSystem ChunkSystem => _chunkSystem;

        private List<IEntity> _entities;
        private List<IEntity> _createdEntities;
        public IEnumerable<IEntity> Entities => _entities;

        private List<IWorldEventListener> _eventListeners;

        private int _score;
        public int Score
        {
            get
            {
                if(!_character)
                {
                    return 0;
                }

                var characterPosition = _character.Position;
                _score = Mathf.Max(_score, Mathf.CeilToInt(characterPosition.y));
                return _score;
            }
        }

        private int _jumpedPlatformCount;
        public int JumpedPlatformCount => _jumpedPlatformCount;

        private Action<int> OnLoseCallback;

        public World(IEntityFactory entityFactory, Action<int> onLoseCallback)
        {
            State = WorldState.Initial;

            _entityFactory = entityFactory;
            _entityFactory.World = this;
            
            //init entities
            _entities = new List<IEntity>();
            _createdEntities = new List<IEntity>();

            //create the character
            _character = entityFactory.CreateEntity<CharacterController>();

            _chunkSystem = new ChunkSystem(this);
            _eventListeners = new List<IWorldEventListener>();

            OnLoseCallback = onLoseCallback;
        }

        public void OnUpdate(float dt)
        {
            if(State != WorldState.Simulating)
            {
                return;
            }
            
            _chunkSystem.OnUpdate();

            foreach(var entity in _entities)
            {
                if(entity != null && !entity.IsDestroyed)
                {
                    entity.OnUpdate(Time.deltaTime);
                }
            }

            //add created entities
            foreach(var entity in _createdEntities)
            {
                _entities.Add(entity);
            }
            _createdEntities.Clear();

            //destroy entities
            foreach(var entity in _entities)
            {
                if(entity.IsDestroyed)
                {
                    DestroyEntityGameObject(entity);
                }
            }
            _entities.RemoveAll(e => e.IsDestroyed);
        }

        public void OnStart()
        {
            State = WorldState.Simulating;
        }

        public void Reset()
        {
            _character.Reset();
            _chunkSystem.Clear();
            _eventListeners.Clear();
            ClearAllEntities();
        }

        private void ClearAllEntities()
        {
            foreach (var entity in _entities)
            {
                DestroyEntityGameObject(entity);
            }
            _entities.Clear();

            foreach (var entity in _createdEntities)
            {
                DestroyEntityGameObject(entity);
            }
            _createdEntities.Clear();
        }

        private void DestroyEntityGameObject(IEntity ientity)
        {
            var entity = ientity as Entity;
            if(entity)
            {
                GameObject.Destroy(entity.gameObject);
            }
        }

        public void OnLose()
        {
            State = WorldState.Final;
            OnLoseCallback(Score);
        }

        public void AddEventListener(IWorldEventListener eventListener)
        {
            _eventListeners.Add(eventListener);
        }

        public void OnPlatformJumpFirstTime(Platform platform)
        {
            _jumpedPlatformCount++;
        }

        private void RaiseWorldEvent(Action<IWorldEventListener> eventCallback)
        {
            foreach(var eventListener in _eventListeners)
            {
                if(eventListener != null)
                {
                    eventCallback(eventListener);
                }
            }
        }

        public void AddEntity(IEntity entity)
        {
            _createdEntities.Add(entity);
            entity.Init(this);
        }

        public void RemoveEntity(IEntity entity)
        {
            _entities.Remove(entity);
        }
    }
}
