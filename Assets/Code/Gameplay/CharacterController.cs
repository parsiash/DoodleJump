using System.Collections;
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
        [SerializeField] private float accelerationY = -20;
        [SerializeField] private float moveSpeedX = 8;
        [SerializeField] private float dragFactor = 2;

        private const float SCREEN_HALF_WIDTH = 3;

        private Vector2 _speed;

        private Camera _mainCamera;
        private Camera mainCamera
        {
            get
            {
                if(!_mainCamera)
                {
                    _mainCamera = Camera.main;
                }

                return _mainCamera;
            }
        }

        public void InitializeForTheGame()
        {
            SetSpeed(Vector2.zero);
        }

        private DragListenerBox dragListener => UniversalCamera.Instance.DragListener;

        void Start()
        {
            dragListener.OnUniversalDrag += OnDrag;
            dragListener.OnUniversalBeginDrag += OnBeginDrag;
            dragListener.OnUniversalEndDrag += OnEndDrag;
        }

        private Vector2 _startDragWorldPosition;
        private Vector2 _lastPointPosition;
        private bool _isDragging;
        private float dragSign;

        private const float DRAG_THRESH = 3f;

        public void OnDrag(Vector2 worldPosition, Vector2 WorldDelta)
        {
        }

        public void OnBeginDrag(Vector2 worldPosition)
        {
            _startDragWorldPosition = worldPosition;
            _lastPointPosition = worldPosition;
            dragSign = 0f;
            _isDragging = true;
        }

        public void OnEndDrag(Vector2 worldPosition)
        {
            _isDragging = false;
        }

        void Update()
        {
            var worldPosition = InputManager.Instance.TouchPosition;
            var worldDelta = worldPosition - _lastPointPosition;
            _lastPointPosition = worldPosition;

            float currentDragSign = Mathf.Sign(worldDelta.x);
            if (currentDragSign != dragSign && Mathf.Abs(worldDelta.x) > 0.01f)
            {
                _startDragWorldPosition = worldPosition;
                dragSign = currentDragSign;
            }
            var dragAmount = worldPosition.x - _startDragWorldPosition.x;
            MoveX(dragAmount * dragFactor * Time.deltaTime);

            //handle gravity movement
            SetSpeed(_speed + Vector2.up * accelerationY * Time.deltaTime);
            ApplyMovement();

            //handle x movement
            var horizontal = Input.GetAxis("Horizontal");
            if(Input.GetKey(KeyCode.RightArrow))
            {
                horizontal = 1f;
            }else if(Input.GetKey(KeyCode.LeftArrow))
            {
                horizontal = -1f;
            }
            MoveX(horizontal * moveSpeedX * Time.deltaTime);

            //update camera position
            var cameraPos = mainCamera.transform.position;
            cameraPos.y = Mathf.Max(cameraPos.y, Position.y);
            mainCamera.transform.position = cameraPos;
        }

        private void MoveX(float delta)
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
            SetSpeedY(jumpSpeed);
        }

        private void ApplyMovement()
        {
            Position += _speed * Time.deltaTime;
        }

        private void SetSpeedY(float speedY)
        {
            _speed.y = speedY;
        }

        private void SetSpeed(Vector2 speed)
        {
            _speed = speed;
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
                if (_speed.y < 0)
                {
                    Jump();
                }
            }
        }
    }
}
