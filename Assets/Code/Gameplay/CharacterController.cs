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

        private CharacterInputController _inputController;

        void Start()
        {
            SetSpeed(Vector2.zero);
            _inputController = new CharacterInputController(this, UniversalCamera.Instance.DragListener, dragFactor);
        }


        void Update()
        {
            _inputController.Update(Time.deltaTime);

            //handle gravity movement
            SetSpeed(_speed + Vector2.up * accelerationY * Time.deltaTime);
            ApplyMovement();

            //update camera position
            var cameraPos = mainCamera.transform.position;
            cameraPos.y = Mathf.Max(cameraPos.y, Position.y);
            mainCamera.transform.position = cameraPos;
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
