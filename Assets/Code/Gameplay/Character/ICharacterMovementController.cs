using UnityEngine;


namespace DoodleJump.Gameplay
{
    /// <summary>
    /// An abstraction for character vertical movement controller.
    /// </summary>
    public interface ICharacterMovementController
    {
        Vector2 Velocity { get; }
        void Update(float dt);
        bool Jump(float jumpSpeed);
    }
}
