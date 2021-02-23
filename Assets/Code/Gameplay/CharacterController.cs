using System.Collections.Generic;
using DoodleJump.Common;
using UnityEngine;

namespace DoodleJump.Gameplay
{
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

        private const float SCREEN_HALF_WIDTH = 3;

        private Vector2 _velocity;
        public Vector2 Veolicty => _velocity;

        private UniversalCamera universalCamera => UniversalCamera.Instance;
        private Camera mainCamera => UniversalCamera.Instance.UnityCamera;

        private CharacterInputController _inputController;

        public override void Init(IWorld world)
        {
            base.Init(world);

            Position = Vector2.zero;
            universalCamera.SetY(0);

            SetVelocity(Vector2.zero);
            _inputController = new CharacterInputController(this, UniversalCamera.Instance.DragListener, dragFactor);
        }


        void Update()
        {
            _inputController.Update(Time.deltaTime);

            //handle gravity movement
            if(_rocket == null)
            {
                SetVelocity(_velocity + Vector2.up * accelerationY * Time.deltaTime);
                ApplyMovement();
            }

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
        }

        public void Jump()
        {
            Jump(jumpSpeed);
        }

        public void Jump(float jumpSpeed)
        {
            SetSpeedY(jumpSpeed);
        }

        public void SpringJump()
        {
            Jump(springJumpSpeed);
        }

        private void ApplyMovement()
        {
            Position += _velocity * Time.deltaTime;
        }

        private void SetSpeedY(float speedY)
        {
            _velocity.y = speedY;
        }

        private void SetVelocity(Vector2 speed)
        {
            _velocity = speed;
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
                if (_velocity.y < 0)
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

        private Rocket _rocket;
        public bool AttachRocket(Rocket rocket)
        {
            if(_rocket != null)
            {
                return false;
            }

            _rocket = rocket;
            return true;
        }

        public void DetachRocket(Rocket rocket)
        {
            _rocket = null;
        }
    }
}
