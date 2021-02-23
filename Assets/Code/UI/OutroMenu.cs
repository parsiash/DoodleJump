using System;
using System.Collections;
using System.Collections.Generic;

namespace DoodleJump.UI
{
    public class OutroMenu : UIComponent
    {
        private LoseMenu loseMenu => GetCachedComponentInChildren<LoseMenu>();

        public void Show(int score, Action onTryAgainCallback)
        {
            loseMenu.Show(score, onTryAgainCallback);
            SetActive(true);
        }

        public void Hide()
        {
            SetActive(false);
        }
    }
}