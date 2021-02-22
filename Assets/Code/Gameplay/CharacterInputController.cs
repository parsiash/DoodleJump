using DoodleJump.Common;
using UnityEngine;

namespace DoodleJump.Gameplay
{
    public class CharacterInputController
    {
        private CharacterController _character;
        private DragListenerBox _dragListener;
        private float _dragFactor;
        private const float DRAG_THRESH = 3f;

        private Vector2 _startDragWorldPosition;
        private Vector2 _lastPointPosition;
        private bool _isDragging;
        private float dragSign;

        public CharacterInputController(CharacterController character, DragListenerBox dragListener, float dragFactor)
        {
            _character = character;
            _dragListener = dragListener;
            _dragFactor = dragFactor;

            dragListener.OnUniversalDrag += OnDrag;
            dragListener.OnUniversalBeginDrag += OnBeginDrag;
            dragListener.OnUniversalEndDrag += OnEndDrag;
        }


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

        public void Update(float dt)
        {
            //keep touch positions
            var worldPosition = InputManager.Instance.TouchPosition;
            var worldDelta = worldPosition - _lastPointPosition;
            _lastPointPosition = worldPosition;

            //handle swipe direction change
            float currentDragSign = Mathf.Sign(worldDelta.x);
            if (currentDragSign != dragSign && Mathf.Abs(worldDelta.x) > 0.01f)
            {
                _startDragWorldPosition = worldPosition;
                dragSign = currentDragSign;
            }

            //handle swipe input
            var dragAmount = worldPosition.x - _startDragWorldPosition.x;
            _character.MoveX(dragAmount * _dragFactor * Time.deltaTime);

            //handle key input
            var horizontal = Input.GetAxis("Horizontal");
            _character.MoveXWithSpeed(horizontal);
        }
    }
}