
#if UNITY_EDITOR || UNITY_STANDALONE
#define MOUSE_INPUT
#elif UNITY_IOS || UNITY_ANDROID
#define TOUCH_INPUT
#endif

using UnityEngine;
using UnityEngine.EventSystems;


namespace DoodleJump.Common
{
    public class InputManager : SingletonBehaviour<InputManager>
    {
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

        EventSystem _eventSystem;
        EventSystem eventSystem
        {
            get
            {
                if(!_eventSystem)
                {
                    _eventSystem = EventSystem.current;
                }

                return _eventSystem;
            }
        }

        private bool isDown;
        private bool isContinued;
        private bool isUp;

        public bool IsDown => isDown;
        public bool IsContinue => isContinued;
        public bool IsUp => isUp;

        private Touch touch;


        public Vector2 TouchPosition
        {
            get
            {
#if MOUSE_INPUT
                
                return mainCamera.ScreenToWorldPoint(Input.mousePosition);
#elif TOUCH_INPUT
                UpdateTouch();
                if(mainCamera && Input.touchCount > 0)
                {
                    return mainCamera.ScreenToWorldPoint(touch.position);
                }
#endif
                return Vector2.zero;
            }
        }

        public bool IsPointerOnUI
        {
            get
            {
                var eventSystem = this.eventSystem;
                if(eventSystem)
                {
#if MOUSE_INPUT
                    return eventSystem.IsPointerOverGameObject();
#elif TOUCH_INPUT
                    return eventSystem.IsPointerOverGameObject(touch.fingerId);
#endif
                }

                return false;
            }
        }

        void Update()
        {
#if MOUSE_INPUT

            isDown = Input.GetMouseButtonDown(0);
            isContinued = Input.GetMouseButton(0);
            isUp = Input.GetMouseButtonUp(0);

#elif TOUCH_INPUT
            UpdateTouch();
#endif
        }

        private int _lastTouchUpdateFrame;
        private void UpdateTouch()
        {
            if(_lastTouchUpdateFrame == Time.frameCount)
            {
                return;
            }
            _lastTouchUpdateFrame = Time.frameCount;

            if(Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);

                isDown = touch.phase == TouchPhase.Began;
                isContinued = touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary;
                isUp = touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled;

            }else
            {
                if(isDown || isContinued)
                {
                    isUp = true;
                }else
                {
                    isUp = false;
                }

                isDown = false;
                isContinued = false;
            }
        }
    }
}