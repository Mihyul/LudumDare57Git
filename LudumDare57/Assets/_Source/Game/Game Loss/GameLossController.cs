using System;
using UnityEngine;

namespace GameSystem
{
    public class GameLossController
    {
        private readonly GameLossMenu _menu;

        public GameLossController(GameLossMenu menu)
        {
            _menu = menu != null ? menu : throw new ArgumentNullException(nameof(menu));
        }

        public event Action OnGameLost;

        public void LoseGame()
        {
            Time.timeScale = 0f;
            _menu.OpenMenu();
            OnGameLost?.Invoke();
        }
    }
}
