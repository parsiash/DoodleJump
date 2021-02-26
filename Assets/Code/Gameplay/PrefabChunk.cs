using System.Collections.Generic;
using DoodleJump.Common;
using DoodleJump.Gameplay.Chunks;
using UnityEngine;

namespace DoodleJump.Gameplay
{
    public class PrefabChunk : Entity, IChunk
    {
        [SerializeField] private string chunkName;
        public string ChunkName => chunkName;
        
        public float Length => box.Height;

        public Box BoundingBox => box;

        private List<Entity> _entities;
        private List<Entity> entities
        {
            get
            {
                if(_entities == null)
                {
                    _entities = new List<Entity>();
                    _entities.AddRange(GetComponentsInChildren<Entity>());
                    _entities.RemoveAll(e => e == this);
                }

                return _entities;
            }
        }
        public IEnumerable<IEntity> Entities => entities;

        public override void Init(IWorld world)
        {
            base.Init(world);

            foreach(var entity in entities)
            {
                if(entity)
                {
                    entity.Init(world);
                }
            }
        }

        public void OnUpdate()
        {
            if(!IsActive)
            {
                return;
            }

            var cameraBox = UniversalCamera.Instance.CameraBox;

            var chunkBoundingBox = BoundingBox;
            if(chunkBoundingBox.TopY < cameraBox.BottomY)
            {
                Dispose();
            }else
            {
                foreach(var entity in entities)
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

        private bool _isDisposed;
        public bool IsActive => !_isDisposed;

        public void Dispose()
        {
            _isDisposed = true;
            Destroy();
        }

    }
}
