using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem
{
    public class GameWinMenu : AMenu
    {
        [SerializeField] private Button nextButton;
        [SerializeField] private Button quitButton;

        public event Action OnNextButtonClicked;
        public event Action OnQuitButtonClicked;

        protected override void OnMenuOpened()
        {
            nextButton.onClick.AddListener(InvokeNextButtonEvent);
            quitButton.onClick.AddListener(InvokeQuitButtonEvent);
        }

        protected override void OnMenuclosed()
        {
            nextButton.onClick.RemoveListener(InvokeNextButtonEvent);
            quitButton.onClick.RemoveListener(InvokeQuitButtonEvent);
        }

        private void InvokeNextButtonEvent()
        {
            OnNextButtonClicked?.Invoke();
        }

        private void InvokeQuitButtonEvent()
        {
            OnQuitButtonClicked?.Invoke();
        }
    }
}
