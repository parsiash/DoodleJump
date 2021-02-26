using System;
using DoodleJump.Common;
using UnityEngine;

namespace DoodleJump.Gameplay
{
    public interface IWorld
    {
        float LeftEdgeX { get; }
        float RightEdgeX { get; }
        CharacterController Character { get; }

        void OnStart();
        void OnUpdate();
        void Reset();

        IEntityFactory EntityFactory { get; }
        DoodleJump.Common.ILogger Logger { get; }

        IChunkSystem ChunkSystem { get; }

        void OnLose();
        int Score { get; }
    }

    public class World : IWorld
    {
        public float LeftEdgeX => -4f;
        public float RightEdgeX => 4f;

        private CharacterController _character;
        public CharacterController Character => _character;

        public Common.ILogger Logger => Common.Logger.Instance;

        private IEntityFactory _entityFactory;
        public IEntityFactory EntityFactory => _entityFactory;

        private IChunkSystem _chunkSystem;
        public IChunkSystem ChunkSystem => _chunkSystem;

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

        private Action<int> OnLoseCallback;

        public World(CharacterController character, IEntityFactory entityFactory, Action<int> onLoseCallback)
        {
            _character = character;
            _entityFactory = entityFactory;

            _chunkSystem = new ChunkSystem(this);

            OnLoseCallback = onLoseCallback;
        }

        public void OnUpdate()
        {
            _chunkSystem.OnUpdate();
        }

        public void OnStart()
        {
            _character.Init(this);
        }

        public void Reset()
        {
            _character.Reset();
            _chunkSystem.Clear();

        }

        public void OnLose()
        {
            OnLoseCallback(Score);
        }
    }
}
