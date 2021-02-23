using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DoodleJump.UI
{
    public class LoseMenu : UIComponent
    {
        [SerializeField] private Button tryAgainButton;
        [SerializeField] private KeyValueUI scoreUI;
        private Action OnTryAgainCallback;

        public void Show(int score, Action onTryAgainCallback)
        {
            scoreUI.SetValue(score);

            SetButtonCallback(tryAgainButton, onTryAgainCallback);
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
    }
}