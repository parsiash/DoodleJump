using DoodleJump.Common;
using UnityEngine;

namespace DoodleJump.Common
{
    public interface IEntity
    {
        Vector2 Position { get; set; }
        float ZIndex { get; set; }
        void Reset();
    }

    public class Entity : CommonBehaviour, IEntity
    {
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

        public virtual void Reset()
        {
        }
    }
}
