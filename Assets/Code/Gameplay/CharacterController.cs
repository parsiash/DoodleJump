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

        private const float SCREEN_HALF_WIDTH = 3;

        private Vector2 _speed;

        public void InitializeForTheGame()
        {
            SetSpeed(Vector2.zero);
        }

        void Update()
        {
            SetSpeed(_speed + Vector2.up * accelerationY * Time.deltaTime);
            ApplyMovement();

            if(Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            // if(Input.GetKey(KeyCode.RightArrow))
            // {
            //     MoveX(moveSpeedX * Time.deltaTime);
            // }else if(Input.GetKey(KeyCode.LeftArrow))
            // {
            //     MoveX(-moveSpeedX * Time.deltaTime);
            // }

            var horizontal = Input.GetAxis("Horizontal");
            MoveX(horizontal * moveSpeedX * Time.deltaTime);
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
    }
}
