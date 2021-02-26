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

    public class ChunkSystem : IChunkSystem
    {
        private IWorld _world;
        private List<IChunk> _chunks;
        private UniversalCamera universalCamera => UniversalCamera.Instance;

        public ChunkSystem(IWorld world)
        {
            _world = world;
            _chunks = new List<IChunk>();
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
                if(topY - _lastVerticalMovingChunkY > 150)
                {
                    if(Random.value < 0.2f)
                    {
                        var prefabChunk = _world.EntityFactory.CreateEntity<PrefabChunk>("MovingPlatformChunk");
                        prefabChunk.Position = Vector2.up * (topY + prefabChunk.Size.y * 0.5f);
                        prefabChunk.Init(_world);
                        chunk = prefabChunk;

                        _lastVerticalMovingChunkY = topY;
                    }
                }

                if(chunk == null)
                {
                    chunk = CreateSimpleChunk(topY, 10);
                }

                _chunks.Add(chunk);
            }
        }

        private float _lastVerticalMovingChunkY;

        private float GetMinVerticalInterval(float bottomY)
        {
            return Mathf.Lerp(1f, 1.5f, bottomY / 100f);
        }

        private float GetMaxVerticalInterval(float bottomY)
        {
            return Mathf.Lerp(1, 3, bottomY / 100f);
        }

        private const float oneTimePlatformMinY = 150f;
        private const float oneTimePlatformChance = 0.8f;
        private const float springChance = 0.1f;
        private const float rocketChance = 0.05f;
        private const float movingPlatformChance = 0.9f;
        private const float destroyablePlatformChance = 0.2f;
        private const float destroyablePlatformMarginFactor = 0.75f;

        private IEntityFactory entityFactory => _world.EntityFactory;

        IChunk CreateSimpleChunk(float bottomY, float length)
        {
            var minInterval = GetMinVerticalInterval(bottomY);
            var maxInterval = GetMaxVerticalInterval(bottomY);

            var chunk = new Chunks.SimpleChunk(Vector2.up * bottomY);

            for(int i = 0; i < 10; i++)
            {
                //add rocket to platform
                var collectible = CreateRandomCollectible();

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
                    if (Random.value < destroyablePlatformChance)
                    {
                        var platformBox = platform.box;
                        if (platformBox.RightX < _world.RightEdgeX - platformBox.Size.x * 1.5f)
                        {
                            var destroyablePlatform = entityFactory.CreateEntity<DestroyablePlatform>();
                            destroyablePlatform.Position = new Vector2(Random.Range(platformBox.RightX + platformBox.Size.x * destroyablePlatformMarginFactor, _world.RightEdgeX - platformBox.Size.x * 0.75f), platform.Position.y);
                            destroyablePlatform.Init(_world);
                            chunk.AddEntity(destroyablePlatform);
                        }
                    }
                }
            }
            return chunk;
        }

        private Entity CreateRandomCollectible()
        {
            Entity collectible = null;
            if (Random.value < springChance)
            {
                collectible = entityFactory.CreateEntity<Spring>();
            }
            else if (Random.value < rocketChance)
            {
                collectible = entityFactory.CreateEntity<Rocket>();
            }

            return collectible;
        }

        private Platform CreateRandomPlatform(float bottomY, Entity collectible, float platformBottomEdgeY)
        {
            Platform platform;
            if (Random.value > movingPlatformChance)
            {
                platform = CreatePlatform<MovingPlatform>(platformBottomEdgeY, collectible);
            }
            else
            {
                if (bottomY > oneTimePlatformMinY && Random.value > oneTimePlatformChance)
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
