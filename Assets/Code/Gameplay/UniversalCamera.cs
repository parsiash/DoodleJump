using DoodleJump.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DoodleJump.Gameplay
{
    public interface ICamera
    {
        Box CameraBox { get; }
        Camera UnityCamera { get; }
    }

    [RequireComponent(typeof(Camera))]
    public class UniversalCamera : SingletonBehaviour<UniversalCamera>, ICamera
    {
        public Box CameraBox
        {
            get
            {
                var camera = UnityCamera;
                float height = camera.orthographicSize * 2f;
                return Box.CreateByPosition(transform.position, new Vector2(height * camera.aspect, height));
            }
        }
        public Camera UnityCamera => GetCachedComponent<Camera>();

        public Vector2 Position
        {
            get
            {
                return transform.position;
            }

            set
            {
                var position = transform.position;
                transform.position = new Vector3(value.x, value.y, position.z);
            }
        }

        private DragListenerBox _dragListener;
        public DragListenerBox DragListener
        {
            get
            {
                if(!_dragListener)
                {
                    _dragListener = AddChildWithComponent<DragListenerBox>();

                    if(!GetComponent<Physics2DRaycaster>())
                    {
                        gameObject.AddComponent<Physics2DRaycaster>();
                    }
                }

                return _dragListener;
            }
        }

        void Start()
        {
            FitSize(new Vector2(6, 10));
        }

        public void Move(Vector2 delta)
        {
            Position += delta;
        }

        public void SetY(float y)
        {
            var position = transform.position;
            position.y = y;
            transform.position = position;
        }

        public void FitSize(Vector2 minSize)
        {
            var camera = UnityCamera;

            var minHeight = Mathf.Max(minSize.y, camera.aspect * minSize.x);
            camera.orthographicSize = minHeight / 2f;

            DragListener.SetSize(CameraBox.Size);
        }
    }
}