using System;
using UnityEngine;

namespace GameSystem
{
    public class GameWinController
    {
        private readonly GameWinMenu _menu;

        public GameWinController(GameWinMenu menu)
        {
            _menu = menu != null ? menu : throw new ArgumentNullException(nameof(menu));
        }

        public event Action OnGameWon;

        public void WinGame()
        {
            Time.timeScale = 0f;
            _menu.OpenMenu();
            OnGameWon?.Invoke();
        }
    }
}
