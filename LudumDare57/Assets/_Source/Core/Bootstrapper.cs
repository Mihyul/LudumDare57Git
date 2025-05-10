using CustomUtilities;
using EnemySystem;
using FlashlightSystem;
using GameSystem;
using InputSystem;
using InteractionSystem;
using MovementSystem;
using OxygenSystem;
using System;
using UnityEngine;

namespace Core
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private Transform startPoint;

        [SerializeField] private MovementController movementController;
        [SerializeField] private LookController lookController;

        [Space]
        [SerializeField] private Flashlight flashlight;

        [Header("Oxygen")]
        [SerializeField] private OxygenIndicator oxygenTankUIView;
        [SerializeField] private int maxOxygen;
        [SerializeField] private int startOxygen;

        [Space]
        [SerializeField] private CoroutineManager coroutineManager;

        [Header("Interaction")]
        [SerializeField] private InteractionFinder interactionFinder;
        [SerializeField] private AudioSource interactionAudioSource;

        [Header("Game")]
        [SerializeField] private GameStartMenu gameStartMenu;
        [SerializeField] private GamePauseMenu gamePauseMenu;
        [SerializeField] private SceneLoader mainMenuLoader;

        [Header("Win")]
        [SerializeField] private GameWinMenu gameWinMenu;
        [SerializeField] private Bathyscaphe bathyscaphe;
        [SerializeField] private SceneLoader nextLevelLoader;

        [Header("Loss")]
        [SerializeField] private GameLossMenu gameLossMenu;
        [SerializeField] private Target target;
        [SerializeField] private SceneLoader thisLevelLoader;

        private PlayerControls _playerControls;

        private GamePauseController _gamePauseController;
        private GameWinController _gameWinController;
        private GameLossController _gameLossController;
        private GameStartController _gameStartController;

        private OxygenConsumer _oxygenConsumer;

        private Action<int> _dieInstantly;

        private InputListener _inputListener;

        private void Awake()
        {
            Time.timeScale = 0.0f;
        }

        private void Start()
        {
            _playerControls = new();

            _gamePauseController = new(gamePauseMenu);
            _gamePauseController.OnGamePaused += _playerControls.Movement.Disable;
            _gamePauseController.OnGamePaused += _playerControls.Flashlight.Disable;
            _gamePauseController.OnGamePaused += _playerControls.Interaction.Disable;
            _gamePauseController.OnGameUnpaused += _playerControls.Movement.Enable;
            _gamePauseController.OnGameUnpaused += _playerControls.Flashlight.Enable;
            _gamePauseController.OnGameUnpaused += _playerControls.Interaction.Enable;
            gamePauseMenu.OnQuitButtonClicked += mainMenuLoader.LoadScene;
            gamePauseMenu.OnResumeButtonClicked += _gamePauseController.UnpauseGame;

            _gameWinController = new(gameWinMenu);
            _gameWinController.OnGameWon += _playerControls.Movement.Disable;
            _gameWinController.OnGameWon += _playerControls.Flashlight.Disable;
            _gameWinController.OnGameWon += _playerControls.Interaction.Disable;
            _gameWinController.OnGameWon += _playerControls.Pause.Disable;
            gameWinMenu.OnNextButtonClicked += nextLevelLoader.LoadScene;
            gameWinMenu.OnQuitButtonClicked += mainMenuLoader.LoadScene;
            bathyscaphe.OnBathyscapheEntered += _gameWinController.WinGame;

            _gameLossController = new(gameLossMenu);
            _gameLossController.OnGameLost += _playerControls.Movement.Disable;
            _gameLossController.OnGameLost += _playerControls.Flashlight.Disable;
            _gameLossController.OnGameLost += _playerControls.Interaction.Disable;
            _gameLossController.OnGameLost += _playerControls.Pause.Disable;
            gameLossMenu.OnRestartButtonClicked += thisLevelLoader.LoadScene;
            gameLossMenu.OnQuitButtonClicked += mainMenuLoader.LoadScene;
            _dieInstantly = new(DieInstantly);
            target.OndamageRecieved += _dieInstantly.Invoke;
            void DieInstantly(int damage)
            {
                _gameLossController.LoseGame();
            }

            InteractionData interactionData = new();
            interactionData.TryAddService(interactionAudioSource);
            InteractionController interactionController = new(interactionFinder, interactionData);

            OxygenTank oxygenTank = new(maxOxygen);
            _oxygenConsumer = new(1.0f, oxygenTank, coroutineManager);
            _oxygenConsumer.OnSuffocationStarted += _gameLossController.LoseGame;
            OxygenTankUIController oxygenTankUIController = new(oxygenTankUIView);
            oxygenTankUIController.DisplayOxygenTank(oxygenTank);
            interactionData.TryAddService(oxygenTank);

            _gameStartController = new(player, startPoint, gameStartMenu, oxygenTank, flashlight);
            _gameStartController.OnGameStarted += _playerControls.Movement.Enable;
            _gameStartController.OnGameStarted += _playerControls.Flashlight.Enable;
            _gameStartController.OnGameStarted += _playerControls.Interaction.Enable;
            _gameStartController.OnGameStarted += _playerControls.Pause.Enable;
            gameStartMenu.OnStartButtonClicked += _gameStartController.StartGame;
            gameStartMenu.OpenMenu();

            _inputListener = new(movementController, lookController, interactionController, flashlight, _gamePauseController);
            _inputListener.SetupInputActions(_playerControls);
        }

        private void OnDisable()
        {
            _gamePauseController.OnGamePaused -= _playerControls.Movement.Disable;
            _gamePauseController.OnGamePaused -= _playerControls.Flashlight.Disable;
            _gamePauseController.OnGamePaused -= _playerControls.Interaction.Disable;
            _gamePauseController.OnGameUnpaused -= _playerControls.Movement.Enable;
            _gamePauseController.OnGameUnpaused -= _playerControls.Flashlight.Enable;
            _gamePauseController.OnGameUnpaused -= _playerControls.Interaction.Enable;
            gamePauseMenu.OnQuitButtonClicked -= mainMenuLoader.LoadScene;
            gamePauseMenu.OnResumeButtonClicked -= _gamePauseController.UnpauseGame;

            _gameWinController.OnGameWon -= _playerControls.Movement.Disable;
            _gameWinController.OnGameWon -= _playerControls.Flashlight.Disable;
            _gameWinController.OnGameWon -= _playerControls.Interaction.Disable;
            _gameWinController.OnGameWon -= _playerControls.Pause.Disable;
            gameWinMenu.OnNextButtonClicked -= nextLevelLoader.LoadScene;
            gameWinMenu.OnQuitButtonClicked -= mainMenuLoader.LoadScene;
            bathyscaphe.OnBathyscapheEntered -= _gameWinController.WinGame;

            _gameLossController.OnGameLost -= _playerControls.Movement.Disable;
            _gameLossController.OnGameLost -= _playerControls.Flashlight.Disable;
            _gameLossController.OnGameLost -= _playerControls.Interaction.Disable;
            _gameLossController.OnGameLost -= _playerControls.Pause.Disable;
            gameLossMenu.OnRestartButtonClicked -= thisLevelLoader.LoadScene;
            gameLossMenu.OnQuitButtonClicked -= mainMenuLoader.LoadScene;
            target.OndamageRecieved -= _dieInstantly.Invoke;
            _oxygenConsumer.OnSuffocationStarted -= _gameLossController.LoseGame;

            _gameStartController.OnGameStarted -= _playerControls.Movement.Enable;
            _gameStartController.OnGameStarted -= _playerControls.Flashlight.Enable;
            _gameStartController.OnGameStarted -= _playerControls.Interaction.Enable;
            _gameStartController.OnGameStarted -= _playerControls.Pause.Enable;
            gameStartMenu.OnStartButtonClicked -= _gameStartController.StartGame;

            _inputListener.UnsetupInputActions();
        }
    }
}
