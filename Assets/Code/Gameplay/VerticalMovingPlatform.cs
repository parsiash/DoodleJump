using UnityEngine;

namespace DoodleJump.Gameplay
{
    public class VerticalMovingPlatform : MovingPlatform
    {
        [SerializeField] protected bool startMovingDown;
        [SerializeField] protected float verticalRange = 3;

        protected override void InitSourceAndDestination()
        {

            if(startMovingDown)
            {
                source = _initialPosition - Vector2.up * verticalRange;
                destination = _initialPosition + Vector2.up * verticalRange;
            }else
            {
                source = _initialPosition + Vector2.up * verticalRange;
                destination = _initialPosition - Vector2.up * verticalRange;
            }
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(box.Position, box.Size + Vector2.up * verticalRange * 2f);
        }
    }
}
