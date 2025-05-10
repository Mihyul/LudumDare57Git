using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem
{
    public class GamePauseMenu : AMenu
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private GameObject settingsTab;

        public event Action OnResumeButtonClicked;
        public event Action OnQuitButtonClicked;

        protected override void OnMenuOpened()
        {
            resumeButton.onClick.AddListener(InvokeResumeButtonEvent);
            quitButton.onClick.AddListener(InvokeQuitButtonEvent);
            settingsButton.onClick.AddListener(OpenAudioSettings);
        }

        protected override void OnMenuclosed()
        {
            resumeButton.onClick.RemoveListener(InvokeResumeButtonEvent);
            quitButton.onClick.RemoveListener(InvokeQuitButtonEvent);
            settingsButton.onClick.RemoveListener(OpenAudioSettings);

            settingsTab.SetActive(false);
        }

        private void InvokeResumeButtonEvent()
        {
            OnResumeButtonClicked?.Invoke();
        }

        private void InvokeQuitButtonEvent()
        {
            OnQuitButtonClicked?.Invoke();
        }

        private void OpenAudioSettings()
        {
            settingsTab.SetActive(true);
        }
    }
}
