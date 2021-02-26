using System.Collections.Generic;
using DoodleJump.Common;
using UnityEngine;

namespace DoodleJump.Gameplay.Chunks
{
    public class SimpleChunk : IPlatformChunk
    {
        public IWorld World { get; private set; }

        private Vector2 _startPosition;
        private float _length;
        public float Length => _length;
        public Box BoundingBox => new Box(_startPosition, _startPosition + Vector2.up * Length);

        private List<Entity> _entities;
        public IEnumerable<IEntity> Entities => _entities;

        private bool _isDisposed;
        public bool IsActive => !_isDisposed;


        public SimpleChunk(IWorld world, Vector2 startPosition)
        {
            World = world;

            _startPosition = startPosition;
            _length = 0f;

            _entities = new List<Entity>();;
        }

        public void Dispose()
        {
            foreach(var entity in _entities)
            {
                if(entity)
                {
                    entity.Destroy();
                }
            }
            _entities.Clear();

            _isDisposed = true;
        }

        public void AddEntity(Entity entity)
        {
            _entities.Add(entity);
            _length +=  Mathf.Max(0f, entity.box.TopY - BoundingBox.TopY);
        }
        
        public void OnUpdate()
        {
            var cameraBox = UniversalCamera.Instance.CameraBox;

            var chunkBoundingBox = BoundingBox;
            if(chunkBoundingBox.TopY < cameraBox.BottomY)
            {
                Dispose();
            }else
            {
                foreach(var entity in _entities)
                {
                    if(entity)
                    {
                        if(entity.box.TopY < cameraBox.BottomY)
                        {
                            entity.Destroy();
                        }
                    }
                }
                _entities.RemoveAll(e => !e);
            }
        }
    }
}
