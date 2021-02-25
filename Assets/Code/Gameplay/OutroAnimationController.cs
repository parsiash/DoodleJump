using System;
using System.Collections;
using DoodleJump.Common;
using UnityEngine;

namespace DoodleJump.Gameplay
{
    public class OutroAnimationController : CommonBehaviour
    {
        [SerializeField] private float outroAnimationTime = 2f;
        [SerializeField] private float cameraMovementSpeed = 10f;

        private UniversalCamera universalCamera => UniversalCamera.Instance;
        private Coroutine _outroCoroutine;

        public void StartOutroAnimation(CharacterController character, Action OnFinishCallback)
        {
            StopOutroAnimation();

            _outroCoroutine = StartCoroutine(OutroAnimation(character, OnFinishCallback));
        }

        public void Reset()
        {
            StopOutroAnimation();
        }

        private void StopOutroAnimation()
        {
            if(_outroCoroutine != null)
            {
                StopCoroutine(_outroCoroutine);
                _outroCoroutine = null;
            }
        }

        private IEnumerator OutroAnimation(CharacterController character, Action OnFinishCallback)
        {
            float timer = 0f;

            while(timer <= outroAnimationTime)
            {
                var characterPosition = character.Position;
                var cameraPosition = universalCamera.Position;

                Vector2 direction = Vector2.down;
                universalCamera.Move(direction * cameraMovementSpeed * Time.deltaTime);

                yield return null;
                timer += Time.deltaTime;
            }

            if(OnFinishCallback != null)
            {
                OnFinishCallback();
            }

            _outroCoroutine = null;
        }
    }
}
