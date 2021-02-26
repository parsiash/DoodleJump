using System.Collections;
using System.Collections.Generic;
using DoodleJump.Common;
using DoodleJump.Gameplay;
using UnityEngine;
using TMPro;

namespace DoodleJump.UI
{
    public class HUD : CommonBehaviour
    {
        private IWorld _world;

        [SerializeField] private TextMeshProUGUI _scoreText;

        public void Initialize(IWorld world)
        {
            _world = world;
        }

        void Update()
        {
            if(_world != null)
            {
                _scoreText.text = $"{_world.Score} / {_world.JumpedPlatformCount}";
            }
        }
    }
}
