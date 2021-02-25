using System;
using System.Collections;
using System.Collections.Generic;

namespace DoodleJump.UI
{
    public class OutroMenu : UIComponent
    {
        private IPlayerDataRepository playerDataRepository => PlayerDataRepository.Instance;
        private LoseMenu loseMenu => GetCachedComponentInChildren<LoseMenu>();

        public void Show(int score, Action onTryAgainCallback)
        {
            var highScore = playerDataRepository.GetHighScore();
            if(highScore == null)
            {
                highScore = new PlayerScoreData("Loser", score);
            }else if(highScore.Score < score)
            {
                highScore.Score = score;
            }
            playerDataRepository.SaveHighScore(highScore);

            loseMenu.Show(score, highScore.Score, onTryAgainCallback);
            SetActive(true);
        }

        public void Hide()
        {
            SetActive(false);
        }
    }
}