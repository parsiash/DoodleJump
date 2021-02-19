using DoodleJump.Common;
using UnityEngine;

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
