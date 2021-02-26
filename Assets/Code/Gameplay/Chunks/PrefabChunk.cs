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
                    _entities.AddRange(GetComponentsInChildren<Entity>(true));
                    _entities.RemoveAll(e => e == this);
                }

                return _entities;
            }
        }
        public IEnumerable<IEntity> Entities => entities;


        public virtual void InitializeEntities()
        {
            foreach(var entity in entities)
            {
                if(entity)
                {
                    _world.AddEntity(entity);
                    entity.SetActive(true);
                }
            }
        }

        public virtual void OnUpdate()
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
                            entity.SetActive(false);
                            World.RemoveEntity(entity);
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
            foreach(var entity in entities)
            {
                World.RemoveEntity(entity);
            }

            _isDisposed = true;
            Destroy();
        }

    }
}
