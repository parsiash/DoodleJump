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
        private GameplayController _gameplayController;

        [SerializeField] private TextMeshProUGUI _scoreText;

        public void Initialize(GameplayController gameplayController)
        {
            _gameplayController = gameplayController;
        }

        void Update()
        {
            if(_gameplayController)
            {
                _scoreText.text = _gameplayController.Score.ToString();
            }
        }
    }
}
