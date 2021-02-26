using UnityEngine;

namespace DoodleJump.Gameplay
{
    public class Parallax : Entity
    {
        [SerializeField] private float parallaxFactor = 0.5f;
        private Vector2 _previousCameraPosition;
        
        private UniversalCamera universalCamera => UniversalCamera.Instance;

        public void Init(float parallaxFactor, float zIndex)
        {
            this.parallaxFactor = parallaxFactor;
            _previousCameraPosition = universalCamera.Position;
            ZIndex = zIndex;
        }

        public void Init()
        {
            Init(parallaxFactor, 0f);
        }


        void Update()
        {
            var cameraPosition = universalCamera.Position;
            var cameraDelta = cameraPosition - _previousCameraPosition;

            _previousCameraPosition = cameraPosition;

            Position += cameraDelta * parallaxFactor;
        }
    }
}