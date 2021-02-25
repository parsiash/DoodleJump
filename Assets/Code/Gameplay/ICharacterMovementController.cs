using UnityEngine;


namespace DoodleJump.Gameplay
{
    public interface ICharacterMovementController
    {
        Vector2 Velocity { get; }
        void Update(float dt);
        bool Jump(float jumpSpeed);
    }
}
