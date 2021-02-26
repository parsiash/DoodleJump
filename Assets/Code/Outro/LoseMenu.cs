using System;
using DoodleJump.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DoodleJump.UI
{
    public class LoseMenu : UIComponent
    {
        [SerializeField] private Button tryAgainButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private KeyValueUI scoreUI;
        [SerializeField] private KeyValueUI highScoreUI;
        private Action OnTryAgainCallback;

        public void Show(int score, int highScore, Action onTryAgainCallback)
        {
            scoreUI.SetValue(score);
            highScoreUI.SetValue(highScore);

            SetButtonCallback(tryAgainButton, onTryAgainCallback);
            SetButtonCallback(exitButton, OnExitButtonClick);
        }

        private void SetButtonCallback(Button button, Action callback)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(new UnityAction(callback));
        }

        public void OnTryAgainButtonClick()
        {
            OnTryAgainCallback();
        }

        public void OnExitButtonClick()
        {
            Common.Logger.Instance.Log("Exit Application");
            Application.Quit();
        }
    }
}