using System.Collections.Generic;
using DoodleJump.Common;
using UnityEngine;

namespace DoodleJump.Gameplay
{
    public class GameplayController : CommonBehaviour
    {
        private CharacterController _character;
        private List<IChunk> _chunks;
        private IChunkSystem _chunkSystem;

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
        
        [SerializeField] private Platform platformPrefab;

        public void Initialize(IChunkSystem chunkSystem, CharacterController character)
        {
            _character = character;
            _chunks = new List<IChunk>();
            _score = 0;
        }

        void Update()
        {
            if(!_character)
            {
                return;
            }

            var characterPosiiton = _character.Position;
            var box = Box.CreateByPosition(characterPosiiton, new Vector2(10, 10));

            float topY = box.BottomY;

            //remove chunks that are past
            foreach(var chunk in _chunks)
            {
                var chunkBoundingBox = chunk.BoundingBox;
                if(chunkBoundingBox.TopY < box.BottomY)
                {
                    chunk.Dispose();
                }else
                {
                    topY = Mathf.Max(chunkBoundingBox.TopY, topY);
                }
            }
            _chunks.RemoveAll(chunk => !chunk.IsActive);

            //create new chunk on top
            if(box.TopY > topY - 1)
            {
                var chunk = CreateChunk(topY, 10);
                _chunks.Add(chunk);
            }
        }

        IChunk CreateChunk(float bottomY, float length)
        {
            var chunk = new SimplePlatformChunk(Vector2.up * bottomY, length, platformPrefab);
            chunk.Initialize();
            return chunk;
        }
    }

    public interface IChunkSystem
    {
        IChunk CreateChunk(float bottomY, float length);
    }
}
