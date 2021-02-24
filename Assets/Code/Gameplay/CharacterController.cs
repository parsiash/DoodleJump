using System;
using System.Collections.Generic;
using DoodleJump.Common;
using UnityEngine;


namespace DoodleJump.Gameplay
{
    public interface ICharacterMovementController
    {
        Vector2 Velocity { get; }
        void Update(float dt);
        void Jump(float jumpSpeed);
    }

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

        public void Jump(float jumpSpeed)
        {
            _velocity.y = jumpSpeed;
        }
    }

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

    /// <summary>
    /// The main script on the main jumping character.
    /// </summary>
    public class CharacterController : Entity
    {
        [SerializeField] private int jumpSpeed = 12;
        [SerializeField] private int springJumpSpeed = 30;
        [SerializeField] private float accelerationY = -20;
        [SerializeField] private float moveSpeedX = 8;
        [SerializeField] private float dragFactor = 2;

        [Header("Rocket")]
        [SerializeField] private AnimationCurve rocketMovementCurve;
        [SerializeField] private float rocketMovementTime;


        private const float SCREEN_HALF_WIDTH = 3;

        private ICharacterMovementController _verticalMovementController;
        public Vector2 Veolicty => Vector2.up * _verticalMovementController.Velocity;
        public bool IsRocketAttached =>  _verticalMovementController is RocketMovementController;

        private UniversalCamera universalCamera => UniversalCamera.Instance;
        private Camera mainCamera => UniversalCamera.Instance.UnityCamera;

        private CharacterInputController _inputController;

        public override void Init(IWorld world)
        {
            base.Init(world);

            Position = Vector2.zero;
            universalCamera.SetY(0);

            _verticalMovementController = new GravityMovementController(this, Vector2.zero, accelerationY);
            _inputController = new CharacterInputController(this, UniversalCamera.Instance.DragListener, dragFactor);
        }


        void Update()
        {
            _inputController.Update(Time.deltaTime);

            _verticalMovementController.Update(Time.deltaTime);

            //update camera position
            var cameraPos = mainCamera.transform.position;
            universalCamera.SetY(Mathf.Max(cameraPos.y, Position.y));

            //handle losing
            var cameraBox = UniversalCamera.Instance.CamerBox;
            if(Position.y < cameraBox.BottomY)
            {
                _world.OnLose();
            }
        }

        public void MoveXWithSpeed(float factor)
        {
            MoveX(factor * moveSpeedX * Time.deltaTime);
        }

        public void MoveX(float delta)
        {
            //move position by delta
            var position = Position;
            position += Vector2.right * delta;

            //check screen limit
            if(position.x > SCREEN_HALF_WIDTH)
            {
                position.x = -SCREEN_HALF_WIDTH;
            }else if(position.x < -SCREEN_HALF_WIDTH)
            {
                position.x = SCREEN_HALF_WIDTH;
            }

            Position = position;

            if(Mathf.Abs(delta) >= 0.001f)
            {
                var theScale = transform.localScale;
                theScale.x = delta > 0f ? 1f : -1f;
                transform.localScale = theScale;
            }
        }


        public void Jump()
        {
            Jump(jumpSpeed);
        }

        private Animator animator => GetCachedComponentInChildren<Animator>();
        public void Jump(float jumpSpeed)
        {
            _verticalMovementController.Jump(jumpSpeed);

            if(!IsRocketAttached)
            {
                animator.SetTrigger("Jump");
            }
        }

        public void SpringJump()
        {
            Jump(springJumpSpeed);
        }

        public void ApplyVelcity(Vector2 velocity)
        {
            Position += velocity * Time.deltaTime;
        }

        void OnTriggerEnter2D(Collider2D otherCollider)
        {
            OnTrigger(otherCollider);
        }

        void OnTriggerStay2D(Collider2D otherCollider)
        {
            OnTrigger(otherCollider);
        }

        private void OnTrigger(Collider2D otherCollider)
        {
            if (otherCollider.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                if (Veolicty.y < 0)
                {
                    Jump();
                }
            }
            else if (otherCollider.gameObject.layer == LayerMask.NameToLayer("Collectible"))
            {
                var collectible = otherCollider.GetComponentInParent<ICollectible>();
                if(collectible == null)
                {
                    _world.Logger.LogError($"Collectible has no ICollectible component attached. GameObject name : {otherCollider?.gameObject.name}");
                    return;
                }

                collectible.OnCollected(this);
            }
        }

        public bool AttachRocket(Rocket rocket)
        {
            if (IsRocketAttached)
            {
                return false;
            }

            _verticalMovementController = new RocketMovementController(this, rocketMovementCurve, rocketMovementTime, () => DetachRocket(rocket));
           return true;
        }

        public void DetachRocket(Rocket rocket)
        {
            _verticalMovementController = new GravityMovementController(this, _verticalMovementController.Velocity, accelerationY);
        }
    }
}
