using UnityEngine;

namespace DoodleJump.Gameplay
{
    public class Spring : Entity, ICollectible
    {
        private const string  ANIM_TRIGGER_PRESS = "Press";
        private Animator animator => GetCachedComponentInChildren<Animator>();

        public void OnCollected(CharacterController character)
        {
            if(character.Veolicty.y < 0)
            {
                if(character.SpringJump())
                {
                    animator.SetTrigger(ANIM_TRIGGER_PRESS);
                }
            }
        }
    }
}
