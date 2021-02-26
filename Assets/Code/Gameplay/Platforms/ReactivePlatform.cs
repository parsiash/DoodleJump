using UnityEngine;

namespace DoodleJump.Gameplay
{
    public class ReactivePlatform : Platform
    {
        private const float marginFactor = 0.75f;
        [SerializeField] private float moveSpeed = 3f;

        private float _targetX;

        public override void Init(IWorld world)
        {
            base.Init(world);

            _targetX = GetRandomXPosition();
        }

        private float GetRandomXPosition()
        {
            var currentX = Position.x;

            if(currentX > 0)
            {
                return Random.Range(_world.LeftEdgeX * marginFactor, 0f);
            }else
            {
                return Random.Range(0f, _world.RightEdgeX * marginFactor);
            }
        }

        public void React()
        {
            _targetX = GetRandomXPosition();
        }

        public override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
            
            this.SetX(Mathf.MoveTowards(Position.x, _targetX, moveSpeed * Time.deltaTime));
        }
    }
}
