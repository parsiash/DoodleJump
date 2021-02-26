using DoodleJump.Common;
using UnityEngine;
using UnityEngine.Events;

namespace DoodleJump.Gameplay
{
    public interface IPlatform : IEntity
    {
        bool OnCharacterJump(CharacterController character);
    }

    public class Platform : Entity, IPlatform
    {
        public event UnityAction OnJump;
        private const string ANIM_TRIGGER_HIT = "Hit";
        private Animator animator => GetCachedComponentInChildren<Animator>();

        public virtual bool OnCharacterJump(CharacterController character)
        {
            animator.SetTrigger(ANIM_TRIGGER_HIT);
            
            if(OnJump != null)
            {
                OnJump();
            }
            return true;
        }
    }
}
