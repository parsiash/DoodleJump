using System.Collections.Generic;
using DoodleJump.Common;
using UnityEngine;

namespace DoodleJump.Gameplay
{
    public interface IEntity
    {
        IWorld World { get; }
        int Id { get; }
        Vector2 Position { get; set; }
        float ZIndex { get; set; }

        void Init(IWorld world);
        void Reset();
    }

    public class Entity : CommonBehaviour, IEntity
    {
        protected IWorld _world;
        public IWorld World => _world;

        public virtual Vector2 Position 
        { 
            get
            {
                return transform.position;
            }

            set
            {
                var currentPosition = transform.position;
                transform.position = new Vector3(value.x, value.y, currentPosition.z);
            }
        }

        public virtual float ZIndex
        { 
            get
            {
                return transform.position.z;
            }

            set
            {
                var position = transform.position;
                position.z = value;
                transform.position = position;
            }
        }


        public int Id => gameObject.GetInstanceID();

        public virtual void Init(IWorld world)
        {
            _world = world;
        }

        public virtual void Reset()
        {
            _world = null;
        }
    }
}
