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

            public Configuration(IWorld world, Vector2 startPosition, int platformCount, float minInterval, float maxInterval)
            {
                this.world = world;
                this.startPosition = startPosition;
                this.platformCount = platformCount;
                this.minInterval = minInterval;
                this.maxInterval = maxInterval;
            }
        }
        private Configuration _configuration;

        private IEntityFactory entityFactory => _configuration.world.EntityFactory;

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
                    platform = entityFactory.CreateEntity<MovingPlatform>();
                }else
                {
                    platform = entityFactory.CreateEntity<Platform>();
                }


                platform.Position = startPosition + Vector2.up * _length + Vector2.right * Random.Range(-2, 2);
                
                //add rocket to platform
                Entity collectible = null;
                if(Random.value < 0.1f)
                {
                    var spring = entityFactory.CreateEntity<Spring>();
                    collectible = spring;
                }else if(Random.value < 0.05f)
                {
                    var rocket = entityFactory.CreateEntity<Rocket>();
                    collectible = rocket;
                }

                if(collectible)
                {
                    collectible.Init(_configuration.world);
                    collectible.transform.parent = platform.transform;
                    collectible.Position = platform.Position + Vector2.up * (platform.Size.y / 2f + collectible.Size.y / 2f);
                }


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
