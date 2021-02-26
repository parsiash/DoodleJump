using DoodleJump.Common;
using UnityEngine;

namespace DoodleJump.Gameplay
{
    /// <summary>
    /// Handls character swipe and key control.
    /// </summary>
    public class CharacterInputController
    {
        private CharacterController _character;
        private float _moveSpeedX;

        private bool _isDragging;
        private Vector2 _startDragWorldPosition;
        private float _netDragPositionX;
        private float _characterStartDragPositionX;
        InputManager inputManager => InputManager.Instance;

        public CharacterInputController(CharacterController character, float moveSpeedX)
        {
            _character = character;
            _moveSpeedX = moveSpeedX;
        }

        public void OnBeginDrag(Vector2 worldPosition)
        {
            _startDragWorldPosition = worldPosition;
            _characterStartDragPositionX = _character.Position.x;
            _netDragPositionX = 0f;
            _isDragging = true;
        }

        public void OnEndDrag(Vector2 worldPosition)
        {
            _isDragging = false;
        }

        public void Update(float dt)
        {
            var worldPosition = InputManager.Instance.TouchPosition;

            //check drag state
            if (inputManager.IsDown && !inputManager.IsPointerOnUI)
            {
                OnBeginDrag(worldPosition);
            }else if(InputManager.Instance.IsUp)
            {
                OnEndDrag(worldPosition);
            }

            //handle swipe input
            if(_isDragging)
            {
                var targetDeltaX = (worldPosition - _startDragWorldPosition).x;
                var deltaX = Mathf.MoveTowards(_netDragPositionX, targetDeltaX, _moveSpeedX * Mathf.Clamp01(Mathf.Abs(targetDeltaX - _netDragPositionX)) * Time.deltaTime) - _netDragPositionX;
                _netDragPositionX += deltaX;
                _character.MoveX(deltaX);
            }

            //handle key input
            var horizontal = Input.GetAxis("Horizontal") * 0.5f;
            _character.MoveXWithSpeed(horizontal);
        }
    }
}