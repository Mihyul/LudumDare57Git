using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem
{
    public class GameLossMenu : AMenu
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private Button quitButton;

        public event Action OnRestartButtonClicked;
        public event Action OnQuitButtonClicked;

        protected override void OnMenuOpened()
        {
            restartButton.onClick.AddListener(InvokeRestartButtonEvent);
            quitButton.onClick.AddListener(InvokeQuitButtonEvent);
        }

        protected override void OnMenuclosed()
        {
            restartButton.onClick.RemoveListener(InvokeRestartButtonEvent);
            quitButton.onClick.RemoveListener(InvokeQuitButtonEvent);
        }

        private void InvokeRestartButtonEvent()
        {
            OnRestartButtonClicked?.Invoke();
        }

        private void InvokeQuitButtonEvent()
        {
            OnQuitButtonClicked?.Invoke();
        }
    }
}
