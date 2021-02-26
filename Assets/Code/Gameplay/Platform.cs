using System;
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
        //jump and events
        private bool _isJumpedUpon;
        public event Action OnJump;

        //animation
        private const string ANIM_TRIGGER_HIT = "Hit";
        private Animator animator => GetCachedComponentInChildren<Animator>();

        public override void Init(IWorld world)
        {
            base.Init(world);
            _isJumpedUpon = false;
        }

        public virtual bool OnCharacterJump(CharacterController character)
        {
            animator.SetTrigger(ANIM_TRIGGER_HIT);

            if(!_isJumpedUpon)
            {
                _world.OnPlatformJumpFirstTime(this);
                _isJumpedUpon = true;
            }
            
            if(OnJump != null)
            {
                OnJump();
            }
            return true;
        }

        public override void Reset()
        {
            base.Reset();
            OnJump = null;
        }
    }
}
