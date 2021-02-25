using UnityEngine;


namespace DoodleJump.Gameplay
{
    public class GravityMovementController : ICharacterMovementController
    {
        private CharacterController _character;
        private float _accelerationY;
        private Vector2 _velocity;
        public Vector2 Velocity => _velocity;

        public GravityMovementController(CharacterController character, Vector2 startVelocity, float accelerationY)
        {
            _character = character;
            _velocity = startVelocity;
            _accelerationY = accelerationY;
        }

        public void Update(float dt)
        {
            //handle gravity movement
            SetVelocity(_velocity + Vector2.up * _accelerationY * dt);
            _character.ApplyVelcity(_velocity);
        }

        private void SetVelocity(Vector2 speed)
        {
            _velocity = speed;
        }

        public bool Jump(float jumpSpeed)
        {
            if(jumpSpeed > _velocity.y)
            {
                _velocity.y = jumpSpeed;
                return true;
            }

            return false;
        }
    }
}
