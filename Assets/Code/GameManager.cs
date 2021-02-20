using System;
using DoodleJump.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DoodleJump
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        private Common.ILogger _logger;

        public override void Init()
        {
            base.Init();
            
            _logger = new Common.Logger();
            _logger.Log("Game Manager Initialized");

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scebe, LoadSceneMode loadSceneMode)
        {
            var gameController = UnityEngine.Object.FindObjectOfType<Gameplay.GameplayController>();
            var characterController  = UnityEngine.Object.FindObjectOfType<Gameplay.CharacterController>();
            if(!gameController)
            {
                _logger.LogError($"Cannot start the game. No {nameof(Gameplay.GameplayController)} found.");
                return;
            }
            
            gameController.Initialize(null, characterController);
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
        }
    }
}
