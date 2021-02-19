using DoodleJump.Common;

namespace DoodleJump
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        private Common.ILogger _logger;

        public override void Init()
        {
            base.Init();
            
            _logger = new Logger();
            _logger.Log("Game Manager Initialized");
        }
    }
}
