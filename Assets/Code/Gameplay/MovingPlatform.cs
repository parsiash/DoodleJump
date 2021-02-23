using UnityEngine;

namespace DoodleJump.Gameplay
{
    public class MovingPlatform : Platform
    {
        private Vector2 source => new Vector2(_world.LeftEdgeX, _initialPosition.y);
        private Vector2 destination =>  new Vector2(_world.RightEdgeX, _initialPosition.y);
        private Vector2 _initialPosition;
        private float _timeOffset;
        [SerializeField] private float speed;

        public override void Init(IWorld world)
        {
            base.Init(world);
            _initialPosition = transform.position;
            _timeOffset = Random.value;
        }

        void Update()
        {
            var t = Mathf.PingPong((Time.time + _timeOffset) * speed, 1);
            Position = Vector2.Lerp(source, destination, t);
        }
    }
}
