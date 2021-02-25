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
        [Header("Default Movement")]
        [SerializeField] private int jumpSpeed = 12;
        [SerializeField] private int springJumpSpeed = 30;
        [SerializeField] private float accelerationY = -20;
        [SerializeField] private float moveSpeedX = 8;
        [SerializeField] private float dragFactor = 2;

        [Header("Rocket")]
        [SerializeField] private AnimationCurve rocketMovementCurve;
        [SerializeField] private float rocketMovementTime;

        private ICharacterMovementController _movementController;
        public Vector2 Veolicty => Vector2.up * _movementController.Velocity;
        public bool IsRocketAttached =>  _movementController is RocketMovementController;

        private UniversalCamera universalCamera => UniversalCamera.Instance;
        private Camera mainCamera => UniversalCamera.Instance.UnityCamera;

        private CharacterInputController _inputController;

        [SerializeField] private GameObject attackedRocket;

        public override void Init(IWorld world)
        {
            base.Init(world);

            Position = Vector2.zero;
            universalCamera.SetY(0);

            _movementController = new GravityMovementController(this, Vector2.zero, accelerationY);
            _inputController = new CharacterInputController(this, UniversalCamera.Instance.DragListener, dragFactor);
        }


        void Update()
        {
            //@TODO : this is a hack. world should has its own simulation state
            if(_world == null)
            {
                return;
            }
            
            _inputController.Update(Time.deltaTime);

            _movementController.Update(Time.deltaTime);

            //update camera position
            var cameraPos = mainCamera.transform.position;
            universalCamera.SetY(Mathf.Max(cameraPos.y, Position.y));

            //handle losing
            var cameraBox = UniversalCamera.Instance.CameraBox;
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
            var characterBox = box;
            if(characterBox.LeftX > _world.RightEdgeX)
            {
                position.x = _world.LeftEdgeX - characterBox.Size.x / 2f;
            }else if(characterBox.RightX < _world.LeftEdgeX)
            {
                position.x = _world.RightEdgeX + characterBox.Size.x / 2f;
            }

            Position = position;

            if(Mathf.Abs(delta) >= 0.001f)
            {
                var theScale = transform.localScale;
                theScale.x = delta > 0f ? 1f : -1f;
                transform.localScale = theScale;
            }
        }


        public bool Jump()
        {
            return Jump(jumpSpeed);
        }

        private Animator animator => GetCachedComponentInChildren<Animator>();
        public bool Jump(float jumpSpeed)
        {
            bool jumped = _movementController.Jump(jumpSpeed);
            if(jumped)
            {
                animator.SetTrigger("Jump");
            }

            return jumped;
        }

        public bool SpringJump()
        {
            return Jump(springJumpSpeed);
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

            attackedRocket.SetActive(true);
            _movementController = new RocketMovementController(this, rocketMovementCurve, rocketMovementTime, () => DetachRocket(rocket));
           return true;
        }

        public void DetachRocket(Rocket rocket)
        {
            attackedRocket.SetActive(false);
            _movementController = new GravityMovementController(this, _movementController.Velocity, accelerationY);
            
            //spawn falling rocket
            var fallingRocket = _world.EntityFactory.CreateEntity<FallingRocket>(Position);
            fallingRocket.StartFalling();
        }
    }
}
