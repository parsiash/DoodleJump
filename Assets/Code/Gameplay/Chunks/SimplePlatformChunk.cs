using System.Collections.Generic;
using DoodleJump.Common;
using UnityEngine;

namespace DoodleJump.Gameplay.Chunks
{
    public class SimplePlatformChunk : IPlatformChunk
    {
        private float _length;
        public float Length => _length;

        private List<Platform> _platforms;
        public IEnumerable<IEntity> Entities => _platforms;

        private Vector2 startPosition => _configuration.startPosition;
        public Box BoundingBox => new Box(startPosition, startPosition + Vector2.up * Length);

        private bool _isDisposed;
        public bool IsActive => !_isDisposed;

        public class Configuration
        {
            public IWorld world { get; set; }
            public Vector2 startPosition { get; set; }
            public int platformCount { get; set; }
            public float minInterval { get; set; }
            public float maxInterval { get; set; }
            public Platform platformPrefab { get; set; }
            public MovingPlatform movingPlatformPrefab { get; set; }

            public Configuration(IWorld world, Vector2 startPosition, int platformCount, float minInterval, float maxInterval, Platform platformPrefab, MovingPlatform movingPlatformPrefab)
            {
                this.world = world;
                this.startPosition = startPosition;
                this.platformCount = platformCount;
                this.minInterval = minInterval;
                this.maxInterval = maxInterval;
                this.platformPrefab = platformPrefab;
                this.movingPlatformPrefab = movingPlatformPrefab;
            }
        }
        private Configuration _configuration;

        public SimplePlatformChunk(Configuration configuration)
        {
            _length = 0f;
            _configuration = configuration;
            _platforms = new List<Platform>();
        }

        public void Initialize()
        {
            _length = 0f;

            for(int i = 0; i < _configuration.platformCount; i++)
            {
                var interval = Random.Range(_configuration.minInterval, _configuration.maxInterval);
                _length += interval;

                Platform platform = null;
                if(Random.value > 0.7f)
                {
                    platform = GameObject.Instantiate<MovingPlatform>(_configuration.movingPlatformPrefab);
                }else
                {
                    platform = GameObject.Instantiate<Platform>(_configuration.platformPrefab);
                }

                platform.Position = startPosition + Vector2.up * _length + Vector2.right * Random.Range(-2, 2);
                platform.Init(_configuration.world);
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
