using DoodleJump.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DoodleJump.Gameplay
{
    public class DragListenerBox : CommonBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public event System.Action<Vector2, Vector2> OnUniversalDrag;
        public event System.Action<Vector2> OnUniversalBeginDrag;
        public event System.Action<Vector2> OnUniversalEndDrag;
        private Camera mainCamera => UniversalCamera.Instance.UnityCamera;

        void Awake()
        {
            var collider = gameObject.AddComponent<BoxCollider2D>();
            collider.size = Vector2.one;
            collider.offset = Vector2.zero;

            transform.localPosition = Vector3.forward;
        }

        public void SetSize(Vector2 size)
        {
            transform.localScale = new Vector3(size.x, size.y, 1f);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 worldDelta =  mainCamera.ScreenToWorldPoint(eventData.delta) - mainCamera.ScreenToWorldPoint(Vector2.zero);
            Vector2 worldPointerPosition = mainCamera.ScreenToWorldPoint(eventData.position);

            if(OnUniversalDrag != null)
            {
                OnUniversalDrag(worldPointerPosition, worldDelta);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Vector2 worldPointerPosition = mainCamera.ScreenToWorldPoint(eventData.position);
            if(OnUniversalBeginDrag != null)
            {
                OnUniversalBeginDrag(worldPointerPosition);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Vector2 worldPointerPosition = mainCamera.ScreenToWorldPoint(eventData.position);
            if(OnUniversalEndDrag != null)
            {
                OnUniversalEndDrag(worldPointerPosition);
            }
        }
    }
}