using System;
using UnityEngine;

namespace GameSystem
{
    public class GamePauseController
    {
        private readonly GamePauseMenu _menu;

        public GamePauseController(GamePauseMenu menu)
        {
            _menu = menu != null ? menu : throw new ArgumentNullException(nameof(menu));
        }

        public bool IsPaused { get; private set; }

        public event Action OnGamePaused;
        public event Action OnGameUnpaused;

        public void PauseGame()
        {
            if (IsPaused)
                return;

            Time.timeScale = 0.0f;
            _menu.OpenMenu();
            IsPaused = true;

            OnGamePaused?.Invoke();
        }

        public void UnpauseGame()
        {
            if (!IsPaused)
                return;
            
            Time.timeScale = 1.0f;
            _menu.CloseMenu();
            IsPaused = false;

            OnGameUnpaused?.Invoke();
        }
    }
}
