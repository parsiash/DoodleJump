using System.Collections.Generic;
using DoodleJump.Common;
using UnityEngine;

namespace DoodleJump.Gameplay.Chunks
{
    public class SimplePlatformChunk : IPlatformChunk
    {
        public float Length => _length;

        private float _length;
        private Vector2 _startPosition;

        private List<Platform> _platforms;
        private Platform _platformPrefab;
        public IEnumerable<IEntity> Entities => throw new System.NotImplementedException();

        public Box BoundingBox => new Box(_startPosition, _startPosition + Vector2.up * Length);

        private bool _isDisposed;
        public bool IsActive => !_isDisposed;

        public SimplePlatformChunk(Vector2 startPosition, float length, Platform platformPrefab)
        {
            _startPosition = startPosition;
            _length = length;
            _platformPrefab = platformPrefab;

            _platforms = new List<Platform>();
        }

        public void Initialize()
        {
            float currentLength = 0;

            while(currentLength < _length)
            {
                currentLength += Random.value * 2;

                var platform = GameObject.Instantiate<Platform>(_platformPrefab);
                platform.Position = _startPosition + Vector2.up * currentLength + Vector2.right * Random.Range(-2, 2);

                _platforms.Add(platform);
            }
        }

        public void Dispose()
        {
            foreach(var platform in _platforms)
            {
                if(platform)
                {
                    platform.Destroy();
                }
            }
            _platforms.Clear();

            _isDisposed = true;
        }
    }
}
