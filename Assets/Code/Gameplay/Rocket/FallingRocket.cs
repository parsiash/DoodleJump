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

        public override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);

            if(box.TopY < UniversalCamera.Instance.CameraBox.BottomY)
            {
                Destroy();
            }
        }
    }
}
