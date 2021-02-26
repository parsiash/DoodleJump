using UnityEngine;

namespace DoodleJump.Gameplay
{
    public class DestroyablePlatform : Platform
    {
        public override bool OnCharacterJump(CharacterController character)
        {
            base.OnCharacterJump(character);
            return false;
        }
    }
}