using FlashlightSystem;
using OxygenSystem;
using System;
using UnityEngine;

namespace GameSystem
{
    public class GameStartController
    {
        private readonly GameObject _player;
        private readonly Transform _startPoint;
        private readonly GameStartMenu _gameStartMenu;
        private readonly OxygenTank _oxygenTank;
        private readonly Flashlight _flashlightView;

        private const int _startOxygen = 30;
        private const int _startFlashlightMode = 0;

        public GameStartController(GameObject player,
                                   Transform startPoint,
                                   GameStartMenu gameStartMenu,
                                   OxygenTank oxygenTank,
                                   Flashlight flashlightView)
        {
            _player = player != null ? player : throw new ArgumentNullException(nameof(player));
            _startPoint = startPoint != null ? startPoint : throw new ArgumentNullException(nameof(startPoint));
            _gameStartMenu = gameStartMenu != null ? gameStartMenu : throw new ArgumentNullException(nameof(gameStartMenu));
            _oxygenTank = oxygenTank ?? throw new ArgumentNullException(nameof(oxygenTank));
            _flashlightView = flashlightView != null ? flashlightView : throw new ArgumentNullException(nameof(flashlightView));
        }

        public event Action OnGameStarted;

        public void StartGame()
        {
            _player.transform.position = _startPoint.position;
            _player.SetActive(true);

            _gameStartMenu.CloseMenu();

            _oxygenTank.OxygenAmount = _startOxygen;
            _flashlightView.SwitchToConfiguration(_startFlashlightMode);

            Time.timeScale = 1.0f;

            OnGameStarted?.Invoke();
        }
    }
}
