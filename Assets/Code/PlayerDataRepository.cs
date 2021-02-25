using DoodleJump.Common;

namespace DoodleJump
{
    public interface IPlayerDataRepository
    {
        PlayerLocalData GetPlayerData();
        void SavePlayerData(PlayerLocalData playerData);
    }

    public static class PlayerDataRepositoryExtensions
    {
        public static void SaveHighScore(this IPlayerDataRepository dataRepository, PlayerScoreData highScore)
        {
            var playerData = dataRepository.GetPlayerData();
            playerData.HighScore = highScore;
            dataRepository.SavePlayerData(playerData);
        }

        public static PlayerScoreData GetHighScore(this IPlayerDataRepository dataRepository)
        {
            return dataRepository.GetPlayerData()?.HighScore;
        }
    }

    public class PlayerDataRepository : Singleton<PlayerDataRepository>, IPlayerDataRepository
    {
        private IObjectStorage objectStorage => LocalObjectStorage.Instance;

        private PlayerLocalData _playerData;

        public PlayerLocalData GetPlayerData()
        {
               if(_playerData == null)
                {
                    _playerData = objectStorage.LoadObject<PlayerLocalData>();
                    if(_playerData == null)
                    {
                        SavePlayerData(new PlayerLocalData());
                    }
                }

                return _playerData;
        }

        public void SavePlayerData(PlayerLocalData playerData)
        {
            _playerData = playerData;
            objectStorage.SaveObject(_playerData);
        }
    }
}