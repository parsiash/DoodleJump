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
                character.StartCoroutine(RocketMoving(character, () => character.DetachRocket(this)));
                transform.SetParent(null);
                _used = true;
            }
        }

        IEnumerator RocketMoving(CharacterController character, System.Action OnFinish)
        {
            Position = character.Position;

            float timer = 0f;
            while(timer <= rocketTime)
            {
                this.SetY(Position.y + speed * Time.deltaTime);
                this.SetX(character.Position.x);
                
                character.SetY(Position.y);

                yield return null;
                timer += Time.deltaTime;
            }

            character.Jump(speed);

            if(OnFinish != null)
            {
                OnFinish();
            }
        }
    }
}
