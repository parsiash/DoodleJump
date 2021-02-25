using System;
using System.Collections.Generic;
using DoodleJump.Common;
using UnityEngine;

namespace DoodleJump.Gameplay
{
    public class IntroAnimationController : CommonBehaviour
    {
        private UniversalCamera universalCamera => UniversalCamera.Instance;
        private Action _OnStartGameCallback;

        public void StartIntroAnimation(Action OnStartGameCallback)
        {
            SetActive(true);
            _OnStartGameCallback = OnStartGameCallback;
        }

        public void Reset()
        {
            SetActive(false);
            _OnStartGameCallback = null;
        }

        public void OnStartGameAnimationEvent()
        {
            if(_OnStartGameCallback != null)
            {
                _OnStartGameCallback();
                Reset();
            }
        }
    }
}
