﻿using System.Collections.Generic;
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

        Box box { get; }
        Vector2 Size { get; }

        void Init(IWorld world);
        void Reset();
    }

    public static class EntityExtensions
    {
        public static void SetY(this IEntity entity, float y)
        {
            var position = entity.Position;
            position.y = y;

            entity.Position = position;
        }

        public static void SetX(this IEntity entity, float x)
        {
            var position = entity.Position;
            position.x = x;

            entity.Position = position;
        }
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


        [SerializeField] private Vector2 boxSize = Vector2.one;
        [SerializeField] private Vector2 boxOffset = Vector2.zero;
        public Box box
        {
            get
            {
                return Box.CreateByPosition(Position + boxOffset, Size);
            }
        }

        public Vector2 Size => boxSize * transform.lossyScale;

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(box.Position, box.Size);
        }

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
