using System.Collections.Generic;
using DoodleJump.Common;
using DoodleJump.Gameplay.Chunks;
using UnityEngine;

namespace DoodleJump.Gameplay
{
    public interface IChunkSystem
    {
        void OnUpdate();
        void Clear();
    }

    /// <summary>
    /// The system responsible for generating and destroying chunks at runtime.
    /// </summary>
    public class ChunkSystem : IChunkSystem
    {
        private IWorld _world;
        private ChunkSystemConfiguration _configuration;
        private List<IChunk> _chunks;
        
        private float _lastVerticalMovingChunkY;
        private float _lastReactiveChunkY;
        
        private UniversalCamera universalCamera => UniversalCamera.Instance;
        private IEntityFactory entityFactory => _world.EntityFactory;


        public ChunkSystem(IWorld world, ChunkSystemConfiguration configuration)
        {
            _world = world;
            _configuration = configuration;
            _chunks = new List<IChunk>();

            CreatePrefabChunk("InitialChunk", -5f);
        }

        private PrefabChunk CreatePrefabChunk(string chunkName, float bottomY)
        {
            var prefabChunk = _world.EntityFactory.CreateEntity<PrefabChunk>(chunkName);
            prefabChunk.Position = Vector2.up * (bottomY + prefabChunk.Size.y * 0.5f);
            
            prefabChunk.InitializeEntities();
            _chunks.Add(prefabChunk);

            return prefabChunk;
        }

        public void OnUpdate()
        {
            var characterPosiiton = _world.Character.Position;

            var cameraBox = universalCamera.CameraBox;
            float topY = cameraBox.BottomY;

            foreach(var chunk in _chunks)
            {
                if(chunk != null && chunk.IsActive)
                {
                    chunk.OnUpdate();
                    topY = Mathf.Max(chunk.BoundingBox.TopY, topY);
                }
            }
            _chunks.RemoveAll(chunk => !chunk.IsActive);

            //create new chunk on top
            if(cameraBox.TopY > topY - 1)
            {
                IChunk chunk = null;
                if(topY - _lastVerticalMovingChunkY > _configuration.verticalMovingChunkInterval)
                {
                    if(Random.value < _configuration.verticalMovingChunkChance)
                    {
                        var prefabChunk = CreatePrefabChunk("MovingPlatformChunk", topY);
                        chunk = prefabChunk;

                        _lastVerticalMovingChunkY = topY;
                    }
                }

                if(chunk == null)
                {
                    if(topY - _lastReactiveChunkY > _configuration.reactiveChunkInterval)
                    {
                        if(Random.value < _configuration.reactiveChunkChance)
                        {
                            var prefabChunk = CreatePrefabChunk("ReactiveChunk", topY);
                            chunk = prefabChunk;

                            _lastReactiveChunkY = topY;
                        }
                    }
                }

                //create simple if non other chunk type is created
                if(chunk == null)
                {
                    chunk = CreateSimpleChunk(topY, 10);
                }
            }
        }

        private float GetMinVerticalInterval(float bottomY)
        {
            return Mathf.Lerp(1f, 1.5f, bottomY / 400f);
        }

        private float GetMaxVerticalInterval(float bottomY)
        {
            return Mathf.Lerp(1, 3, bottomY / 400f);
        }

        IChunk CreateSimpleChunk(float bottomY, float length)
        {
            var minInterval = GetMinVerticalInterval(bottomY);
            var maxInterval = GetMaxVerticalInterval(bottomY);

            var chunk = new Chunks.SimpleChunk(_world, Vector2.up * bottomY);

            for(int i = 0; i < 10; i++)
            {
                //add rocket to platform
                var collectible = CreateRandomCollectible(bottomY);

                var intervalY = Random.Range(minInterval, maxInterval);
                var platformBottomEdgeY = chunk.BoundingBox.TopY + intervalY;

                var platform = CreateRandomPlatform(bottomY, collectible, platformBottomEdgeY);

                chunk.AddEntity(platform);
                if (collectible)
                {
                    chunk.AddEntity(collectible);
                }

                if (platform.GetType() == typeof(Platform))
                {
                    if (Random.value < _configuration.destroyablePlatformChance)
                    {
                        var platformBox = platform.box;
                        if (platformBox.RightX < _world.RightEdgeX - platformBox.Size.x * 1.5f)
                        {
                            var destroyablePlatform = entityFactory.CreateEntity<DestroyablePlatform>();
                            destroyablePlatform.Position = new Vector2(Random.Range(platformBox.RightX + platformBox.Size.x * _configuration.destroyablePlatformMarginFactor, _world.RightEdgeX - platformBox.Size.x * 0.75f), platform.Position.y);
                            destroyablePlatform.Init(_world);
                            chunk.AddEntity(destroyablePlatform);
                        }
                    }
                }
            }

            _chunks.Add(chunk);
            return chunk;
        }

        private float _lastRocketY;
        private float _lastSpringY;

        private Entity CreateRandomCollectible(float y)
        {
            Entity collectible = null;
            if (Random.value < _configuration.springChance && y - _lastSpringY > _configuration.springSpawnInterval)
            {
                collectible = entityFactory.CreateEntity<Spring>();
                _lastSpringY = y;
            }
            else if (Random.value < _configuration.rocketChance && y - _lastRocketY > _configuration.rocketSpawnInterval)
            {
                collectible = entityFactory.CreateEntity<Rocket>();
                _lastRocketY = y;
            }

            return collectible;
        }

        private Platform CreateRandomPlatform(float bottomY, Entity collectible, float platformBottomEdgeY)
        {
            Platform platform;
            if (Random.value > _configuration.movingPlatformChance)
            {
                platform = CreatePlatform<MovingPlatform>(platformBottomEdgeY, collectible);
            }
            else
            {
                if (bottomY > _configuration.oneTimePlatformMinY && Random.value > _configuration.oneTimePlatformChance)
                {
                    platform = CreatePlatform<OneTimePlatform>(platformBottomEdgeY, collectible);
                }
                else
                {
                    platform = CreatePlatform<Platform>(platformBottomEdgeY, collectible);
                }
            }

            return platform;
        }

        private T CreatePlatform<T>(float platformBottomEdgeY, Entity placedEntity = null) where T : Platform
        {    
            var platform = entityFactory.CreateEntity<T>();
            if(!platform)
            {
                Common.Logger.Instance.LogError("No platform created.");
                return null;
            }

            var platformX = Random.Range(_world.LeftEdgeX + platform.Size.x, _world.RightEdgeX - platform.Size.x);
            platform.Position = new Vector2(platformX, platformBottomEdgeY +  platform.Size.y * 0.5f);
            platform.ZIndex = 2f;

            if(placedEntity)
            {
                placedEntity.transform.parent = platform.transform;
                placedEntity.Position = platform.Position + Vector2.up * (platform.Size.y * 0.5f + placedEntity.Size.y * 0.5f);
                placedEntity.ZIndex = 1f;
            }

            platform.Init(_world);
            if(placedEntity)
            {
                placedEntity.Init(_world);
            }

            return platform;
        }
        
        private void ClearChunks()
        {
            foreach(var chunk in _chunks)
            {
                if(chunk != null)
                {
                    chunk.Dispose();
                }
            }

            _chunks.Clear();
        }

        public void Clear()
        {
            ClearChunks();
        }
    }
}
