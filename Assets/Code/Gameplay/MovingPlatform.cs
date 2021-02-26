using UnityEngine;

namespace DoodleJump.Gameplay
{
    public class MovingPlatform : Platform
    {
        protected Vector2 source { get; set; }
        protected Vector2 destination { get; set; } 
        protected Vector2 _initialPosition;
        protected float _timeOffset;
        [SerializeField] protected float speed;

        public override void Init(IWorld world)
        {
            base.Init(world);
            _initialPosition = transform.position;
            _timeOffset = Random.value;

            InitSourceAndDestination();
        }

        protected virtual void InitSourceAndDestination()
        {
            source = new Vector2(_world.LeftEdgeX + box.Size.x / 2f, _initialPosition.y);
            destination = new Vector2(_world.RightEdgeX - box.Size.x / 2f, _initialPosition.y);
        }

        void Update()
        {
            var t = Mathf.PingPong((Time.time + _timeOffset) * speed, 1);
            Position = Vector2.Lerp(source, destination, t);
        }
    }
}
