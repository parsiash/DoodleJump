using DoodleJump.Common;
using UnityEngine;

namespace DoodleJump.Gameplay
{
    public interface IPlatform : IEntity
    {
        bool OnCharacterJump(CharacterController character);
    }

    public class Platform : Entity, IPlatform
    {
        private const string ANIM_TRIGGER_HIT = "Hit";
        private Animator animator => GetCachedComponentInChildren<Animator>();

        public virtual bool OnCharacterJump(CharacterController character)
        {
            animator.SetTrigger(ANIM_TRIGGER_HIT);
            return true;
        }
    }
}
