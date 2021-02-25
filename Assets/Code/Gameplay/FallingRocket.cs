using UnityEngine;

namespace DoodleJump.Gameplay
{
    public class FallingRocket : Entity
    {
        private Parallax parallax => GetCachedComponent<Parallax>();

        public void StartFalling()
        {
            parallax.Init();
        }

        void Update()
        {
            if(box.TopY < UniversalCamera.Instance.CameraBox.BottomY)
            {
                Destroy();
            }
        }
    }
}
