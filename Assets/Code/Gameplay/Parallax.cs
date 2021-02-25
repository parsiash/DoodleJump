using UnityEngine;

namespace DoodleJump.Gameplay
{
    public class Parallax : Entity
    {
        private UniversalCamera universalCamera => UniversalCamera.Instance;

        private Vector2 _previousCameraPosition;
        private float _parallaxFactor = 0.5f;

        public void Init(float parallaxFactor, float zIndex)
        {
            _parallaxFactor = parallaxFactor;
            _previousCameraPosition = universalCamera.Position;
            ZIndex = zIndex;
        }

        void Update()
        {
            var cameraPosition = universalCamera.Position;
            var cameraDelta = cameraPosition - _previousCameraPosition;

            _previousCameraPosition = cameraPosition;

            Position += cameraDelta * _parallaxFactor;
        }
    }
}