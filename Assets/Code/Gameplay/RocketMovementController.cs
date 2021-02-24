using System;
using UnityEngine;


namespace DoodleJump.Gameplay
{
    public class RocketMovementController : ICharacterMovementController
    {
        private CharacterController _character;
        public Vector2 Velocity => Vector2.up * _velocity;
        private AnimationCurve _velocityCurve;
        private float _velocity;
        private float _timer;
        private float _moveTime;
        private Action _onFinish;

        public RocketMovementController(CharacterController character, AnimationCurve velocityCurve, float moveTime, Action onFinishCallback)
        {
            _character = character;
            _velocityCurve = velocityCurve;
            _moveTime = moveTime;
            _onFinish = onFinishCallback;

            _velocity = velocityCurve.Evaluate(0f);
            _timer = 0f;
        }

        public void Update(float dt)
        {
            _velocity = _velocityCurve.Evaluate(Mathf.Clamp01(_timer / _moveTime));
            _character.ApplyVelcity(Velocity);
            
            if(_timer >= _moveTime)
            {
                _onFinish();
            }

            _timer += dt;
        }

        public void Jump(float jumpSpeed)
        {
        }
    }
}
