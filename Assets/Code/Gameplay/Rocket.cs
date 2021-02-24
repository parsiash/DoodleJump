using System.Collections;
using UnityEngine;

namespace DoodleJump.Gameplay
{
    public interface ICollectible
    {
        void OnCollected(CharacterController character);
    }

    public class Rocket : Entity, ICollectible
    {
        private bool _used;
        [SerializeField] private float rocketTime;
        [SerializeField] private float speed;


        public override void Reset()
        {
            base.Reset();

            _used = false;
        }

        public void OnCollected(CharacterController character)
        {
            if(_used)
            {
                return;
            }

            if(character.AttachRocket(this))
            {
                _used = true;
                Destroy();
            }
        }
    }
}
